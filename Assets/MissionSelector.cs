using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSelector : ModDataRoom
{
    public PlayerInventory playerInventory;

    public Material green;
    public Material yellow;
    public Material red;
    public Material blue;

    public TMPro.TextMeshPro test;

    void Start()
    {
        test.text = CreateMission().text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
