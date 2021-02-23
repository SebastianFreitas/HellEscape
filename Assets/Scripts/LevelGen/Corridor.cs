using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corridor : Room
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(Random.Range(0,45), 0, 0);
    }


}
