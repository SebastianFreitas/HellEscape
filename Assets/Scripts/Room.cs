using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
  public Doorway[] doorways;
  public MeshCollider MeshCollider;


  //acess to the bounds of mechcollider
  public Bounds RoomBounds{
    get {return MeshCollider.bounds;}
  }
}
