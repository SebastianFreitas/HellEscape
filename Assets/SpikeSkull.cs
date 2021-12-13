using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSkull : Monster
{

    [SerializeField] float maxWaitingTime;
    [SerializeField] float minWaitingTime;

    private RaycastHit hit;

    private LayerMask mask;
    private void OnEnable()
    {
        mask = LayerMask.GetMask("Dude");
        StartCoroutine(Wait());
    }

    private IEnumerator Attack()
    {
        Vector3 playerPos = new Vector3(base.player.transform.position.x, base.player.transform.position.y, base.player.transform.position.z);

        Vector3 direction_to_player = (playerPos - this.transform.position).normalized;

        if (Physics.Linecast(transform.position, playerPos, mask))
        {
           
        }

        var a = Random.Range(minWaitingTime, maxWaitingTime);
        yield return new WaitForSeconds(a);

    }

    private IEnumerator Wait()
    {




        var a = Random.Range(minWaitingTime, maxWaitingTime);
        yield return new WaitForSeconds(a);

    }
}
