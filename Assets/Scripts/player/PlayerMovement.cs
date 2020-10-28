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

    public Transform playerView;     // Camera
    public float playerViewYOffset = .6f; // The height at which the camera is bound to
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
    private float x = 0; //= Input.GetAxis("Horizontal"); // get movement
    private float z = 0; // = Input.GetAxis("Vertical");

    private bool canDoubleJump;



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
            head.transform.position.x,
            head.transform.position.y,// + playerViewYOffset,
            head.transform.position.z);

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

        x = Input.GetAxis("Horizontal"); // get movement
        z = Input.GetAxis("Vertical");

        if (controller.isGrounded)
            GroundMove();
        else if (!controller.isGrounded)
            AirMove();
        if (z == -1) z = -.8f;    // make movement backwards somehwat slower than rest
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        controller.Move(velocity * Time.deltaTime);
        Debug.Log(controller.velocity);
    }


    void GroundMove()
    {
        canDoubleJump = true;

        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            return;
        }
        /*
        if (x == 0f && z == 0f)
        {
            velocity.x = 0; //make him stop
            velocity.z = 0;
            return;
        }

        if (velocity.x > 0 && x < 0)
        {   // if the player presses to oposite direction he immediately goes to a stop
            velocity.x = 0;
            return;
        }

        if (velocity.z > 0 && z < 0)
        {
            velocity.z = 0;
            return;
        }*/

    }
    //to do -> neste preciso momento o jogador tem tanto controllo no ar como no chao this should not be the case
    //objectivo é que o double jump deia este controllo adicional no ar, no momento em que o jogador faz um double jump input direcional deve ser as impactfull as ground movement
    void AirMove()
    {
        //Debug.Log("In air");
        velocity.y += gravity * Time.deltaTime;
        if (Input.GetButtonDown("Jump") && canDoubleJump)
        { //jump
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            canDoubleJump = false;
            return;
        }
        x = x/2; //reduce sideways movement
        if (z < 0) z = -0.5f; //further reduce backwards movement while on air


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
