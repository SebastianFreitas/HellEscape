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
            Function();

            Destroy(collision.gameObject);
        }
    }
    public AudioClip wrong;
    public AudioClip click;
    public void Function()
    {
        if (craftingTable.GenerateGun())
        {

            craftingTable.StartCoroutine(craftingTable.HighLight(meshes));
            craftingTable.UpdateCrafts();
            AudioSource.PlayClipAtPoint(click, transform.position, .1f);
        }
        else
        {
            craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
            StartCoroutine(ExceptionMessage("No Space"));
            AudioSource.PlayClipAtPoint(wrong, transform.position, 1f);
        }
    }

    private IEnumerator ExceptionMessage(string message)
    {
        labelText.text = message;
        yield return new WaitForSeconds(.3f);
        labelText.text = label;
    }
}
