using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingLava : MonoBehaviour
{
    private Transform player;
    public int damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.transform.Translate(2f * Time.deltaTime, 0f,0f, Space.Self);
        transform.Rotate(0, 2 * Time.deltaTime, 0);
    }
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("collided with player");
            var playerMov = collision.transform.GetComponent<PlayerMovement>();
            playerMov.TakeDamage(damage);
            playerMov.AddImpact(contact.normal, 100f);
        }
        else if (collision.gameObject.CompareTag("Monster"))
        {
            Debug.Log("collided with monster");
            var monster = collision.transform.GetComponent<Monster>();
            monster.gameObject.GetComponent<Rigidbody>().AddForce(contact.normal * 100f);
            monster.TakeDamage(50);
        }
    }
    void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.CompareTag("Player"))
        {
            var playerMov = other.GetComponent<PlayerMovement>();
            playerMov.TakeDamage(damage);
            playerMov.AddImpact(-1 * transform.forward, 250f);
        }
        else if (other.gameObject.CompareTag("Monster"))
        {
            Debug.Log("collided with monster");
            var monster = other.GetComponent<Rigidbody>();
            monster.AddForce(transform.forward * -750f);
        }

    }
}
