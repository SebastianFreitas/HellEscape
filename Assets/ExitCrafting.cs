using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCrafting : MonoBehaviour
{
    public CraftingDevice craftingTable;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            craftingTable.UpdateCrafts();
            craftingTable.offline.SetActive(true);
            craftingTable.online.SetActive(false);
            
            
        }
    }



}
