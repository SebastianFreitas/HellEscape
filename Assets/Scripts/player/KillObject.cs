using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObject : MonoBehaviour
{
    public float timer; 
    void Start()
    {
        StartCoroutine(WaitDie(timer));
    }

    IEnumerator WaitDie(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
