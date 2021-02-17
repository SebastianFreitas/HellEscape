using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBehaviour : MonoBehaviour
{
    public Camera fpsCam;
    private Vector3 targetPoint;
    private RaycastHit hit;


    private void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Ray ray = fpsCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out hit, 10f) && hit.transform.tag == "Button")
            {
                //hit.collider.transform.GetComponent<ButtonDoor>().UseDoor();
                transform.parent.GetComponent<GameMan>().currentRoom.GetComponent<Room>().UseDoor();
            }
        }
    } 

        

    
}
