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
        player = GameObject.FindGameObjectWithTag("Dude").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.transform.Translate(1f * Time.deltaTime, 0f,0f, Space.Self);
        transform.Rotate(0, 1 * Time.deltaTime, 0);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Dude"))
        {
            var playerMov = other.gameObject.GetComponent<PlayerMovement>();
            playerMov.TakeDamage(damage);
            playerMov.AddImpact(-1 * transform.forward, 100f);
        }
        else if (other.gameObject.CompareTag("Monster"))
        {
            Debug.Log("collided with monster");
            var monster = other.GetComponent<Rigidbody>();
            monster.AddForce(transform.forward * -750f);
        }
    }
}
