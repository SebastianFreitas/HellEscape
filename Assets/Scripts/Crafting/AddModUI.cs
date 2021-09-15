using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddModUI : MonoBehaviour
{
    public CraftingDevice craftingTable;
    [SerializeField] TMPro.TextMeshPro price;

    [SerializeField] TMPro.TextMeshPro labelText;

    private string addModLabel = "Add new modifier";



    public MeshRenderer[] meshes;

    private string formatedString = "{value} parts";


    void Start()
    {
        price.text = formatedString.Replace("{value}", craftingTable.gun.level*(craftingTable.gun.mods.Count+1) + "");
    }

    private void OnEnable()
    {
        price.text = formatedString.Replace("{value}", craftingTable.gun.level * (craftingTable.gun.mods.Count + 1) + "");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            var x = craftingTable.AddNewMod();
            if (x == 1)
            {
                craftingTable.StartCoroutine(craftingTable.HighLight(meshes));
            }
            else if (x == -1)
            {
                craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
                StartCoroutine(ExceptionMessage("Not ENough parts"));

            } else
            {
                craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
                StartCoroutine(ExceptionMessage("full capacity"));
            }
                
            
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator ExceptionMessage(string message)
    {
        labelText.text = message;
        yield return new WaitForSeconds(.3f);
        labelText.text = addModLabel;
    }




}
