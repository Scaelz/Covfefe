
using System.Collections.Generic;
using UnityEngine;

public interface ICustomer
{
    Queue<Vector3> ShoppingRoute { get; }
    bool isInLine { get; }

    void Leave();
    void GoShoping();
    void GetInLine();
    void Idle();
    void MoveInLine();
}
