using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsMovement : MonoBehaviour
{
    public GameObject player;

    public CraftingDevice craftdevice;
    // Start is called before the first frame update
    void Start()
    {
        player = craftdevice.player;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(player.transform);
    }
}
