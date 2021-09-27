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

    private bool isCoroutine;

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
        if (craftingTable.gun.isBase) formatedString.Replace("{value}", 0+ "");
        else
        {
            var destroy = 1;
            if (isDestroy) destroy = 2; 
            parts.text = formatedString.Replace("{value}", (destroy * craftingTable.gun.level * (craftingTable.gun.mods.Count + 1)) + "");
        }
       
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
                    buttonText.text = labelText;
                }
                else
                {
                    var result = craftingTable.DisassembleGun();
                    if (result == 0)
                    {
                        craftingTable.StartCoroutine(craftingTable.HighLight(meshes));
                        buttonText.text = labelText;
                    } 
                    else if (result == 1)
                    {
                        craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
                        StartCoroutine(ExceptionMessage("Full Capacity"));
                    }
                    else if (result == 2)
                    {
                        craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
                        StartCoroutine(ExceptionMessage("Cannot do"));
                    }
                }
                areYouSure = false;

            } else if (!isCoroutine) StartCoroutine(ChangeText("Are you sure?"));

            

            Destroy(collision.gameObject);
        }
    }
    private IEnumerator ExceptionMessage(string message)
    {
        buttonText.text = message;
        yield return new WaitForSeconds(.3f);
        buttonText.text = labelText;
    }

    private IEnumerator ChangeText(string x)
    {
        buttonText.text = x;
        areYouSure = true;
        isCoroutine = true;
        yield return new WaitForSecondsRealtime(3f);
        isCoroutine = false;
        areYouSure = false;
        buttonText.text = labelText;
    }


}
