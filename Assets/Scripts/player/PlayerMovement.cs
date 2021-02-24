using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public float health = 50;

  
  public CharacterController controller;
  public GameObject head;
  private bool canTakeDamage = true;

    [Header("Basic Movement")]
  public float speed = 12f;
  public float currentSpeed = 12f;
  public float maxSpeed = 20;
  public float speedModifier = 1;//ammount of speed gained from each speedCounter
  public int speedCounter = 0; //used as stacks to increase speed

  [Header("Advanced movement")]
  public float gravity = -19.81f;
  public float jumpHeight = 1.5f;
  public float mass = 3f;
  public float dashForce;

  [SerializeField] ParticleSystem dashParticleSystem;

  public float JumpDashForce;
  public Vector3 velocity;
  private Vector3 impact = Vector3.zero;
  private bool canDash = true; //check to stop player from dashing instantly after dashing
  private bool isSideDashing = false;
  public float dashCooldown; //ammount of time player needs to wait until dashing again right after dashing
  private Vector3 desiredDirection = Vector3.zero; // used to save the starting state of a jump or dash to simulate inertia
  public bool inputLocked = false;// in this state we are going to lock Horizontal and Vertical input to simulate inertia


  [Header("View")]
  public Transform playerView;     // Camera
  public float fpsDisplayRate = 4.0f; // 4 updates per sec
  private int frameCount = 0;
  private float dt = 0.0f;
  private float fps = 0.0f;

  [Header("Input")]

  public float x = 0; //user input
  public float z = 0;
  private float xRaw = 0; 
  private float zRaw = 0;
  private Vector3 move;
  private Vector3 moveRaw;

  [Header("Ground checks")]  
  public Transform groundCheck;//GameObject from where we use checkSphere to see if player is grounded
  public float groundDistance = 0.4f;
  public LayerMask groundMask;
  public bool isGrounded;

  public bool groundLag;//used to give the player a few frames where he can still jump right after leaving the floor

  public bool fallingAtSomeSpeed = false;

  [Header("Sound")]
  public AudioSource audioSource;
  public AudioClip dash;
  public AudioClip jump;
  public AudioClip land;
  public AudioClip[] steps;
  public AudioClip[] hurts;
  public float volume=0.5f;
  private float nextFootstep = 0;
  public float footstepDelay = .3f;


    public HealthBar hp;
    private bool isGroundedOlder;


    private void Start()
  {

        hp = transform.parent.GetChild(0).GetChild(0).GetChild(0).GetComponent<HealthBar>();
        hp.SetMaxHealth((int)health);

    if (playerView == null)
    {
      Camera mainCamera = Camera.main;
      if (mainCamera != null) playerView = mainCamera.gameObject.transform;     
    }
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
      if ((controller.collisionFlags & CollisionFlags.Below) != 0){
        isGrounded = true;// second check to see if its grounded dependign on collisions
        inputLocked = false;
      } 

      if (velocity.y < -15) fallingAtSomeSpeed = true; //it will only make the landing sound if landing at a decent speed

      if (!isGroundedOlder && isGrounded && fallingAtSomeSpeed) {
        fallingAtSomeSpeed = false;
        audioSource.PlayOneShot(land, volume+.25f);
      } else if (isGroundedOlder && !isGrounded ) StartCoroutine(waiterGroundLag()); 
      
      
      

      GetInputWASD();

      if (isGrounded)
          GroundMove();
      else if (!isGrounded)
          AirMove();
    

    //CheckCollisionPlayer();

    MoveState();
    PlayFootSteps();  
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

      if((desiredDirection == Vector3.zero      ||
        (desiredDirection.z > 0 && zRaw == -1)  ||
        (desiredDirection.z < 0 && zRaw == 1)   ||
        (desiredDirection.x > 0 && xRaw == -1)  ||
        (desiredDirection.x < 0 && xRaw == 1))  && !isSideDashing)
        {
          inputLocked = false;
          speedCounter=0;
        }
    }

    //minimum movement
    //if (x > 0f && x < .1f) x = .1f;
    //if (x < 0f && x > -.1f) x = -.1f;
    //if (z > 0f && z < .1f) z = .1f;
    //if (z < 0f && z > -.1f) z = -.1f;

    move = transform.right * x + transform.forward * z;
    moveRaw = transform.right * xRaw + transform.forward * zRaw;

    Vector3.Normalize(moveRaw);
    Vector3.Normalize(move);


    if (moveRaw == Vector3.zero) speedCounter = 0;
    currentSpeed = speed + (speedCounter * speedModifier);
    if (currentSpeed > maxSpeed) currentSpeed = maxSpeed;

    if (isSideDashing) currentSpeed *= 5;
  
    controller.Move(move * currentSpeed * Time.deltaTime);
    controller.Move(velocity * Time.deltaTime);

    if (impact.magnitude > 0.2) controller.Move(impact * Time.deltaTime);
    // consumes the impact energy each cycle:
    impact = Vector3.Lerp(impact, Vector3.zero, 5*Time.deltaTime);
  }

  void GroundMove()
  {
        if (isSideDashing) inputLocked = true;

  
    if (Input.GetButtonDown("Jump"))
    {
      //audioSource.PlayOneShot(jump, 1f);
      Jump();
      inputLocked = true;
      desiredDirection = new Vector3(xRaw,0,zRaw);
    }
    else if (Input.GetButtonDown("Fire3") && canDash)
    {
      Dash();
      inputLocked = true;
      desiredDirection = new Vector3(xRaw,0,zRaw);
      if (xRaw !=0 || zRaw != 0) desiredDirection = new Vector3(xRaw,0,zRaw);
      else desiredDirection = new Vector3(0,0,1);
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

    if (Input.GetButtonDown("Jump") && canDash )
    {
      if (groundLag){ //jump normally even while not touched the ground
        Jump();
        inputLocked = true;
        desiredDirection = new Vector3(xRaw,0,zRaw);
      } 
      else if (canDash) //dashJump
      {
          inputLocked = false; //remove input lock if player dashes/jumps
          JumpDash();
          inputLocked = true;
      }

    }
    else if (Input.GetButtonDown("Fire3") && canDash )
    {
      inputLocked = false;
      Dash();
      inputLocked = true;
      desiredDirection = new Vector3(xRaw,0,zRaw);
      if (xRaw !=0 || zRaw != 0) desiredDirection = new Vector3(xRaw,0,zRaw);
      else desiredDirection = new Vector3(0,0,1); 
    }
    //if player collides with ceiling he slowly loses height instead of floating agaisnt the ceilling
    if (((controller.collisionFlags & CollisionFlags.Above) != 0) && velocity.y > 0) velocity.y -= .8f;

    Mathf.Clamp(x, -.6f, .6f); //reduce sideways movement
  }

  private void PlayFootSteps()
  {
    if(isGrounded)
    {
      if (moveRaw != Vector3.zero && !isSideDashing)
      {  
        nextFootstep -= Time.deltaTime;
        if (nextFootstep <= 0)
        {
          audioSource.PlayOneShot(steps[Random.Range(0, steps.Length)], volume);
          nextFootstep += footstepDelay;
        }    
      }
    }
    //else nextFootstep = 0;
  }
  public void JumpInput(float height){velocity.y = Mathf.Sqrt(height * -3f * gravity);}

  public void Jump() { velocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);}

  private void Dash()
  {
    dashParticleSystem.Play();
    speedCounter++;
    audioSource.PlayOneShot(dash, volume - .3f);
    if (moveRaw == Vector3.zero) AddImpact(transform.forward, dashForce); 
      else AddImpact(moveRaw, dashForce); 

    canDash = false;
    isSideDashing = true;
    StartCoroutine(waiterDashCD());
    StartCoroutine(waiterDashDuration());
  }

  private void JumpDash()
  {
    //dashParticleSystem.Play();
    audioSource.PlayOneShot(dash, volume - .1f);

    AddImpact(Vector3.up, JumpDashForce); 
    JumpInput(3f);

    canDash = false;
    StartCoroutine(waiterDashCD());
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
    if (!isSideDashing) //prevents modifiyng movement while Dashing
    {
      x = Input.GetAxis("Horizontal");
      z = Input.GetAxis("Vertical");
      xRaw = Input.GetAxisRaw("Horizontal");
      zRaw = Input.GetAxisRaw("Vertical");
    }
  }

  public void TakeDamage(float amount)
  {
        if (canTakeDamage)
        {
            StartCoroutine(waiterImmunity());
            audioSource.PlayOneShot(hurts[Random.Range(0, hurts.Length)], volume+1);

            health -= amount;
            hp.SetHealth((int)health);
            if (health <= 0f)
            {
                Die();
                
            }
            Debug.Log("You took " + amount + " damage.");
        }
  }


  void Die()
  {
        transform.parent.GetComponent<GameMan>().RestartGame();
        Destroy(gameObject);
        
  }

  void CheckCollisionPlayer()
    {
        if (Physics.CheckSphere(transform.position, 5f,LayerMask.NameToLayer("Room")))
        {
           Debug.Log("Entered");
            AddImpact(-transform.forward, 100f);
            TakeDamage(10f);
        }
    }
    IEnumerator waiterImmunity()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(.5f);
        canTakeDamage = true;

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
  }

  IEnumerator waiterGroundLag(){
    groundLag = true;
    yield return new WaitForSeconds(.25f);
    groundLag = false;
  }

  

}
