using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBehaviour : MonoBehaviour
{
    public Camera fpsCam;
    private Vector3 targetPoint;
    private RaycastHit hit;

    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Ray ray = fpsCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out hit, 3f, LayerMask.NameToLayer("Button")))
            {
                //hit.transform.gameObject.GetComponent<ButtonDoor>().UseButton();
                transform.parent.GetComponent<GameMan>().Next(2);
            }
        }

    }
}
