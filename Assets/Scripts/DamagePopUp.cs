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
        GetComponent<Rigidbody>().AddForce(Vector3.up * 30);
    }

}
