using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToCraft : MonoBehaviour
{
    public CraftingDevice craftingTable;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {

            craftingTable.GoToCrafting();

            Destroy(collision.gameObject);
        }
    }
}
