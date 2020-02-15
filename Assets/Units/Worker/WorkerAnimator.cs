using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CoffeeWorkerAnimation
{
    Idle,
    Working,
    Greet,
    Pass
}
[RequireComponent(typeof(CoffeeWorker))]
public class WorkerAnimator : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    AnimationClip idle, working, done, greet ;

    CoffeeWorker worker;

    private void Start()
    {
        worker = GetComponent<CoffeeWorker>();
        worker.OnWorkStarted += WorkStartedHandler;
        worker.OnWorkDone += WorkDoneHandler;
    }

    void WorkDoneHandler()
    {
        Play(CoffeeWorkerAnimation.Pass);
    }

    void WorkStartedHandler()
    {
        Play(CoffeeWorkerAnimation.Working);
    }

    public void ChangeSpeed(float value)
    {
        animator.speed = value;
    }

    public void SetDefaultSpeed()
    {
        animator.speed = 1;
    }

    public void Play(CoffeeWorkerAnimation animType)
    {
        string clipName = idle.name;
        switch (animType)
        {
            case CoffeeWorkerAnimation.Idle:
                clipName = idle.name;
                break;
            case CoffeeWorkerAnimation.Working:
                clipName = working.name;
                break;
            case CoffeeWorkerAnimation.Greet:
                clipName = greet.name;
                break;
            case CoffeeWorkerAnimation.Pass:
                clipName = done.name;
                break;
            default:
                break;
        }
        animator.Play(clipName);
    }
}
