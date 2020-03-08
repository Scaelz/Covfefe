using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeWorker : MonoBehaviour, IWorker, IUpgradeable
{
    [SerializeField] int index;
    public Transform CurrentTransform { get => transform; }
    [SerializeField]
    float workSpeed;
    [SerializeField]
    float timeToEndWork;
    float workTimer = 0;
    public float TimeToEndWork { get => timeToEndWork; }
    IdleConfig idleConfig;
    public bool isWorking { get; private set; } = false;
    public float WorkSpeed { get => workSpeed; }
    [SerializeField]
    Line currentLine;
    public Line CurrentLine { get; private set; }
    IStressable stressScript;
    [SerializeField] int upgradeIndex;
    public int UpgradeIndex { get; private set; }

    public event Action OnWorkDone;
    public event Action OnWorkStarted;
    public event Action OnGreetCustomer;
    public event Action OnFreeLine;
    public event Action OnPassedCofee;
    public event Action<float> OnSpeedMultiplierChanged;

    // Start is called before the first frame update
    void Start()
    {
        UpgradeIndex = upgradeIndex;
        idleConfig = FindObjectOfType<IdleConfig>();
        CurrentLine = currentLine;
        stressScript = GetComponent<IStressable>();
        OnPassedCofee += WorkStateChangedHandler;
        OnWorkStarted += WorkStateChangedHandler;
        OnPassedCofee += CurrentLine.CustomerServicedHandler;
        CurrentLine.OnFirstInLineChanged += GreetCustomer;
        stressScript.OnStressChanged += StressChangedHandler;
    }

    public float GetWorkSpeed()
    {
        return timeToEndWork;
    }

    void StressChangedHandler(float value)
    {
        OnSpeedMultiplierChanged?.Invoke(stressScript.Multiplier);
    }

    void GreetCustomer()
    {
        OnGreetCustomer?.Invoke();
    }

    void CheckForCustomers()
    {
        if (CurrentLine.CurrentCustomer != null)
        {
            OnGreetCustomer?.Invoke();
        }
        else
        {
            OnFreeLine?.Invoke();
        }
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

    //float StressedWorkSpeed()
    //{
    //    return WorkSpeed * stressScript.Multiplier;
    //}

    IEnumerator ProgressWork()
    {
        while (workTimer < TimeToEndWork)
        {
            //float currentSpeed = StressedWorkSpeed();
            yield return null;
            workTimer += Time.deltaTime * stressScript.Multiplier;
        }
        idleConfig.GetComponent<IdleConfig>().Click();
        OnWorkDone?.Invoke();
        workTimer = 0;
    }

    //IEnumerator ProgressWork()
    //{
    //    while (workTimer < TimeToEndWork)
    //    {
    //        float currentSpeed = StressedWorkSpeed();
    //        yield return new WaitForSeconds(WorkSpeed);
    //        workTimer += currentSpeed;
    //    }
    //    idleConfig.GetComponent<IdleConfig>().Click();
    //    OnWorkDone?.Invoke();
    //    workTimer = 0;
    //}

    public void OnCoffeePassedHandler()
    {
        OnPassedCofee?.Invoke();
    }

    public void Upgrade(CustomUpgrade upgrade, int lvl, int maxLvl)
    {
        throw new NotImplementedException();
    }
}
