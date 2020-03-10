using System;
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
    IStressable stressScript;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip rage, cashRegister, coffeeMade;
    [SerializeField] float volume;

    private void Start()
    {
        stressScript = GetComponent<IStressable>();
        worker = GetComponent<CoffeeWorker>();
        worker.OnSpeedMultiplierChanged += ControllSystem;
        worker.OnWorkDone += SpawnAnnotation;
        worker.OnWorkDone += PlayWorkDoneSound;
        worker.OnWorkStarted += PlayWorkSound;
        stressScript.OnStressOut += PlayRageSound;
    }

    void PlayRageSound(object e, EventArgs args)
    {
        //AudioSource.PlayClipAtPoint(rage, transform.position, 3f);
        audioSource.clip = rage;
        audioSource.pitch = UnityEngine.Random.Range(0.45f, 1.85f);
        audioSource.volume = volume * 1.0f;
        audioSource.Play();
    }

    void PlayWorkSound()
    {
        audioSource.clip = coffeeMade;
        audioSource.pitch = UnityEngine.Random.Range(0.01f, 1.85f);
        audioSource.volume = volume * 0.8f;
        audioSource.Play();
    }

    void PlayWorkDoneSound()
    {
        audioSource.clip = cashRegister;
        audioSource.pitch = UnityEngine.Random.Range(0.01f, 1.85f);
        audioSource.volume = volume * 1.45f;
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
