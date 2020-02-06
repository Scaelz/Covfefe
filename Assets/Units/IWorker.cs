using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorker
{
    Transform CurrentTransform { get; }
    float WorkSpeed { get; }
    Line CurrentLine { get; }
    bool isWorking { get; }
    event Action OnWorkDone;

    void GetToWork();
}
