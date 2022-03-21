using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
            foreach (Transform child in transform) child.gameObject.SetActive(true);
            transform.GetComponent<BoxCollider>().size = Vector3.zero;

        }
    }
}
