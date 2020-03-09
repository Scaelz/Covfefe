using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHappyPower : Upgrade<PopularitySystem>
{
    new void Awake()
    {
        LoadData();
        base.Awake();
    }

    public override void SaveData()
    {
        PlayerPrefs.SetInt(PrefsUtils.customerHappyPower, Level);
        PlayerPrefs.SetFloat(PrefsUtils.customerHappyPowerPrice, (float)currentPrice);
        PlayerPrefs.Save();
    }

    public override void LoadData()
    {
        Level = PlayerPrefs.GetInt(PrefsUtils.customerHappyPower);
        float price = PlayerPrefs.GetFloat(PrefsUtils.customerHappyPowerPrice);
        if (price != 0)
        {
            startPrice = price;
        }
    }
}
