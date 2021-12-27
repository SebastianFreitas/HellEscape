using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisassembleGun : MonoBehaviour
{
    public CraftingDevice craftingTable;
    [SerializeField] TMPro.TextMeshPro buttonText;


    public MeshRenderer[] meshes;



    private string labelText = "Deconstruct";

    private bool isCoroutine;

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
        var result = craftingTable.DisassembleGun();
        if (result == 0)
        {
            craftingTable.StartCoroutine(craftingTable.HighLight(meshes));
            buttonText.text = labelText;
            AudioSource.PlayClipAtPoint(click, transform.position, .1f);
        }
        else if (result == 1)
        {
            craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
            StartCoroutine(ExceptionMessage("Full Capacity"));
            AudioSource.PlayClipAtPoint(wrong, transform.position, 1f);
        }
        else if (result == 2)
        {
            craftingTable.StartCoroutine(craftingTable.HighLightNot(meshes));
            StartCoroutine(ExceptionMessage("Cannot do"));
            AudioSource.PlayClipAtPoint(wrong, transform.position, 1f);
        }
    }

    private IEnumerator ExceptionMessage(string message)
    {
        buttonText.text = message;
        yield return new WaitForSeconds(.3f);
        buttonText.text = labelText;
    }

}
