using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    public TMPro.TextMeshPro damageLabel;
    public int damage;

    public Transform player;

    Vector3 targetPoint;
    void Start()
    {
        var x = new Vector3(Random.Range(-.75f,.75f),1,0);
        GetComponent<Rigidbody>().AddForce(x * 30);
    }

}
