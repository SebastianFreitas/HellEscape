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

    internal string formatedString = "{value} parts";
    public int timesUsed = 1;


    void Start()
    {
        UpdatePriceText();
    }

    public void UpdatePriceText()
    {
        if (craftingTable.isHub) timesUsed = 2;
            price.text = formatedString.Replace("{value}", craftingTable.GetAddModPrice() * timesUsed + "");
    }

    private void OnEnable()
    {
        UpdatePriceText();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") )
        {
            Function();
            Destroy(collision.gameObject);
        }
    }
    public AudioClip wrong;
    public AudioClip click;
    public void Function()
    {
        var x = craftingTable.AddNewMod(timesUsed);
        if (x == 1)
        {
            craftingTable.StartCoroutine(craftingTable.HighLight(meshes));
            timesUsed++;
            craftingTable.UpdateCrafts();
            AudioSource.PlayClipAtPoint(click, transform.position, .1f);
        }
        else if (x == -1)
        {
            craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
            StartCoroutine(ExceptionMessage("Not ENough parts"));
            AudioSource.PlayClipAtPoint(wrong, transform.position, 1f);
        }
        else
        {
            craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
            StartCoroutine(ExceptionMessage("full capacity"));
            AudioSource.PlayClipAtPoint(wrong, transform.position, 1f);
        }
    }

    private IEnumerator ExceptionMessage(string message)
    {
        labelText.text = message;
        yield return new WaitForSeconds(.3f);
        labelText.text = addModLabel;
    }




}
