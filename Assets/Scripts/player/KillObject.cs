using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObject : MonoBehaviour
{
    public float timer;
    public bool dieOnStart = true;
    void Start()
    {
        if (dieOnStart) StartCoroutine(WaitDie(timer));
    }

    public IEnumerator WaitDie(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
