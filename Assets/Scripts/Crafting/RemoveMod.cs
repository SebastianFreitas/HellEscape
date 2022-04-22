using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveMod : MonoBehaviour
{
    public CraftingDevice craftingTable;
    [SerializeField] TMPro.TextMeshPro price;

    [SerializeField] TMPro.TextMeshPro labelText;

    private string RemoveModLabel = "Remove Modifier";

    public MeshRenderer[] meshes;

    private string formatedString = "{value} parts";
    public int timesUsed = 1;

    void Start()
    {
        //price = this.gameObject.GetComponentsInChildren<MeshRenderer>();
        UpdatePricetext();

    }

    public void UpdatePricetext()
    {
        if (craftingTable.isHub) timesUsed = 2;
        price.text = formatedString.Replace("{value}", craftingTable.GetRemoveModPrice() * timesUsed + "");
    }

    private void OnEnable()
    {
        UpdatePricetext();
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
        var x = craftingTable.RemoveRandomMod(timesUsed);
        if (x == 0)
        {

            craftingTable.StartCoroutine(craftingTable.HighLight(meshes));

            timesUsed++;
            craftingTable.UpdateCrafts();
            AudioSource.PlayClipAtPoint(click, transform.position, .1f);

        }
        else if (x == 2)
        {
            craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
            StartCoroutine(ExceptionMessage("Not ENough parts"));
            AudioSource.PlayClipAtPoint(wrong, transform.position, 1f);
        }
        else
        {
            craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
            StartCoroutine(ExceptionMessage("No modifiers"));
            AudioSource.PlayClipAtPoint(wrong, transform.position, 1f);
        }
    }

    private IEnumerator ExceptionMessage(string message)
    {
        labelText.text = message;
        yield return new WaitForSeconds(.3f);
        labelText.text = RemoveModLabel;
    }
}
