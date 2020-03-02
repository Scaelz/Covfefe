using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Customer), typeof(AudioSource))]
public class CustomerSounds : MonoBehaviour
{
    [SerializeField] AudioClip[] angrySounds;
    [SerializeField] AudioClip[] joySounds;
    [SerializeField] Customer customer;
    [SerializeField] AudioSource source;

    private void Start()
    {
        customer.OnHappy += PlayHappySound;
        customer.OnRage += PlayAngrySound;
    }

    AudioClip GetRandomClip(AudioClip[] audioClips)
    {
        return audioClips[Random.Range(0, audioClips.Length)];
    }

    void PlayHappySound()
    {
        source.clip = GetRandomClip(joySounds);
        source.Play();
    }

    void PlayAngrySound()
    {
        source.clip = GetRandomClip(angrySounds);
        source.Play();
    }
}
