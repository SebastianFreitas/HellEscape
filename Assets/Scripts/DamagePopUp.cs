using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    public TMPro.TextMeshPro damageLabel;
    public int damage;
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * 50);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
