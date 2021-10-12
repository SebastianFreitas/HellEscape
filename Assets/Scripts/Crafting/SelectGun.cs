using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGun : MonoBehaviour
{
    public CraftingDevice craftingTable;
    public GunOfAType gun;
    public TMPro.TextMeshPro gunTypeText;
    public GeneratedGuns generatedGuns;
    public MeshRenderer[] meshes;

    public int position;

    public bool isSelected;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Select();

            Destroy(collision.gameObject);
        }
    }

    public void Select()
    {
        if (gun != null)
        {
            craftingTable.ReadWeapon(null, gun);
            //craftingTable.EquipGun(gun, position);
            //generatedGuns.SelectSlot(position);
        }
    }
}
