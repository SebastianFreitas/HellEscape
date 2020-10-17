using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

    private Transform player;
    private Vector3 target;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector3 (player.position.x, player.position.y, player.position.z);
        StartCoroutine(waiter());
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x == target.x && transform.position.y == target.y){
          transform.position += transform.forward * Time.deltaTime * speed;
        } else transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime); //makes projectile move towards target (not the player, it would instead follow the player in that case)

    }

    IEnumerator waiter(){
      yield return new WaitForSeconds(5f);
      Destroy(gameObject);
    }
}
