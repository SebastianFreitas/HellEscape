using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
  public Doorway[] doorways;
  public MeshCollider MeshCollider;
  public Transform playerStart;
    public int roomType;
    private bool startedNext = false;
     
    //acess to the bounds of mechcollider
    public Bounds RoomBounds{
    get {return MeshCollider.bounds;}
  }

    void OnTriggerStay(Collider collider)
    {

        if (collider.CompareTag("Player") && Input.GetKeyDown("e") && !startedNext ) {

            startedNext = true;
            transform.parent.GetComponent<GameMan>().Next(roomType);
               
        }

    }

}
