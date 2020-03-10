using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCostomer : MonoBehaviour
{
    [SerializeField] AudioClip customerEntred;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = customerEntred;
    }

    private void OnTriggerEnter(Collider other)
    {
        audioSource.Play();
    }
}
