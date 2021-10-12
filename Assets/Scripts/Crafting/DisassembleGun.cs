using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisassembleGun : MonoBehaviour
{
    public CraftingDevice craftingTable;
    [SerializeField] TMPro.TextMeshPro buttonText;


    public MeshRenderer[] meshes;
    private bool areYouSure = false;



    private string labelText = "Deconstruct";

    private bool isCoroutine;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") )
        {
            if (areYouSure)
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
