using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaKill : MonoBehaviour
{
  public GameObject objToDestroy;

  void OnTriggerEnter(Collider other)
  {
      if (other.gameObject.tag == "Player")
          Destroy(other.gameObject);
  }
}
