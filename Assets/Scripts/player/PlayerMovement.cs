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

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private bool canDoubleJump;
    private bool isGrounded;



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

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        x = Input.GetAxis("Horizontal"); // get movement
        z = Input.GetAxis("Vertical");

        if (isGrounded)
            GroundMove();
        else if (!isGrounded)
            AirMove();

        if (z == -1) z = -.8f;    // make movement backwards somehwat slower than rest
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        controller.Move(velocity * Time.deltaTime);

        if (impact.magnitude > 0.2) controller.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5*Time.deltaTime);
        Debug.Log(controller.velocity);
    }


    void GroundMove()
    {
        canDoubleJump = true;

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
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

    //objectivo é que o double jump deia este controllo adicional no ar, no momento em que o jogador faz um double jump input direcional deve ser as impactfull as ground movement
    void AirMove()
    {
        Debug.Log("In air");
        velocity.y += gravity * Time.deltaTime; //apply gravity

        if (Input.GetButtonDown("Jump") && canDoubleJump)
        {
            Vector3 move = transform.right * x + transform.forward * z;
            if (move != Vector3.zero) AddImpact(move,50);
            else AddImpact(Vector3.up, 50);
            Jump();
            canDoubleJump = false;
            return;
        }

        //if player collides with ceiling he slowly loses height instead of floating agaisnt the ceilling
        if ((controller.collisionFlags & CollisionFlags.Above) != 0) {
          if (velocity.y > 0) {
              velocity.y -= .2f;
          }
        }
        x = x/2; //reduce sideways movement



    }

    public void JumpInput(float height){velocity.y = Mathf.Sqrt(height * -2f * gravity);}
    public void Jump(){velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);}

    public void AddImpact(Vector3 dir, float force){
       dir.Normalize();
       if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
       impact += dir.normalized * force / mass;
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
