using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailSpawner : MonoBehaviour
{
    [SerializeField]  Collider bomb;

    Collider previous;
    void OnEnable()
    {
        StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {
        WaitForSecondsRealtime timer = new WaitForSecondsRealtime(.1f);
        while (true)
        {
            var current = Instantiate(bomb, transform.position, transform.rotation, null) as Collider;

                    
            yield return timer;
        }
    }
}
