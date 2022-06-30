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
        if(lasers.Length > 0)
        {
            foreach (var item in lasers)
            {
                item.damage = (int)damage;
            }
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
        isMoving = false;
        yield return new WaitForSeconds(.5f);
        foreach (var item in transform.GetComponentsInChildren<MeshRenderer>())
        {
            item.enabled = true;
        }
        finalWaitTime = waitingTime - ((actionSpeed - 1) * waitingTime);
        if (finalWaitTime < 0.4) finalWaitTime = 0.4f;
        StartCoroutine(randomJump());
    }
    private bool isMoving = false;
    private bool isBusy = false;

    IEnumerator randomJump()
    {

        while (true)
        {
           
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance >8)
            {
               if(!isMoving) StartCoroutine("WaitAndMove");

                //Vector3 direction_to_player = (player.transform.position - this.transform.position).normalized;
                //transform.position = transform.position + direction_to_player * Random.Range(1, 10);
            }
            
            if(lasers.Length > 0)
            {
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100, -1, QueryTriggerInteraction.Ignore) && (hit.collider))
                {

                    foreach (LazerTrap lazer in lasers)
                    {
                        lazer.transform.LookAt(hit.collider.transform);
                    }
                    
                }
            } 
            else
            {
                if(!isBusy && distance < 5 && lasers.Length <= 0)
                {
                    StartCoroutine("Explode");
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



    IEnumerator WaitAndMove()
    {
        var posA = transform.position;
        var posB = player.transform.position;

        isMoving = true;

        yield return new WaitForSeconds(1); // start at time X
        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        while (Time.time - startTime <= .9)
        { // until one second passed
            transform.position = Vector3.Lerp(posA, posB, Time.time - startTime); // lerp from A to B in one second
            yield return 1; // wait for next frame
        }

        isMoving = false;
    }
    [SerializeField] GameObject ball;
    private IEnumerator Explode()
    {

        isBusy = true;

        var explo = Instantiate(ball, transform);
        explo.SetActive(true);
        explo.GetComponentInChildren<TowerBossExplosion>().StartUp();
        yield return new WaitForSecondsRealtime(2f);


        isBusy = false;

    }
}
