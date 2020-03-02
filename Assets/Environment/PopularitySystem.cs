using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopularitySystem : MonoBehaviour
{
    public static PopularitySystem Instance;

    [SerializeField] float currentPopularity = 0.25f;
    [SerializeField] float negativeTik = 0.1f;
    [SerializeField] float positiveTik = 0.85f;

    public event Action<float> OnPopularityChanged;

    // Start is called before the first frame update
    void Start()
    {
        currentPopularity = ClampPopularity(currentPopularity);
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public float GetPopularity()
    {
        return currentPopularity;
    }

    public void AddPopularity()
    {
        currentPopularity = ClampPopularity(currentPopularity + positiveTik);
        OnPopularityChanged?.Invoke(currentPopularity);
    }

    public void DecreasePopularity()
    {
        currentPopularity = ClampPopularity(currentPopularity - negativeTik);
        OnPopularityChanged?.Invoke(currentPopularity);
    }

    float ClampPopularity(float value)
    {
        return Mathf.Clamp(value, .25f, 5);
    }
}
