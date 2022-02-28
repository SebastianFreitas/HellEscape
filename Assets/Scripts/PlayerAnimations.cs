using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public Animation gunAnimation;
    public Animator gunAnimator;
    public PlayerBasicMovement movement;
    public Gun gun;
    internal bool justShot;

    void Start()
    {
        gunAnimation["Idle"].speed = .2f;
        gunAnimation["Running"].speed = .1f;
        justShot = false;
    }
    void OnEnable()
    {
        justShot = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (movement.moveRaw == Vector3.zero && !gunAnimation.IsPlaying("Idle"))
        {
            gunAnimation.Stop();
            gunAnimation.Play("Idle");
        }
        else if (!gunAnimation.IsPlaying("Running"))
        {
            gunAnimation.Stop();
            gunAnimation.PlayQueued("Running");
        }
        if (justShot)
        {
            justShot = false;
            gunAnimation.Stop();
            gunAnimation.Play("Shoot");
        }
    }
}
