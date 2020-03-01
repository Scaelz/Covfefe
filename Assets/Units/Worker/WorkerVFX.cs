using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CoffeeWorker))]
public class WorkerVFX : MonoBehaviour
{
    [SerializeField]
    ParticleSystem stressEffectPrefab;
    [SerializeField] FloatingText floatingText;
    CoffeeWorker worker;
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        worker = GetComponent<CoffeeWorker>();
        worker.OnSpeedMultiplierChanged += ControllSystem;
        worker.OnWorkDone += SpawnAnnotation;
        worker.OnWorkDone += PlaySound;
    }

    void PlaySound()
    {
        audioSource.Play();
    }

    void SpawnAnnotation()
    {
        floatingText.StartFloat();
    }

    void ControllSystem(float value)
    {
        var emission = stressEffectPrefab.emission;
        emission.rateOverTime = 5 * value;
        if (value - 1 < .1f)
        {
            emission.rateOverTime = 0;
        }
    }
}
