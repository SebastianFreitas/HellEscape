using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveMod : MonoBehaviour
{
    public CraftingDevice craftingTable;
    [SerializeField] TMPro.TextMeshPro buttonText;

    public MeshRenderer[] meshes;

    //private string formatedString = "[Add new modifier ->] {value} Cost";

    void Start()
    {
        //buttonText.text = formatedString.Replace("{value}", "-");
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
