using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGun : MonoBehaviour
{
    public CraftingDevice craftingTable;
    public GunOfAType gun;
    public TMPro.TextMeshPro gunTypeText;

    public int position;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {

            craftingTable.ReadWeapon(collision, gun);
            craftingTable.EquipGun(gun, position);

            Destroy(collision.gameObject);
        }
    }
}
