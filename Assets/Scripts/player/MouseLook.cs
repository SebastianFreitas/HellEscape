using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//https://www.youtube.com/watch?v=_QajrabyTJc&list=PLPV2KyIb3jR7dFbE2UQYu7QWMdUgDnlnk&index=7
//this video explains this
public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
      // Hide the cursor
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
      /* Camera rotation stuff, mouse controls this shit */
      float mouseX =  Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
      float mouseY =  Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

      xRotation -= mouseY;
      xRotation = Mathf.Clamp(xRotation, -90f, 90f);

      transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
      playerBody.Rotate(Vector3.up * mouseX);
    }
}
