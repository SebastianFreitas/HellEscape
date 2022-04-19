using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObject : MonoBehaviour
{
    public float timer;
    public bool dieOnStart = true;

    public IEnumerator WaitDie(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        if (dieOnStart) StartCoroutine(WaitDie(timer));
    }
}
