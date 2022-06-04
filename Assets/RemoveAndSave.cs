using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAndSave : MonoBehaviour
{
    public CraftingDevice craftingTable;
    [SerializeField] TMPro.TextMeshPro price;

    [SerializeField] TMPro.TextMeshPro labelText;

    private string removeAndSave = "Remove and save modifier";
    private string addMod = "Add saved mod";

    public MeshRenderer[] meshes;

    internal string formatedString = "{value} parts";


    void Start()
    {
        if (craftingTable.savedMod != null)
        {
            craftingTable.removedModGameObject.SetActive(true);
            labelText.text = addMod;
        }
        else
        {
            craftingTable.removedModGameObject.SetActive(false);
            labelText.text = removeAndSave;
        }
        UpdatePriceText();
    }

    public void UpdatePriceText()
    {
        price.text = formatedString.Replace("{value}", craftingTable.GetRemoveAndSaveModPrice() + "");
    }

    private void OnEnable()
    {
        UpdatePriceText();
    }

    public AudioClip wrong;
    public AudioClip click;
    public void Function()
    {
        int x = 0;

        if (craftingTable.savedMod != null) x = craftingTable.AddSavedMod();
        else x = craftingTable.RemoveAndSaveMod();

        if (x == 0)
        {
            craftingTable.StartCoroutine(craftingTable.HighLight(meshes));
            craftingTable.UpdateCrafts();
            if (craftingTable.savedMod != null)
            {
                craftingTable.removedModGameObject.SetActive(true);
                labelText.text = addMod;
            }
            else
            {
                craftingTable.removedModGameObject.SetActive(false);
                labelText.text = removeAndSave;
            }
            AudioSource.PlayClipAtPoint(click, transform.position, .1f);

        }
        else if (x == 2)
        {
            craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
            StartCoroutine(ExceptionMessage("Not enough parts"));
            AudioSource.PlayClipAtPoint(wrong, transform.position, 1f);
        }
        else if (x == 1)
        {
            craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
            StartCoroutine(ExceptionMessage("No Mods"));
            AudioSource.PlayClipAtPoint(wrong, transform.position, 1f);
        }
        else if (x == 3)
        {
            craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
            StartCoroutine(ExceptionMessage("Full"));
            AudioSource.PlayClipAtPoint(wrong, transform.position, 1f);
        }
        else if (x == 4)
        {
            craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
            StartCoroutine(ExceptionMessage("Not Compatible"));
            AudioSource.PlayClipAtPoint(wrong, transform.position, 1f);
        }
    }

    private IEnumerator ExceptionMessage(string message)
    {
        labelText.text = message;
        yield return new WaitForSeconds(.3f);
        if (craftingTable.savedMod != null) labelText.text = addMod;
        else labelText.text = removeAndSave;
    }

}
