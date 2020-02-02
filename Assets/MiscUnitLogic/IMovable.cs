using UnityEngine;

public interface IMovable
{
    float Speed { get; }

    void MoveTo(Vector3 position);
    void Stop();
}
