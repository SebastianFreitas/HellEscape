using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed;
    private Vector3 target;
    public float damage = 50f;
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        //target = new Vector3 (player.position.x, player.position.y, player.position.z);
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        StartCoroutine(waiter());
    }

    void Update()
    {
        //if (transform.position.x == target.x && transform.position.y == target.y){
        //  transform.position += transform.forward * Time.deltaTime * speed;
        //} else transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime); //makes projectile move towards target (not the player, it would instead follow the player in that case)

    }

    IEnumerator waiter(){
      yield return new WaitForSeconds(5f);
      Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collision)
    {
      if (collision.gameObject.tag == "Player")
      collision.GetComponent<PlayerMovement>().TakeDamage((int)damage);
    }

    void OnCollisionEnter(Collision collision)
    {
      Destroy(gameObject);
    }
}
