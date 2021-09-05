using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicMovement : MonoBehaviour
{
    public float health = 50;


    public CharacterController controller;
    public GameObject head;

    [Header("Basic Movement")]
    public float speed = 12f;

    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    [Header("Advanced movement")]
    public float gravity = -19.81f;
    public float jumpHeight = 1.5f;
    public float mass = 3f;
    public float dashForce;

    public float JumpDashForce;
    private Vector3 velocity;
    private Vector3 impact = Vector3.zero;
    public bool canDash = true; //check to stop player from dashing instantly after dashing
    public bool isSideDashing = false;
    public float dashCooldown; //ammount of time player needs to wait until dashing again right after dashing
    public Vector3 desiredDirection = Vector3.zero; // used to save the starting state of a jump or dash to simulate inertia
    public bool inputLocked = false;// in this state we are going to lock Horizontal and Vertical input to simulate inertia


    [Header("View")]
    public Transform playerView;     // Camera

    [Header("Input")]

    private float x = 0; //user input
    private float z = 0;
    public float xRaw = 0;
    public float zRaw = 0;
    private Vector3 move;
    public Vector3 moveRaw;

    [Header("Ground checks")]
    public Transform groundCheck;//GameObject from where we use checkSphere to see if player is grounded
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;

    public bool groundLag;//used to give the player a few frames where he can still jump right after leaving the floor

    public bool fallingAtSomeSpeed = false;

    private bool isGroundedOlder;


    private void Start()
    {
        if (playerView == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null) playerView = mainCamera.gameObject.transform;
        }
    }
    void Update()
    {
        /* Ensure that the cursor is locked into the screen */
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            if (Input.GetButtonDown("Fire1"))
                Cursor.lockState = CursorLockMode.Locked;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if ((controller.collisionFlags & CollisionFlags.Below) != 0)
        {
            isGrounded = true;// second check to see if its grounded dependign on collisions
            inputLocked = false;
        }

        if (velocity.y < -15) fallingAtSomeSpeed = true; //it will only make the landing sound if landing at a decent speed

        if (!isGroundedOlder && isGrounded && fallingAtSomeSpeed)
        {
            fallingAtSomeSpeed = false;
            //audioSource.PlayOneShot(land, volume + .25f);
        }
        else if (isGroundedOlder && !isGrounded) StartCoroutine(waiterGroundLag());




        GetInputWASD();

        if (isGrounded)
            GroundMove();
        else if (!isGrounded)
            AirMove();

        MoveState();
        
        isGroundedOlder = isGrounded;
    }


    private void MoveState()
    {
        transform.forward = new Vector3(playerView.transform.forward.x, 0f, playerView.transform.forward.z).normalized;   //align view with camera
                                                                                                                          //inertia
        if (inputLocked)
        {
            if (desiredDirection.z > 0 && zRaw != -1) z = 1f;
            else if (desiredDirection.z < 0 && zRaw != 1) z = -1f;
            else if (desiredDirection.x > 0 && xRaw != -1) x = 1f;
            else if (desiredDirection.x < 0 && xRaw != 1) x = -1f;

            if ((desiredDirection == Vector3.zero ||
              (desiredDirection.z > 0 && zRaw == -1) ||
              (desiredDirection.z < 0 && zRaw == 1) ||
              (desiredDirection.x > 0 && xRaw == -1) ||
              (desiredDirection.x < 0 && xRaw == 1)) && !isSideDashing)
            {
                inputLocked = false;
            }
        }

        move = transform.right * x + transform.forward * z;
        moveRaw = transform.right * xRaw + transform.forward * zRaw;

        Vector3.Normalize(moveRaw);
        Vector3.Normalize(move);



        //if (isSideDashing) speed *= 5;
        //else speed = 12f;

        controller.Move(move * speed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);

        if (impact.magnitude > 0.2) controller.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);

        if ((x != 0 || z != 0) && OnSlope())
            controller.Move(Vector3.down * controller.height / 2 * slopeForce * Time.deltaTime);
    }

    void GroundMove()
    {
        if (isSideDashing) inputLocked = true;
        inputLocked = false;

        if (Input.GetButtonDown("Jump"))
        {
            //audioSource.PlayOneShot(jump, 1f);
            Jump();
            inputLocked = true;
            desiredDirection = new Vector3(xRaw, 0, zRaw);
            groundLag = false;
        }
        else if (Input.GetButtonDown("Fire2") && canDash)
        {
            Dash();
            inputLocked = true;
            desiredDirection = new Vector3(xRaw, 0, zRaw);
            if (xRaw != 0 || zRaw != 0) desiredDirection = new Vector3(xRaw, 0, zRaw);
            else desiredDirection = new Vector3(0, 0, 1);
        }
        else if (velocity.y < 0)
        {
            velocity.y = -1;
            inputLocked = false;
        }
    }
    void AirMove()
    {
        if (!isSideDashing)
        {
            //these checks are made to increase gravity in a certain moment of air movement making it feel heavier without reducing height reach
            if (velocity.y > 7) velocity.y += gravity * Time.deltaTime;
            else velocity.y += 3 * gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0f;//while sidedashing no gravity is applied
            inputLocked = true;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (groundLag)
            { //jump normally even while not touched the ground
                Jump();
                inputLocked = true;
                desiredDirection = new Vector3(xRaw, 0, zRaw);
                groundLag = false;
            }
            else if (canDash) //dashJump
            {
                inputLocked = false; //remove input lock if player dashes/jumps
                JumpDash();
                inputLocked = true;
            }
        }
        else if (Input.GetButtonDown("Fire2") && canDash)
        {
            inputLocked = false;
            Dash();
            inputLocked = true;
            desiredDirection = new Vector3(xRaw, 0, zRaw);
            if (xRaw != 0 || zRaw != 0) desiredDirection = new Vector3(xRaw, 0, zRaw);
            else desiredDirection = new Vector3(0, 0, 1);
        }
        //if player collides with ceiling he slowly loses height instead of floating agaisnt the ceilling
        if (((controller.collisionFlags & CollisionFlags.Above) != 0) && velocity.y > 0) velocity.y -= .8f;

        Mathf.Clamp(x, -.6f, .6f); //reduce sideways movement
    }

    public void JumpInput(float height) { velocity.y = Mathf.Sqrt(height * -3f * gravity); }

    public void Jump() { velocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity); }
    private void JumpDash()
    {
        //dashParticleSystem.Play();
        //audioSource.PlayOneShot(dash, volume / 2);

        AddImpact(Vector3.up, JumpDashForce);
        JumpInput(3f);

        canDash = false;
        StartCoroutine(waiterDashCD());
        StartCoroutine(waiterDashDuration());
    }

    private void Dash()
    {
        //dashParticleSystem.Play();
        //audioSource.PlayOneShot(dash, volume / 2);
        if (moveRaw == Vector3.zero) AddImpact(transform.forward, 250);
        else AddImpact(moveRaw, 250);

        canDash = false;
        isSideDashing = true;
        StartCoroutine(waiterDashCD());
        StartCoroutine(waiterDashDuration());
    }

    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
    }

    public void AddHighImpact(Vector3 dir)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * 400f / mass;
    }

    private void GetInputWASD()
    {
        if (!isSideDashing) //prevents modifiyng movement while Dashing
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
            xRaw = Input.GetAxisRaw("Horizontal");
            zRaw = Input.GetAxisRaw("Vertical");
        }
    }


    private bool OnSlope()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;
        return false;
    }

    IEnumerator waiterDashCD()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    IEnumerator waiterDashDuration()
    {
        yield return new WaitForSeconds(.15f);
        isSideDashing = false;
        inputLocked = false;
    }

    IEnumerator waiterGroundLag()
    {
        groundLag = true;
        yield return new WaitForSeconds(.1f);
        groundLag = false;
    }


}
