using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveMod : MonoBehaviour
{
    public CraftingDevice craftingTable;
    [SerializeField] TMPro.TextMeshPro buttonText;

    //private string formatedString = "[Add new modifier ->] {value} Cost";

    void Start()
    {
        //buttonText.text = formatedString.Replace("{value}", "-");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            craftingTable.RemoveRandomMod();

            Destroy(collision.gameObject);
        }
    }
}
