using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    protected IMovable moveScript;
    protected IStressable stressScript; 


    protected void Initialize()
    {
        moveScript = GetComponent<IMovable>();
        stressScript = GetComponent<IStressable>();
    }
}
