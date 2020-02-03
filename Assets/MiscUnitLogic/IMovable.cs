using System;
using UnityEngine;

public interface IMovable
{
    Vector3 CurrentDestination { get; }
    event Action OnDestinationReached;
    float Speed { get; }


    void MoveTo(Vector3 position);
    void Stop();
    bool isMoving();
}
