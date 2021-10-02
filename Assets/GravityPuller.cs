using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPuller : MonoBehaviour
{
    private PlayerBasicMovement player;

    private bool hasBeen = false;
    void Start()
    {
        player = transform.parent.GetComponent<Room>().player.GetComponent<PlayerBasicMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude") && !hasBeen)
        {
            hasBeen = true;
            StartCoroutine(PullPlayer());

        }
    }

    IEnumerator PullPlayer()
    {
        var x = Random.Range(.5f,1.5f);
        yield return new WaitForSeconds(.05f);
        player.AddImpact(new Vector3(0,0.1f,1), 1500);
        StartCoroutine(PullPlayer());
    }
}
