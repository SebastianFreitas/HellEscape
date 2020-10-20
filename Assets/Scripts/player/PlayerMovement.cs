using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float health = 50f;

    public CharacterController controller;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float speed = 12f;
    public float gravity = -19.81f;
    public float jumpHeight = 3f;

    public Transform playerView;     // Camera
    public float playerViewYOffset = 0.6f; // The height at which the camera is bound to
    public float xMouseSensitivity = 30.0f;
    public float yMouseSensitivity = 30.0f;
    public float fpsDisplayRate = 4.0f; // 4 updates per sec

    // Camera rotations
    private float rotX = 0.0f;
    private float rotY = 0.0f;

    private int frameCount = 0;
    private float dt = 0.0f;
    private float fps = 0.0f;

    private Vector3 velocity;
    private bool isGrounded;
    private bool canDoubleJump;

    //public int interpolationFramesCount = 1; // Number of frames to completely interpolate between the 2 positions
    //int elapsedFrames = 0;

    private void Start()
    {
        // Hide the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerView == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
                playerView = mainCamera.gameObject.transform;
        }

        // Put the camera inside the capsule collider
        playerView.position = new Vector3(
            transform.position.x,
            transform.position.y + playerViewYOffset,
            transform.position.z);
    }
    // Update is called once per frame
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

      /* Camera rotation stuff, mouse controls this shit */
      rotX -= Input.GetAxisRaw("Mouse Y") * xMouseSensitivity * 0.02f;
      rotY += Input.GetAxisRaw("Mouse X") * yMouseSensitivity * 0.02f;

      // Clamp the X rotation
      if(rotX < -90)
          rotX = -90;
      else if(rotX > 90)
          rotX = 90;

      this.transform.rotation = Quaternion.Euler(0, rotY, 0); // Rotates the collider
      playerView.rotation     = Quaternion.Euler(rotX, rotY, 0); // Rotates the camera

          //float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;
          isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // checks floor by using a cube gameObject

          if (isGrounded && canDoubleJump == false) canDoubleJump = true;

          if (isGrounded && velocity.y < 0) // touching floor
          {
            velocity.y += -0.10f;

            //transform.Translate(Vector3()) * Time.deltaTime, Space.World); tentativa de empurrar o jogador numa direção constantmente
            //Vector3 down = new Vector3(0,(float)-0.01,0);
            //Vector3 interpolatedPosition = Vector3.Lerp(down, Vector3.forward, interpolationRatio);
            //transform.position = transform.position + interpolatedPosition; //+ new  new Vector3(0, -1 * movementSpeed * Time.deltaTime, 0);
          }

          if (Input.GetButtonDown("Jump") && isGrounded){ //jump
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            //isGrounded = false;
          }

          if (Input.GetButtonDown("Jump") && !isGrounded && canDoubleJump){ //jump
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            canDoubleJump = false;
            Debug.Log("Hello:"+gravity);
          }

          float x = Input.GetAxis("Horizontal"); // get movement
          float z = Input.GetAxis("Vertical");

          velocity.y += gravity * Time.deltaTime;


          if (velocity.x > 0 && x < 0) {   // if the player presses to oposite direction he immedially goes to a stop
            velocity.x = 0;

          }

          if (velocity.z > 0 && z < 0 ){
            velocity.z = 0;

          }

          Vector3 move = transform.right * x + transform.forward * z;
          controller.Move(move * speed * Time.deltaTime);

          if (isGrounded && x == 0f && z == 0f)// && velocity.y == 0 )
          { velocity.x = 0; //make him stop
            velocity.z = 0;}


          controller.Move(velocity * Time.deltaTime);

    }

    void OnCollisionEnter(Collision otherObj) //touch lava die
    {
        if (otherObj.gameObject.tag == "Player") {
            Destroy (gameObject, .5f);
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
}
