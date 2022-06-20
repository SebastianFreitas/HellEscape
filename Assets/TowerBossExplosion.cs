using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBossExplosion : MonsterDamageRay
{

    private bool isExploding = false;
    internal void StartUp()
    {
        transform.parent.parent = null;
        StartCoroutine("Explode");
    }

    private IEnumerator Explode()
    {
        isExploding = true;
        for (int i = 0; i < 15; i++)
        {
           transform.parent.transform.localScale *= 1.2f;
            yield return new WaitForSecondsRealtime(.15f);
        }
        Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        if (isExploding)
        {
            Destroy(this.transform.parent.gameObject);
        }
    }
}
