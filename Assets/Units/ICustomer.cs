
using System.Collections.Generic;
using UnityEngine;

public interface ICustomer
{
    Transform CurrentTransform { get; }
    Queue<Line> ShoppingRoute { get; }

    void Leave(Vector3 exitPoint);
}
