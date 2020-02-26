using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopularitySystem : MonoBehaviour
{
    public static PopularitySystem Instance;

    [SerializeField] float currentPopularity = 0;
    [SerializeField] float negativeTik = 0.1f;
    [SerializeField] float positiveTik = 0.15f;

    public event Action<float> OnPopularityChanged;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        OnPopularityChanged += ClampPopularity;
    }

    public float GetPopularity()
    {
        return currentPopularity;
    }

    public void AddPopularity()
    {
        currentPopularity += positiveTik;
        OnPopularityChanged?.Invoke(currentPopularity);
        Debug.Log("Increased");
    }

    public void DecreasePopularity()
    {
        currentPopularity -= negativeTik;
        OnPopularityChanged?.Invoke(currentPopularity);
        Debug.Log("Decrease");
    }

    void ClampPopularity(float value)
    {
        currentPopularity = Mathf.Clamp(value, 0, 1);
    }
}
