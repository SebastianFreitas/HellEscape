using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToInventory : MonoBehaviour
{
    public CraftingDevice craftingTable;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {

            transform.parent.parent.parent.GetComponentInParent<CraftingDevice>().GoToInventory();

            Destroy(collision.gameObject);
        }
    }


}
