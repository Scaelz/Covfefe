using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : Unit, ICustomer
{
    private void Start()
    {
        Initialize();
        stressScript.OnStressOut += StressOutHandler;
    }

    void StressOutHandler(object e, EventArgs args)
    {
        Leave();
    }

    public void GetInLine()
    {
        throw new System.NotImplementedException();
    }

    public void GoShoping()
    {
        Debug.Log("Sta");

    }

    public void Idle()
    {
        throw new System.NotImplementedException();
    }

    public void Leave()
    {
        Debug.Log("Leaving this cafe!");
    }

    public void MoveInLine()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            stressScript.IncreaseStress();
        }
    }
}
