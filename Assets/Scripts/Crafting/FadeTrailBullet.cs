using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTrailBullet : MonoBehaviour
{
    public IEnumerator KillTrail()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
