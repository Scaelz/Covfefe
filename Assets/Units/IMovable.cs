using System;
using UnityEngine;

public interface IMovable
{
    Vector3 CurrentDestination { get; }
    event Action OnDestinationReached;
    event Action OnStartMoving;
    float Speed { get; }

    float GetMaxSpeed();
    float GetDefaultSpeed();
    void MoveTo(Vector3 position);
    void Stop();
    bool isMoving();
    void RotateTowards(Vector3 target);
    void SetPriority(int priority);

    void SetQuality(bool state);

    void ChangeSpeed(float value);
}
