using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisassembleGun : MonoBehaviour
{
    public CraftingDevice craftingTable;
    [SerializeField] TMPro.TextMeshPro buttonText;

    public MeshRenderer[] meshes;
    private bool areYouSure = false;

    //private string formatedString = "[Add new modifier ->] {value} Cost";

    void Start()
    {
        //buttonText.text = formatedString.Replace("{value}", "-");
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

    void OnEnable()
    {
        buttonText.text = "Desassemble gun";
    }
}
