using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBoss : Monster
{
    [SerializeField] Transform shootPoint;

    [SerializeField] Transform shootPointdowndown;
    [SerializeField] GameObject turretBullet;
    [SerializeField] float fireRateshotDown;


    [SerializeField] Transform[] steps;
    [SerializeField] float waitingTime;

    [SerializeField] float shotDownSpeed = 100;
    private float finalWaitTime;

    List<string> text = new List<string>();
    int attempts = 0;
    int victories = 0;

    bool cantClimb = false;
    private void Start()
    {

        //if (PlayerPrefs.HasKey("attemptsTower")) attempts = PlayerPrefs.GetInt("attemptsTower");

        text.Add("It's not the first time one of you has made it here.");

       //Speak(text[attempts]); //this crashes stuff

        StartCoroutine("Stop");


        attempts++;
        //PlayerPrefs.SetInt("attemptsTower", attempts);
    }

    private void Speak(string text)
    {
        var direction = transform.position - player.transform.position;
        var rot = Quaternion.LookRotation(direction);

        var x = Instantiate(dmgPopUp, transform.position, rot, null);
        x.GetComponent<KillObject>().timer += 20;
        x.transform.localScale *= 2;

        x.damageLabel.text = text;
        x.damageLabel.color = Color.red;

    }

    private void OnEnable()
    {

        StartCoroutine(waiterStart());
        StartCoroutine(ShotDown());
        cantClimb = false;
    }
    IEnumerator waiterStart()
    {
        yield return new WaitForSeconds(.5f);

        finalWaitTime = waitingTime - ((actionSpeed - 1) * waitingTime);
        if (finalWaitTime < 0.4) finalWaitTime = 0.4f;
        StartCoroutine(randomJump());
    }

    [SerializeField] float phaseOneWaitTime;
    internal IEnumerator Stop()
    {
        cantClimb = true;
        yield return new WaitForSeconds(phaseOneWaitTime);
        cantClimb = false;
    }

    private void FixedUpdate()
    {
        if (transform.position.y < player.transform.position.y + 5 && !cantClimb)
        {
            transform.position += Vector3.up * 10;

        }
    }
    IEnumerator randomJump()
    {
        while (true)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            //if(distance > 5)
            //{
                Vector3 direction_to_player;

                var noheight = player.transform.position;
                noheight.y = transform.position.y;
                direction_to_player = (noheight - this.transform.position).normalized;
                transform.position = transform.position + direction_to_player;


            


            yield return new WaitForSecondsRealtime(.15f);
        }

    }


    private IEnumerator ShotDown()
    {
        finalWaitTime = waitingTime - ((actionSpeed - 1) * waitingTime);
        if (finalWaitTime < 0.4) finalWaitTime = 0.4f;
       // isRunning = true;

 
        while (true)
        {
            //shootPoint.transform.LookAt(shootPointdowndown);
            //var bullet = Instantiate(turretBullet, shootPoint.position, shootPoint.rotation, null);
            //var bulletScript = bullet.GetComponent<TurretBullet>();
            //bulletScript.speed = shotDownSpeed;

            yield return new WaitForSecondsRealtime(finalWaitTime);
        }
    }
}
