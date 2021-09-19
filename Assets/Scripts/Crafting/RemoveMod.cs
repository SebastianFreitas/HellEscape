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
        price.text = formatedString.Replace("{value}", (craftingTable.gun.level * (1 + craftingTable.gun.mods.Count)) * 2 * timesUsed + "");
    }

    private void OnEnable()
    {
        UpdatePricetext();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {

            var x = craftingTable.RemoveRandomMod(timesUsed);
            if (x == 0)
            {
              
                craftingTable.StartCoroutine(craftingTable.HighLight(meshes));

                timesUsed++;
                craftingTable.UpdateCrafts();
            }
            else if (x == 2)
            {
                craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
                StartCoroutine(ExceptionMessage("Not ENough parts"));

            }
            else
            {
                craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
                StartCoroutine(ExceptionMessage("No modifiers"));
            }

          

            Destroy(collision.gameObject);
        }
    }

    private IEnumerator ExceptionMessage(string message)
    {
        labelText.text = message;
        yield return new WaitForSeconds(.3f);
        labelText.text = RemoveModLabel;
    }
}
