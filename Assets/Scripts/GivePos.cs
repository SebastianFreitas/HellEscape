using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInParent<Hub>().statspos = transform;
    }


}
