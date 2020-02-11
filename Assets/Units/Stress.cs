using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stress : MonoBehaviour, IStressable, IClickable
{
    [SerializeField]
    float maxStress;
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
        OnStressOut += StressOutHandler;
        OnChilled += OnChilledHandler;
    }

    private void Update()
    {
        Debug.Log($"Update: {CurrentStress} + {this.name}");
        if (canDecrease)
        {
            Debug.Log("tada");
            DecreaseStress();
        }
    }

    public void DecreaseStress()
    {
        Debug.Log($"DecreaseStress: {CurrentStress}");
        if (CurrentStress > 0)
        {
            Debug.Log(2);
            CurrentStress -= stressPerTick / 2 / loyalty;
            OnStressChanged?.Invoke(CurrentStress);
        }
        if (CurrentStress < 0)
        {
            CurrentStress = 0;
            //OnStressOut?.Invoke(this, EventArgs.Empty);
        }
    }

    public void IncreaseStress()
    {
        if (canIncrease)
        {
            if (CurrentStress < MaxStress)
            {
                CurrentStress += stressPerTick / loyalty;
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
        StartCoroutine(Chill());
    }

    IEnumerator Chill()
    {
        while (CurrentStress != 0)
        {
            yield return null;
            DecreaseStress();
        }
        OnChilled?.Invoke(this, EventArgs.Empty);
    }

    void OnChilledHandler(object e, EventArgs args)
    {
        canIncrease = true;
    }

    public void OnDown()
    {
        throw new NotImplementedException();
    }

    public void OnHold()
    {
        canDecrease = false; 
        IncreaseStress();
    }

    public void OnUp()
    {
        canDecrease = true;
    }
}
