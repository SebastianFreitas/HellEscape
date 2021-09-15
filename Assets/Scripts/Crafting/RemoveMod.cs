using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveMod : MonoBehaviour
{
    public CraftingDevice craftingTable;
    [SerializeField] TMPro.TextMeshPro price;

    public MeshRenderer[] meshes;

    private string formatedString = "{value} parts";


    void Start()
    {
        //price = this.gameObject.GetComponentsInChildren<MeshRenderer>();
        price.text = formatedString.Replace("{value}", craftingTable.gun.GetPrice() * (craftingTable.gun.mods.Count + 1) + "");

    }

    private void OnEnable()
    {
        price.text = formatedString.Replace("{value}", craftingTable.gun.GetPrice().ToString());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (craftingTable.RemoveRandomMod()) craftingTable.StartCoroutine(craftingTable.HighLight(meshes));
            else craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));

            Destroy(collision.gameObject);
        }
    }
}
