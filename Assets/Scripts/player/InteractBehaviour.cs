using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBehaviour : MonoBehaviour
{
    public Camera fpsCam;
    private RaycastHit hit;

    public AudioClip teleport;


    private void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Ray ray = fpsCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out hit, 10f) )
            {
                if (hit.transform.CompareTag("Item")) hit.collider.transform.GetComponent<Item>().CollectItem();
                else if (hit.transform.CompareTag("Button"))
                {
                    hit.collider.transform.GetComponent<SelectGun>().Select();
                }
                else if (hit.transform.CompareTag("AddMod"))
                {
                    hit.collider.transform.GetComponent<AddModUI>().Function();
                }
                else if (hit.transform.CompareTag("RemoveMod"))
                {
                    hit.collider.transform.GetComponent<RemoveMod>().Function();
                }
                else if (hit.transform.CompareTag("DestroyGun"))
                {
                    hit.collider.transform.GetComponent<Destroy>().Function();
                }
                else if (hit.transform.CompareTag("Deconstruct"))
                {
                    hit.collider.transform.GetComponent<DisassembleGun>().Function();
                }
                else if (hit.transform.CompareTag("Generate"))
                {
                    hit.collider.transform.GetComponent<Generate>().Function();
                }
                else if (hit.transform.CompareTag("Exit"))
                {
                    hit.collider.transform.GetComponent<ExitCrafting>().Function();
                }
                else if (hit.transform.CompareTag("CraftingDevice"))
                {
                    hit.collider.transform.parent.GetComponentInParent<CraftingDevice>().TurnOn();
                }
            }
            
        }
    }




}
