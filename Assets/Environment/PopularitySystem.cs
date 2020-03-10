using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopularitySystem : MonoBehaviour, IUpgradeable
{
    public static PopularitySystem Instance;

    [SerializeField] float currentPopularity = 0.25f;
    [SerializeField] float negativeTik = 0.1f;
    [SerializeField] float minPositiveTik = 0.15f;
    [SerializeField] float maxPositiveTik = 1.15f;
    public float positiveTik;

    public int UpgradeIndex { get; } = 0;

    public event Action<float> OnPopularityChanged;

    // Start is called before the first frame update
    void Start()
    {
        TestUpgradeSystem.UpgradeRequest(GetType(), this);
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

    public void Upgrade(CustomUpgrade upgrade, int lvl, int maxLvl)
    {
        if (upgrade == CustomUpgrade.CustomerHappyPower)
        {
            float upgradeTick = (maxPositiveTik - minPositiveTik) / maxLvl;
            float newSpeed = minPositiveTik;
            positiveTik = minPositiveTik + upgradeTick * lvl;
        }
    }
}
