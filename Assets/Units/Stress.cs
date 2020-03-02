using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stress : MonoBehaviour, IStressable, IClickable
{
    float defaultMultiplier;
    [SerializeField]
    float multiplier;
    public float Multiplier { get => multiplier; }

    [SerializeField]
    float maxStress = 100f;
    public float MaxStress { get => maxStress; }

    public float CurrentStress { get; private set; }

    [SerializeField]
    float loyalty;
    public float Loyalty { get => Mathf.Clamp(loyalty, 0.1f, 100); }

    [SerializeField]
    float stressPerTick;

    bool canIncrease = true;
    bool canDecrease = true;

    public event EventHandler OnStressOut;
    public event EventHandler OnChilled;
    public event Action<float> OnStressChanged;


    private void Start()
    {
        defaultMultiplier = multiplier;
        OnStressOut += StressOutHandler;
        OnChilled += OnChilledHandler;
        OnStressChanged += BoostMultiplier;
    }

    private void Update()
    {
        if (canDecrease)
        {
            DecreaseStress();
        }
    }

    void ReduceMultiplier()
    {
        multiplier = defaultMultiplier / 4;
    }

    void BoostMultiplier(float stressValue)
    {
        if (canIncrease)
        {
            multiplier = defaultMultiplier + 3 * stressValue / 100 ;
        }
    }

    void RestoreMultiplier()
    {
        multiplier = defaultMultiplier;
    }

    public void DecreaseStress()
    {
        if (CurrentStress > 0)
        {
            float chillingCorrector = !canIncrease ? 3 : 1;
            CurrentStress -= stressPerTick / loyalty * Time.deltaTime * 100 / chillingCorrector;
            OnStressChanged?.Invoke(CurrentStress);
        }
        if (CurrentStress < 0)
        {
            CurrentStress = 0;
            OnStressChanged?.Invoke(CurrentStress);
            //OnStressOut?.Invoke(this, EventArgs.Empty);
        }
    }

    public void IncreaseStress()
    {
        if (canIncrease)
        {
            if (CurrentStress < MaxStress)
            {
                CurrentStress += stressPerTick / loyalty + Time.deltaTime * CurrentStress * 2.75f;
                OnStressChanged?.Invoke(CurrentStress);
            }

            if (CurrentStress >= MaxStress)
            {
                OnStressOut?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    void StressOutHandler(object e, EventArgs args)
    {
        canIncrease = false;
        ReduceMultiplier();
        StartCoroutine(Chill());
    }

    IEnumerator Chill()
    {
        while (CurrentStress != 0)
        {
            yield return null;
        }
        OnChilled?.Invoke(this, EventArgs.Empty);
        OnStressChanged?.Invoke(CurrentStress);
    }

    void OnChilledHandler(object e, EventArgs args)
    {
        canIncrease = true;
        RestoreMultiplier();
    }

    public void OnDown()
    {
        throw new NotImplementedException();
    }

    public void OnHold()
    {
        if (canIncrease)
        {
            canDecrease = false; 
            IncreaseStress();
        }
    }

    public void OnUp()
    {
        canDecrease = true;
    }
}
