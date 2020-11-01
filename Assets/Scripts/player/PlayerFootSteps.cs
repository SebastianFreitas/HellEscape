using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootSteps : MonoBehaviour
{

  AudioSource[] sources;

  public float footstepDelay = .5f;
  public PlayerMovement playerMov;

  public AudioSource doubleJump;
  public AudioSource jump;

  private float nextFootstep = 0;
  private bool asDoubleJumped = false;

  void Start()
  {
  sources = GetComponents<AudioSource>();
  }

  void Update () {
    if(playerMov.isGrounded){
      asDoubleJumped = false;
      if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S)
          || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W)) {
          nextFootstep -= Time.deltaTime;
          if (nextFootstep <= 0) {
              sources[Random.Range(0,5)].Play();
              nextFootstep += footstepDelay;
          }
      }
      if (Input.GetButtonDown("Jump")) jump.Play();
    }
    if(playerMov.isGrounded == false)
        {
        if (Input.GetButtonDown("Jump") && !asDoubleJumped) {
        doubleJump.Play();
        asDoubleJumped = true;
        }
    }
  }
}
