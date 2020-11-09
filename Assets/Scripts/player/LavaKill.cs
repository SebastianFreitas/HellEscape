using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaKill : MonoBehaviour
{
  private PlayerMovement playerMov;

   void OnTriggerEnter(Collider other)
  {
      if (other.gameObject.tag == "Player")
          playerMov = other.GetComponent<PlayerMovement>();
          playerMov.JumpInput(5);
          playerMov.TakeDamage(10);
  }

  void OnCollisionEnter(Collision collision)
  {
      ContactPoint contact = collision.contacts[0];
      Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
      Vector3 position = contact.point;
      //Instantiate(explosionPrefab, position, rotation);
      Destroy(gameObject);
  }
}
