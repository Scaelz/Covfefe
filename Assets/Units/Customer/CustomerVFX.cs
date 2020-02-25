using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Customer))]
public class CustomerVFX : MonoBehaviour
{
    [SerializeField]
    ParticleSystem rageEffect, happyEffect;
    [SerializeField] Material[] materials;
    [SerializeField] SkinnedMeshRenderer renderer;
    Customer customer;

    // Start is called before the first frame update
    void Start()
    {
        //renderer = GetComponent<MeshRenderer>();
        customer = GetComponent<Customer>();
        customer.OnHappy += PlayHappyEffect;
        customer.OnRage += PlayRageEffect;
        customer.OnEnabling += GetRandomLook;
    }

    void GetRandomLook()
    {
        Material random_mat = materials[Random.Range(0, materials.Length)];
        renderer.material = random_mat;
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
