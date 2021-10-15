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
            Function();

            Destroy(collision.gameObject);

        }
    }

    public void Function()
    {
        craftingTable.UpdateCrafts();
        craftingTable.online.SetActive(false);
        craftingTable.offline.SetActive(true);
    }


}
