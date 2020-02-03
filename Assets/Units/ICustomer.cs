
using System.Collections.Generic;
using UnityEngine;

public interface ICustomer
{
    Transform CurrentTransform { get; }
    Queue<CashBox> ShoppingRoute { get; }
    bool isInLine { get; }

    void Leave(Vector3 exitPoint);
    void Idle();
    void MoveInLine(Vector3 newPosition);
}
