using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBoss : Monster
{
    [SerializeField] GameObject ball;
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


    bool IsFight = false;
    private ShootForwardMonster[] shooters;
    private void Start()
    {
        ball.gameObject.SetActive(false);
        shooters = GetComponentsInChildren<ShootForwardMonster>();
        ActivateShooters(false);
        //if (PlayerPrefs.HasKey("attemptsTower")) attempts = PlayerPrefs.GetInt("attemptsTower");

        text.Add("It's not the first time one of you has made it here.");

        //Speak(text[attempts]); //this crashes stuff

        //StartCoroutine("Stop");


        attempts++;
        timer = phaseOneWaitTime;
        //PlayerPrefs.SetInt("attemptsTower", attempts);
        StartCoroutine(waiterStart());
    }

    private void ActivateShooters(bool isActive)
    {
        foreach (var item in shooters)
        {
            item.gameObject.SetActive(isActive);
        }
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

        if (IsFight) StartCoroutine("Stop");
        else StartCoroutine("FollowPlayer");

        isBusy = false;

    }
    IEnumerator waiterStart()
    {
        yield return new WaitForSeconds(.5f);

        finalWaitTime = waitingTime - ((actionSpeed - 1) * waitingTime);
        if (finalWaitTime < 0.4) finalWaitTime = 0.4f;

        //StartCoroutine("FollowPlayer");
    }

    [SerializeField] float phaseOneWaitTime;
    [SerializeField] float delayTime;
    private float timer;
    internal IEnumerator Stop()
    {
        IsFight = true;
        StopCoroutine("FollowPlayer");
        StartCoroutine("WaitAndMove");


        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
        }
       

        IsFight = false;
        StartCoroutine("FollowPlayer");
        StopCoroutine("WaitAndMove");

        timer = phaseOneWaitTime;
    }

    bool isBusy = false;
    internal Transform[] currentMovementList;

    private void FixedUpdate()
    {
        if (!IsFight)
        {
            if (transform.position.y < player.transform.position.y + 5)
            {
                transform.position += Vector3.up * 5;

            }
            else if (transform.position.y > player.transform.position.y + 10 )
            {
                transform.position -= Vector3.up * 5;

            }
        } 
        else if (!isBusy)
        {
            isBusy = true;
            var dis = Vector3.Distance(player.transform.position, transform.position);

            if (dis < 15)
            {
                StartCoroutine("Explode");
            }
            else
            {
                StartCoroutine("RangedAttack");
            }
        }


    }
    private IEnumerator Explode()
    {
        var explo = Instantiate(ball, transform);
        explo.gameObject.SetActive(true);
        explo.GetComponentInChildren<TowerBossExplosion>().StartUp();
        yield return new WaitForSecondsRealtime(2f);
        

        isBusy = false;

    }
    private IEnumerator RangedAttack()
    {
        ActivateShooters(true);
        yield return new WaitForSeconds(3f);

        ActivateShooters(false);
        isBusy = false;
    }

    [SerializeField] float delayFollowPlayer;
    IEnumerator FollowPlayer()
    {
        while (true)
        {

            Vector3 direction_to_player;

            var noheight = player.transform.position;
            noheight.y = transform.position.y;

            if(Vector3.Distance(noheight, transform.position) > 1)
            {
                direction_to_player = (noheight - this.transform.position).normalized;
                transform.position = transform.position + direction_to_player;
            }



            yield return new WaitForSecondsRealtime(delayFollowPlayer);
        }

    }

    IEnumerator WaitAndMove()
    {
        var posA = transform.position;
        var posB = currentMovementList[Random.Range(0,currentMovementList.Length)].position;

        yield return new WaitForSeconds(delayTime); // start at time X
        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        while (Time.time - startTime <= 2)
        { // until one second passed
            transform.position = Vector3.Lerp(posA, posB, Time.time - startTime); // lerp from A to B in one second
            yield return 1; // wait for next frame
        }

        StartCoroutine("WaitAndMove");
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
