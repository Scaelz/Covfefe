
using System;

public interface IStressable
{
    float MaxStress { get; }
    float CurrentStress { get; }
    float Loyalty { get; }
    event EventHandler OnStressOut;
    event EventHandler OnChilled;
    event Action<float> OnStressChanged;

    void IncreaseStress();
    void DecreaseStress();
}
