using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public CraftingDevice craftingTable;
    [SerializeField] TMPro.TextMeshPro buttonText;
    [SerializeField] TMPro.TextMeshPro parts;

    public MeshRenderer[] meshes;
    private bool areYouSure = false;

    private string labelText = "destroy";

    private string formatedString = "{value} parts";

    private bool isCoroutine;

    void Start()
    {
        UpdatePriceText();
    }

    private void OnEnable()
    {
        UpdatePriceText();
        isCoroutine = false;
        areYouSure = false;
    }



    public void UpdatePriceText()
    {

            parts.text = formatedString.Replace("{value}", craftingTable.GetDestroyReward() + "");


    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Function();

            Destroy(collision.gameObject);
        }
    }

    public void Function()
    {
        if (areYouSure)
        {
            craftingTable.DestroyGun();
            craftingTable.StartCoroutine(craftingTable.HighLight(meshes));
            buttonText.text = labelText;
            areYouSure = false;

        }
        else if (!isCoroutine) StartCoroutine(ChangeText("Are you sure?"));
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
