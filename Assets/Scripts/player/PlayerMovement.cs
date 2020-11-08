using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float health = 50f;

    public CharacterController controller;
    public GameObject head;

    public float speed = 12f;
    public float gravity = -19.81f;
    public float jumpHeight = 3f;
    public float mass = 3f;

    public Transform playerView;     // Camera
    public float playerViewYOffset = .6f; // The height at which the camera is bound to
    public float fpsDisplayRate = 4.0f; // 4 updates per sec

    private int frameCount = 0;
    private float dt = 0.0f;
    private float fps = 0.0f;

    private Vector3 velocity;
    private Vector3 impact = Vector3.zero;
    private float x = 0; //user input
    private float z = 0;
    private float xRaw = 0; //user input
    private float zRaw = 0;
    private Vector3 move;
    private Vector3 moveRaw;


    private Vector3 olderMove = Vector3.zero;
    private Vector3 olderMoveRaw = Vector3.zero;
    private Vector3 desiredDirection = Vector3.zero;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private bool canDoubleJump;
    public bool isGrounded;

    public bool inputLocked = false;

    public AudioSource audioSource;
    public AudioClip dash;
    public AudioClip jump;
    public AudioClip[] steps;
    public float volume=0.5f;
    private float nextFootstep = 0;
    public float footstepDelay = .3f;



    private void Start()
    {
        if (playerView == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
                playerView = mainCamera.gameObject.transform;
        }

        // Put the camera inside the capsule collider
        //this bullshit is centering the camera on the player body and not the head
        /*playerView.position = new Vector3(
            head.transform.position.x,
            head.transform.position.y,// + playerViewYOffset,
            head.transform.position.z);*/

    }
    void Update()
    {
        // Do FPS calculation
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0 / fpsDisplayRate)
        {
            fps = Mathf.Round(frameCount / dt);
            frameCount = 0;
            dt -= 1.0f / fpsDisplayRate;
        }
        /* Ensure that the cursor is locked into the screen */
        if (Cursor.lockState != CursorLockMode.Locked) {
            if (Input.GetButtonDown("Fire1"))
                Cursor.lockState = CursorLockMode.Locked;
        }
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        GetInputWASD();

        transform.forward = new Vector3(playerView.transform.forward.x, 0f, playerView.transform.forward.z);

        PlayFootSteps();

        if (isGrounded)
            GroundMove();
        else if (!isGrounded)
            AirMove();

        //SwitchedDirection();
      MoveState();
        


    }

    private void MoveState()
    {


        if (inputLocked)
        {
          if (desiredDirection.z > 0 && zRaw != -1) z = 1f;
          if (desiredDirection.z < 0 && zRaw != 1) z = -1f;
          if (desiredDirection.x > 0 && xRaw != -1) x = 1f;
          if (desiredDirection.x < 0 && xRaw != 1) x = -1f;  

          if(desiredDirection == Vector3.zero       ||
            (desiredDirection.z > 0 && zRaw == -1)  ||
            (desiredDirection.z < 0 && zRaw == 1)   ||
            (desiredDirection.x > 0 && xRaw == -1)  ||
            (desiredDirection.x < 0 && xRaw == 1))
          inputLocked = false;
        }

        move = transform.right * x + transform.forward * z;
        moveRaw = transform.right * xRaw + transform.forward * zRaw;

        Vector3.Normalize(moveRaw);
        Vector3.Normalize(move);
        


        /*if (inputLocked)
        {
          if( olderMoveRaw.z >= 0  && zRaw == -1) inputLocked = false;
          else move = olderMove;
        }*/
        controller.Move(move * speed * Time.deltaTime); //Time.unscaledDeltaTime
        controller.Move(velocity * Time.deltaTime);

        if (impact.magnitude > 0.2) controller.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5*Time.deltaTime);

        if (!inputLocked){
        olderMove = move;
        olderMoveRaw = moveRaw;
        }

    }

    void GroundMove()
    {
        inputLocked = false;
        canDoubleJump = true;
        if (Input.GetButtonDown("Jump"))
        {
            audioSource.PlayOneShot(jump, 1f);
            Jump();
            inputLocked = true;
            desiredDirection = new Vector3(xRaw,0,zRaw);
        }
    }
    void AirMove()
    {        
        velocity.y += gravity * Time.deltaTime; //apply gravity

        if (Input.GetButtonDown("Jump") && canDoubleJump)
        {
          Dash();
          inputLocked = true;
          desiredDirection = new Vector3(xRaw,0,zRaw);
        }


        //if player collides with ceiling he slowly loses height instead of floating agaisnt the ceilling
        if (((controller.collisionFlags & CollisionFlags.Above) != 0) && velocity.y > 0) velocity.y -= .2f;

        Mathf.Clamp(x, -.3f, .3f); //reduce sideways movement, do this by the end so we can use raw input for checks


    }
    private void PlayFootSteps()
    {
      if(isGrounded)
      {
        //if (x < 1f && x > -1f && z < 1f && z > -1f)                         nextFootstep = 0;
        if (Mathf.Approximately(x,0f) && Mathf.Approximately(z,0f))   nextFootstep = 0; //if player stops movement reset
        //maybe add steps when  adadadadadad or wswswswswswsw

        if (Mathf.Approximately(x,1f) || Mathf.Approximately(x,-1f)  
        || Mathf.Approximately(z,1f)  || Mathf.Approximately(z,-1f)) 
        {  
            if (nextFootstep <= 0) 
            {
                nextFootstep -= Time.deltaTime;
                audioSource.PlayOneShot(steps[Random.Range(0, steps.Length)], volume);
                nextFootstep += footstepDelay;     
            }
            else
            {
              nextFootstep -= Time.deltaTime;
              if (nextFootstep <= 0)
              {
                audioSource.PlayOneShot(steps[Random.Range(0, steps.Length)], volume);
                nextFootstep += footstepDelay;
              }
            }
              
        }
      }
      else nextFootstep = 0;
    }
    public void JumpInput(float height){velocity.y = Mathf.Sqrt(height * -2f * gravity);}
    public void Jump() { velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);}
    private void Dash()
    {
      canDoubleJump = false;
      audioSource.PlayOneShot(dash, volume - .1f);

      if (Input.GetButton("Fire3"))
      {
        AddImpact(Vector3.up, 75); 
        JumpInput(3f); 
        return;
      }

      if (moveRaw != Vector3.zero)
      {
        AddImpact(moveRaw, 50); 
        JumpInput(.5f); 
        return; 
      }
      AddImpact(Vector3.up, 75); 
      JumpInput(3f);    
      //Mathf.Approximately(xRaw,1f) || Mathf.Approximately(xRaw,-1f) || Mathf.Approximately(zRaw,1f) || Mathf.Approximately(zRaw,-1f)
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

    private void GetSmoothRawAxis(string name, float sensitivity, float gravity) //ref float axis
    {
      var r = Input.GetAxisRaw(name);
      var s = sensitivity;
      var g = gravity;
      var t = Time.unscaledDeltaTime;
  
      if (r != 0)
      {
          currentSpeed = Mathf.Clamp(currentSpeed + r * s * t, -1f, 1f);
      }
      else
      {
          currentSpeed = Mathf.Clamp01(Mathf.Abs(currentSpeed) - g * t) * Mathf.Sign(currentSpeed);
      }
    }

    public float acceleration = 100f;
    public float deceleration = 2f;
    public float currentSpeed = 0;

    private void UpdateInputGround()
     {
         if (z == -1) z = -.85f; //do this by the end so we can use raw input for checks
         float acceleratingZ = Input.GetAxisRaw("Vertical");
         if (!Mathf.Approximately(acceleratingZ,0f) || currentSpeed > 0)
         {
             if (acceleratingZ > 0)
             {
                 currentSpeed = Mathf.Clamp01(currentSpeed + acceleration * Time.deltaTime);
             }
             else
             {
                 currentSpeed = Mathf.Clamp01(currentSpeed - deceleration * Time.deltaTime);
             }
         }

         float acceleratingX = Input.GetAxisRaw("Vertical");
         if (!Mathf.Approximately(acceleratingX,0f) || currentSpeed > 0)
         {
             if (acceleratingX > 0)
             {
                 currentSpeed = Mathf.Clamp01(currentSpeed + acceleration * Time.deltaTime);
             }
             else
             {
                 currentSpeed = Mathf.Clamp01(currentSpeed - deceleration * Time.deltaTime);
             }
         }
     }

    public void TakeDamage(float amount){
      health-= amount;
      if (health <= 0f){
        Die();
      }
    }

    void Die(){
      Destroy(gameObject);
    }

    IEnumerator JumpWaiter(){
      yield return new WaitForSeconds(1.2f);
      inputLocked = false;
    }
}
