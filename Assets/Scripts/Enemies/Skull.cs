using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : Monster
{

    [SerializeField] float maxAltitudeJumpDistance;
    [SerializeField] float minAltitudeJumpDistance;
    [SerializeField] float waitingTime;
    [SerializeField] float maxJumpForce;
    [SerializeField] float minJumpForce;

    public ParticleSystem attack;


    bool isLeft;
    Vector3 randomHeight;

    private float finalWaitTime;
    [SerializeField]public bool isHive =false;
    public Skull[] otherSkulls;

    new void Start()
    {

        
        var x = Random.Range(-2,3);
        rigidBody.angularVelocity = new Vector3(x,x,x);
        base.Start();
        


    }


    private void OnEnable()
    {
        
        StartCoroutine(waiterStart());
    }

    
    IEnumerator randomJump()
    {

        Vector3 playerPos = new Vector3(base.player.transform.position.x, base.player.transform.position.y, base.player.transform.position.z);

        Vector3 direction_to_player = (playerPos - this.transform.position).normalized;

        var x = false;
        if (Random.Range(1, 100) > 95) x = true;
        if (x) randomHeight = new Vector3(0, Random.Range(minAltitudeJumpDistance, maxAltitudeJumpDistance), 0);
        else randomHeight = new Vector3(0,0,0);

        base.rigidBody.AddForce((randomHeight + direction_to_player) * Random.Range(minJumpForce, maxJumpForce));


        yield return new WaitForSeconds(finalWaitTime);
        if (isHive) base.rigidBody.AddForce(((otherSkulls[Random.Range(0, otherSkulls.Length)].transform.position - this.transform.position).normalized) * 150 * 3);

        StartCoroutine(randomJump());


    }

    private IEnumerator Attack(Vector3 direction_to_player, Vector3 playerPos)
    {
        var x = Random.Range(-10, 10);
        rigidBody.angularVelocity = new Vector3(x, x, x);

        rigidBody.velocity = Vector3.zero;
        var rep = player.transform;
        rep.LookAt(transform.position);


        audioSource.PlayOneShot(hurts[Random.Range(0, hurts.Length)], volume/4);
        yield return new WaitForSeconds(1f);

        Vector3 playerPoss = new Vector3(base.player.transform.position.x, base.player.transform.position.y, base.player.transform.position.z);

        Vector3 direction_to_players = (playerPoss - this.transform.position).normalized; 
        
        base.rigidBody.AddForce((direction_to_players) * maxJumpForce * 3);
         
    }


    IEnumerator waiterStart()
    {
        yield return new WaitForSeconds(Random.Range(1, 3));
        finalWaitTime = waitingTime - ((actionSpeed - 1) * waitingTime);
        if (finalWaitTime < 0.4) finalWaitTime = 0.4f;
        if (isHive) otherSkulls = transform.parent.GetComponentsInChildren<Skull>();
        StartCoroutine(randomJump());
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