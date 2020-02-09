
using System;

public interface IStressable
{
    float MaxStress { get; }
    float CurrentStress { get; }
    float Loyalty { get; }
    event EventHandler OnStressOut;

    void IncreaseStress();
    void DecreaseStress();
}
