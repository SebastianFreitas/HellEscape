using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisassembleGun : MonoBehaviour
{
    public CraftingDevice craftingTable;
    [SerializeField] TMPro.TextMeshPro buttonText;
    [SerializeField] TMPro.TextMeshPro parts;

    public MeshRenderer[] meshes;
    private bool areYouSure = false;


    public bool isDestroy;
    private string labelText;

    private string formatedString = "{value} parts";

    void Start()
    {
        UpdatePriceText();
        //buttonText.text = formatedString.Replace("{value}", "-");
    }

    private void OnEnable()
    {
        StartLabel();
        UpdatePriceText();
    }

    private void StartLabel()
    {
        if (isDestroy) labelText = "Destroy";
        else labelText = "Remove Layout";
        buttonText.text = labelText;
    }

    public void UpdatePriceText()
    {
        var destroy = 1;
        if (isDestroy) destroy = 2; 
        parts.text = formatedString.Replace("{value}", (destroy * craftingTable.gun.level * (craftingTable.gun.mods.Count + 1)) + "");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (areYouSure)
            {
                if (isDestroy)
                {
                    craftingTable.DestroyGun();
                    craftingTable.StartCoroutine(craftingTable.HighLight(meshes));
                    areYouSure = false;
                    buttonText.text = labelText;
                }
                else
                {
                    if (craftingTable.DisassembleGun())
                    {
                        craftingTable.StartCoroutine(craftingTable.HighLight(meshes));
                        areYouSure = false;
                        buttonText.text = labelText;
                    } 
                    else
                    {
                        craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
                        StartCoroutine(ChangeText("Full Capacity"));
                    }
                }


            } else StartCoroutine(ChangeText("Are you sure ?"));

            

            Destroy(collision.gameObject);
        }
    }
    private IEnumerator ChangeText(string x)
    {
        buttonText.text = x;
        areYouSure = true;
        yield return new WaitForSeconds(3f);
        areYouSure = false;
        buttonText.text = labelText;
    }


}
