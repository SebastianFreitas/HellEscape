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
        transform.position = transform.position + transform.forward * Time.deltaTime *2;
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
           var playerMov = other.GetComponent<PlayerMovement>();
           playerMov.JumpInput(10);
           playerMov.TakeDamage(50);
        }

    }
}
