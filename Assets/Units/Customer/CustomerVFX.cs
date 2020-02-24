using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Customer))]
public class CustomerVFX : MonoBehaviour
{
    [SerializeField]
    ParticleSystem rageEffect, happyEffect;
    Customer customer;

    // Start is called before the first frame update
    void Start()
    {
        customer = GetComponent<Customer>();
        customer.OnHappy += PlayHappyEffect;
        customer.OnRage += PlayRageEffect;
    }

    void PlayRageEffect()
    {
        rageEffect.Play();
    }

    void PlayHappyEffect()
    {
        happyEffect.Play();
    }
}
