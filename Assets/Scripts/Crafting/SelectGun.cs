using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGun : MonoBehaviour
{
    public CraftingDevice craftingTable;
    public GunOfAType gun;
    public TMPro.TextMeshProUGUI gunTypeText;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {

            craftingTable.ReadWeapon(collision);

            Destroy(collision.gameObject);
        }
    }
}
