using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    public CraftingDevice craftingTable;

    [SerializeField] TMPro.TextMeshPro labelText;

    private string label = "Generate";

    public MeshRenderer[] meshes;




    void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        if (collision.gameObject.CompareTag("Bullet"))
        {

            if (craftingTable.GenerateGun())
            {

                craftingTable.StartCoroutine(craftingTable.HighLight(meshes));
                craftingTable.UpdateCrafts();
            }
            else
            {
                craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
                StartCoroutine(ExceptionMessage("No Space"));
            }



            Destroy(collision.gameObject);
        }
    }

    private IEnumerator ExceptionMessage(string message)
    {
        labelText.text = message;
        yield return new WaitForSeconds(.3f);
        labelText.text = label;
    }
}
