using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingLava : MonoBehaviour
{
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(5f, 0f, 0f, Space.Self);
        // transform.Rotate(Vector3.up * 50 * Time.deltaTime, Space.Self);
       // transform.rotation = new Quaternion(rotx, roty, rotz, rotw);
       // transform.Rotate(0f, 360f, 0f, Space.Self);
        
        //= transform.position + transform.forward * Time.deltaTime *2;

        transform.parent.transform.Translate(0.02f,0f,0f, Space.Self);
        transform.Rotate(0, 2 * Time.deltaTime, 0);

        float degrees = 90;
        Vector3 to = new Vector3(degrees, 0, 0);
       //transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
           var playerMov = other.GetComponent<PlayerMovement>();
           playerMov.JumpInput(10);
           playerMov.TakeDamage(50);
        }

    }
}
