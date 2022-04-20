using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenTurretBullet : TurretBullet
{

    [SerializeField] GameObject trail;
    [SerializeField] GameObject bombs;
    private void OnDestroy()
    {
        //trail.transform.parent = null;
        //bombs.transform.parent = null;
    }
}
