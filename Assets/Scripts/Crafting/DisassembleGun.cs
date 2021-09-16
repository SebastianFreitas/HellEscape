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

    private string formatedString = "{value} parts";

    void Start()
    {
        UpdatePriceText();
        //buttonText.text = formatedString.Replace("{value}", "-");
    }

    private void OnEnable()
    {
        buttonText.text = "Desassemble gun";
        UpdatePriceText();
    }
    public void UpdatePriceText()
    {
        parts.text = formatedString.Replace("{value}", (craftingTable.gun.level * (craftingTable.gun.mods.Count + 1)) + "");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (areYouSure)
            {
                craftingTable.DisassembleGun();
                craftingTable.StartCoroutine(craftingTable.HighLight(meshes));
                areYouSure = false;
                buttonText.text = "Desassemble gun";
            } else StartCoroutine(AreYouSure());

            

            Destroy(collision.gameObject);
        }
    }
    private IEnumerator AreYouSure()
    {
        buttonText.text = "Are you sure?";
        areYouSure = true;
        yield return new WaitForSeconds(3f);
        areYouSure = false;
        buttonText.text = "Desassemble gun";
    }


}
