using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeWorker : MonoBehaviour, IWorker
{
    public Transform CurrentTransform { get => transform; }
    [SerializeField]
    float workSpeed;
    public GameObject idleConfig;
    public bool isWorking { get; private set; } = false;
    public float WorkSpeed { get => workSpeed; }
    [SerializeField]
    Line currentLine;
    public Line CurrentLine { get; private set; }

    public event Action OnWorkDone;
    public event Action OnWorkStarted;


    // Start is called before the first frame update
    void Start()
    {
        CurrentLine = currentLine;
        OnWorkDone += WorkStateChangedHandler;
        OnWorkStarted += WorkStateChangedHandler;
        OnWorkDone += CurrentLine.CustomerServicedHandler;
    }

    void WorkStateChangedHandler()
    {
        isWorking = !isWorking;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWorking)
        {
            if (CurrentLine.isCustomerReady())
            {
                GetToWork();
            }
        }
    }

    public void GetToWork()
    {
        OnWorkStarted?.Invoke();
        StartCoroutine(ProgressWork());
    }

    IEnumerator ProgressWork()
    {
        yield return new WaitForSeconds(WorkSpeed);
        idleConfig.GetComponent<IdleConfig>().Click();
        OnWorkDone?.Invoke();
    }
}
