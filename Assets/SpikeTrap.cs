
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private bool hasBeenTriggered = false;
    private bool cr_running= false;

    private void Awake()
    {
        if (Random.Range(1f, 20f) > 15f) gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Dude") )
        {
            if (!hasBeenTriggered && !cr_running) StartCoroutine(SpikeTimer());
            else if (hasBeenTriggered) other.transform.GetComponent<PlayerHpManager>().TakeDamage(10);

        }
    }

    IEnumerator SpikeTimer()
    {
        cr_running = true;

        WaitForSeconds interval = new WaitForSeconds(1f);
        yield return interval;

        Shoot();

        yield return interval;
        Retract();

        cr_running = false;
    }

    private void Shoot()
    {
        hasBeenTriggered = true;
        transform.position = new Vector3(transform.position.x, transform.position.y +2, transform.position.z);

    }

    private void Retract()
    {
        hasBeenTriggered = false;
        transform.position = new Vector3(transform.position.x, transform.position.y - 2, transform.position.z);
    }

    private void OnEnable()
    {
        if (hasBeenTriggered)Retract();
        hasBeenTriggered = false;
    }
}
