using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ClientAnims
{
    angry,
    take,
    walk,
    idle
}

[RequireComponent(typeof(ICustomer), typeof(Animator))]

public class CustomerAnimator : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    AnimationClip idle, anger, walk, take;

    public void Play(ClientAnims animType)
    {
        string clipName = "idle";

        switch (animType)
        {
            case ClientAnims.angry:
                clipName = anger.name;
                break;
            case ClientAnims.take:
                clipName = take.name;
                break;
            case ClientAnims.walk:
                clipName = walk.name;
                break;
            case ClientAnims.idle:
                clipName = idle.name;
                break;
            default:
                break;
        }
        animator.Play(clipName);
    }
}
