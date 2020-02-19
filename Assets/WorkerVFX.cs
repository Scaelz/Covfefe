using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CoffeeWorker))]
public class WorkerVFX : MonoBehaviour
{
    [SerializeField]
    ParticleSystem stressEffectPrefab;


    CoffeeWorker worker;
    private void Start()
    {
        worker = GetComponent<CoffeeWorker>();
        worker.OnSpeedMultiplierChanged += ControllSystem;
    }

    void ControllSystem(float value)
    {
        Debug.Log(value);
        var emission = stressEffectPrefab.emission;
        emission.rateOverTime = 5 * value;
        if (value - 1 < .1f)
        {
            emission.rateOverTime = 0;
        }
    }
}
