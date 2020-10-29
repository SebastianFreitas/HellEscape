using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://answers.unity.com/questions/242648/force-on-character-controller-knockback.html
public class ImpactReciever : MonoBehaviour
{
  public float mass = 3f; // defines the character mass
  Vector3 impact = Vector3.zero;
  private CharacterController controller;

  void Start(){
    controller = gameObject.GetComponent<CharacterController>();
  }

  // call this function to add an impact force:
  void AddImpact(Vector3 dir, float force){
    dir.Normalize();
    if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
    impact += dir.normalized * force / mass;
  }

  void Update(){
    // apply the impact force:
    if (impact.magnitude > 0.2) controller.Move(impact * Time.deltaTime);
    // consumes the impact energy each cycle:
    impact = Vector3.Lerp(impact, Vector3.zero, 5*Time.deltaTime);
  }
}
