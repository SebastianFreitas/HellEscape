using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaKill : MonoBehaviour
{
  public GameObject objToDestroy;
  private PlayerMovement playerMov;

   void OnTriggerEnter(Collider other)
  {
      if (other.gameObject.tag == "Player")
          playerMov = other.GetComponent<PlayerMovement>();
          playerMov.JumpInput(5);
          playerMov.TakeDamage(10);
  }
}
