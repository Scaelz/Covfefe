using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stress : MonoBehaviour, IStressable
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

    public event EventHandler OnStressOut;

    public void DecreaseStress()
    {
        CurrentStress -= stressPerTick / 2 / loyalty;
        if (CurrentStress < 0)
        {
            OnStressOut?.Invoke(this, EventArgs.Empty);
        }
    }

    public void IncreaseStress()
    {
        CurrentStress += stressPerTick / loyalty;
        if (CurrentStress >= MaxStress)
        {
            OnStressOut?.Invoke(this, EventArgs.Empty);
        }
    }
}
