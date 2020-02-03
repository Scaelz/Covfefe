using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorker
{
    Transform CurrentTransform { get; }
    float WorkSpeed { get; }
    event Action OnWorkDone;

    void GetToWork();
}
