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
    public AudioClip wrong;
    public AudioClip click;


    public void Select()
    {

        if (gun != null)
        {
            craftingTable.ReadWeapon(null, gun);
            AudioSource.PlayClipAtPoint(click, transform.position, .1f);
        }
        else AudioSource.PlayClipAtPoint(wrong, transform.position, 1f);
    }
}
