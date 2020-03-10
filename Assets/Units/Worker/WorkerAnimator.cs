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

    [SerializeField]
    GameObject coffeecup;

    CoffeeWorker worker;

    private void Start()
    {
        worker = GetComponent<CoffeeWorker>();
        worker.OnWorkStarted += WorkStartedHandler;
        worker.OnWorkDone += WorkDoneHandler;
        worker.OnGreetCustomer += GreetHandle;
        worker.OnPassedCofee += GreetHandle;
        worker.OnSpeedMultiplierChanged += ChangeAnimatorSpeed;
        //worker.OnFreeLine += IdleHandler;
        //worker.OnGreetCustomer += GreetHandle;
    }

    void ChangeAnimatorSpeed(float value)
    {
        animator.speed = value;
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(done.name) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            worker.OnCoffeePassedHandler();
            //IdleHandler();
        }
    }

    void IdleHandler()
    {
        coffeecup.SetActive(false);
        Play(CoffeeWorkerAnimation.Idle);
    }

    void GreetHandle()
    {
        try
        {
            coffeecup.SetActive(false);
        }
        catch (System.Exception)
        {

            Debug.Log("Null");
        }
        
        Play(CoffeeWorkerAnimation.Greet);
    }

    void WorkDoneHandler()
    {
        coffeecup.SetActive(true);
        Play(CoffeeWorkerAnimation.Pass);
    }

    void WorkStartedHandler()
    {
        coffeecup.SetActive(false);
        animator.SetTrigger("goWork");
        //Play(CoffeeWorkerAnimation.Working);
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
