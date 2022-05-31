using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicMovement : MonoBehaviour
{
    public CharacterController controller;
    public PlayerSounds playerSound;

    [Header("Basic Movement")]
    public float speed = 12f;
    public float increasedSpeed = 0;
    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    [Header("Advanced movement")]
    public float gravity = -19.81f;
    public float jumpHeight = 1.5f;
    public float mass = 3f;
    public float dashForce;
    public float JumpDashForce;
    internal Vector3 velocity;
    private Vector3 impact = Vector3.zero;
    private bool canDash = true; //check to stop player from dashing instantly after dashing
    internal bool isSideDashing = false;
    internal bool isJumpDashing = false;
    public float dashCooldown; //ammount of time player needs to wait until dashing again right after dashing
    private Vector3 desiredDirection = Vector3.zero; // used to save the starting state of a jump or dash to simulate inertia
    private bool inputLocked = false;// in this state we are going to lock Horizontal and Vertical input to simulate inertia

    [Header("View")]
    public Transform playerView;     // Camera

    [Header("Input")]
    private float x = 0; //user input
    private float z = 0;
    private float xRaw = 0;
    private float zRaw = 0;
    public Vector3 move;
    internal Vector3 moveRaw;

    [Header("Ground checks")]
    public Transform groundCheck;//GameObject from where we use checkSphere to see if player is grounded
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

    private bool fallingAtSomeSpeed = false;
    private bool isGroundedOlder;
    private float currentSpeed;

    internal Transform lastPos;

    public GrenadeCooldown cd;

    [SerializeField] GameMan manager;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private void Start()
    {
        if (playerView == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null) playerView = mainCamera.gameObject.transform;
        }
    }

    private bool isFireMovement = false;
    private float fireMovementMod = 0;
    internal IEnumerator FireMovement()
    {
        isFireMovement = true;
        fireMovementMod = 10;
        yield return new WaitForSecondsRealtime(3);
        fireMovementMod = 0;
        isFireMovement = false;
    }
    private void OnEnable()
    {

        if (isFireMovement)
        {
            isFireMovement = false;
            fireMovementMod = 0;
        }
        StartCoroutine(waiterDashDuration());

        if (isChilled) increasedSpeed += chillValue;

        canDash = true;
        canJumpDash = true;       

    }

    void Update()
    {


        /* Ensure that the cursor is locked into the screen */
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            if (Input.GetButtonDown("Fire1"))
                Cursor.lockState = CursorLockMode.Locked;
        }

        JumpBuffer();

        IsGrounded();


        if (velocity.y < -15)
        {
            fallingAtSomeSpeed = true; //it will only make the landing sound if landing at a decent speed
            //velocity.y += -.2f;
        }

        if (!isGroundedOlder && isGrounded && fallingAtSomeSpeed)
        {
            fallingAtSomeSpeed = false;
            playerSound.PlayLandSound();
        }
        else 

        GetInputWASD();

        if (isGrounded)
            GroundMove();
        else if (!isGrounded)
            AirMove();

        MoveState();
        playerSound.PlayFootStepsSound(isGrounded, moveRaw, isSideDashing);

        //if (isGrounded) inputLocked = false;
        isGroundedOlder = isGrounded;


    }

    private void JumpBuffer()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    public Animator animator;

    [SerializeField] PlayerInventory playerInv;
    private void MoveState()
    {
        if (transform.position.y < -1000) GetComponent<PlayerHpManager>().TakeDamage(10000000);
        transform.forward = new Vector3(playerView.transform.forward.x, 0f, playerView.transform.forward.z).normalized;   //align view with camera
                                                                                                                         
        Inertia();

        move = transform.right * x + transform.forward * z;
        moveRaw = transform.right * xRaw + transform.forward * zRaw;

        Vector3.Normalize(moveRaw);
        Vector3.Normalize(move);

        currentSpeed = (speed + fireMovementMod) *(1 + ((increasedSpeed + playerInv.increasedMovementSpeed) / 100));

        controller.Move(move * currentSpeed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);

        if (impact.magnitude > 0.2) controller.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);

        if ((x != 0 || z != 0) && OnSlope())
            controller.Move(Vector3.down * controller.height / 2 * slopeForce * Time.deltaTime);
    }

    [SerializeField] float holdTime;
    [SerializeField]  float amountToIncrease;

    int tier = 1;
    float downTimeRight = 0f;
    private float SpeedIncrease()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
             downTimeRight = Time.time;
        }
        if (Input.GetKey(KeyCode.W))
        {
            if ((downTimeRight + holdTime * tier) <= Time.time && tier <= 4)
            {
                tier++;
                AddImpact(transform.forward, 20);

            }
            return amountToIncrease*(tier-1);
        }
        else
        {
            tier = 1;
        }

        return 1;
    }

    private void Inertia()
    {
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
              (desiredDirection.x < 0 && xRaw == 1)) && !isSideDashing) inputLocked = false;
            
                

            //if ((desiredDirection.z - zRaw == 0) || (desiredDirection.x - xRaw == 0)) inputLocked = false;
        }
    }

    public void GainSpeed(int v)
    {
        speed += v;
    }

    void GroundMove()
    {


        coyoteTimeCounter = coyoteTime;

        if (cd.cdUI <= 0)
        {
            canDash = true;
            canJumpDash = true;
        }

        lastPos = transform;
        speed = 12;

        if (isSideDashing) inputLocked = true;
        //else inputLocked = false;

        if (jumpBufferCounter > 0f)
        {
            JumpDash(false);//Jump();
            inputLocked = true;
            desiredDirection = new Vector3(xRaw, 0, zRaw);

        }
        else if (Input.GetButtonDown("Fire2") && canDash)
        {
            canDash = false;
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
        coyoteTimeCounter -= Time.deltaTime;

        if (!isSideDashing)
        {
            //these checks are made to increase gravity in a certain moment of air movement making it feel heavier without reducing height reach
            if (velocity.y > 7) velocity.y += gravity * Time.deltaTime;
            else velocity.y += 3 * gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0f;//while sidedashing no gravity is applied

        }

        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f)
            { //jump normally even while not touched the ground
                inputLocked = false;
                JumpDash(false);
                inputLocked = true;
                desiredDirection = new Vector3(xRaw, 0, zRaw);
            }
            else if (canJumpDash) //dashJump
            {
                inputLocked = false; //remove input lock if player dashes/jumps
                JumpDash(true);
                inputLocked = true;
            }
        }
        else if (Input.GetButtonDown("Fire2") && canDash)
        {
            canDash = false;
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

    private void JumpDash(bool isDash)
    {
        
        AddImpact(Vector3.up, JumpDashForce * strongerDash);
        JumpInput(3f * strongerDash);

        if (isDash)
        {
            cd.startCD((int)dashCooldown);
            playerSound.PlayDashSound();
            canJumpDash = false;
            StartCoroutine(waiterJumpDashDuration());
        }
        else
        {
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }

    }

    private void Dash()
    {
        cd.startCD((int)dashCooldown);
        playerSound.PlayDashSound();
        if (moveRaw == Vector3.zero) AddImpact(transform.forward, 250 * strongerDash);
        else AddImpact(moveRaw, 250 * strongerDash);

        canDash = false;
        isSideDashing = true;
        StartCoroutine(waiterDashDuration());
    }

    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
    }

    private void GetInputWASD()
    {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");

            xRaw = Input.GetAxisRaw("Horizontal");
            zRaw = Input.GetAxisRaw("Vertical");
    }

    private bool OnSlope()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;
        return false;
    }

    private void IsGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask, QueryTriggerInteraction.Ignore);


        if ((controller.collisionFlags & CollisionFlags.Below) != 0)
        {
            isGrounded = true;// second check to see if its grounded dependign on collisions
            //inputLocked = false;
        }
        else
        {
            //inputLocked = true;
            //isGrounded = false;
        }
        if (Physics.Raycast(transform.position, Vector3.down, controller.height / 2 +.4f, groundMask, QueryTriggerInteraction.Ignore))
            isGrounded = true;
    }

    IEnumerator waiterDashDuration()
    {
       
        yield return new WaitForSecondsRealtime(.3f);
        isSideDashing = false;
        inputLocked = false;

    }

    IEnumerator waiterJumpDashDuration()
    {

        yield return new WaitForSecondsRealtime(.3f);
        isJumpDashing = false;
        inputLocked = false;

    }

    private int chillValue = 50;
    private bool isChilled = false;
    private bool canJumpDash;
    internal float strongerDash = 1;

    internal IEnumerator Chilled()
    {
        increasedSpeed -= chillValue;
        isChilled = true;
        yield return new WaitForSecondsRealtime(1f);
        increasedSpeed += chillValue;
        isChilled = false;

    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        var pushPower = 5f;

        if (hit.collider.CompareTag("Bullet") || hit.collider.CompareTag("Monster")) { return; }
        // no rigidbody
        if (body == null || body.isKinematic) { return; }
        // We dont want to push objects below us
        if (hit.moveDirection.y < -.3)
        {
            //velocity = new Vector3( body.velocity.x, body.velocity.z, body.velocity.y);
           
            return;
        } 

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.
        // Apply the push
        var dashPower = 1f;
        if (isSideDashing) dashPower = 5f;
        body.velocity = dashPower * pushPower * pushDir ;
    }

    

}
