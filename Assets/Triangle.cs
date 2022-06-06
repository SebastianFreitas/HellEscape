using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : Monster
{
    [SerializeField] float waitingTime;

    [SerializeField] LazerTrap[] lasers;
    private float finalWaitTime;

    private void Start()
    {
        foreach (var item in lasers)
        {
            item.damage = (int)damage;
        }

        foreach (var item in transform.GetComponentsInChildren<MeshRenderer>())
        {
            item.enabled = false;
        }
    }

    private void OnEnable()
    {

        StartCoroutine(waiterStart());
    }

    IEnumerator waiterStart()
    {
        yield return new WaitForSeconds(.5f);
        foreach (var item in transform.GetComponentsInChildren<MeshRenderer>())
        {
            item.enabled = true;
        }
        finalWaitTime = waitingTime - ((actionSpeed - 1) * waitingTime);
        if (finalWaitTime < 0.4) finalWaitTime = 0.4f;
        StartCoroutine(randomJump());
    }

    IEnumerator randomJump()
    {
        while (true)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance >15)//!Approximately(transform.position, player.transform.position, 5)
            {
                Vector3 direction_to_player = (player.transform.position - this.transform.position).normalized;
                transform.position = transform.position + direction_to_player * Random.Range(1, 10);
            }

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100, -1, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider)
                {
                    foreach (LazerTrap lazer in lasers)
                    {
                        lazer.transform.LookAt(hit.collider.transform);
                    }
                }
            }

            yield return new WaitForSeconds(finalWaitTime);
        }

    }
    private void FixedUpdate()
    {

        Vector3 relativePos = player.transform.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 2 * Time.deltaTime);
    }
    public bool Approximately(Vector3 me, Vector3 other, float allowedDifference)
    {
        var dx = me.x - other.x;
        if (Mathf.Abs(dx) > allowedDifference)
            return false;

        var dy = me.y - other.y;
        if (Mathf.Abs(dy) > allowedDifference)
            return false;

        var dz = me.z - other.z;

        return Mathf.Abs(dz) >= allowedDifference;
    }
}
