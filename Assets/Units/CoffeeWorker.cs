using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeWorker : MonoBehaviour, IWorker
{
    public Transform CurrentTransform { get => transform; }

    public float WorkSpeed { get => 2; }

    public event Action OnWorkDone;

    public void GetToWork()
    {
        throw new NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
