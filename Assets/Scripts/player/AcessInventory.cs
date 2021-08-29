using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcessInventory : MonoBehaviour
{
    private GameObject inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectsWithTag("Inventory")[0];
        inventory.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("i")) inventory.SetActive(true);
        else if (Input.GetButton("Fire1")) inventory.SetActive(false);
    }

 
}
