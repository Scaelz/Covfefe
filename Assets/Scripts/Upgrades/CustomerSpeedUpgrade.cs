using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpeedUpgrade : Upgrade<Customer>
{
    new void Awake()
    {
        LoadData();
        base.Awake();
    }

    public override void SaveData()
    {
        PlayerPrefs.SetInt(PrefsUtils.customerSpeedUpgrade, Level);
        PlayerPrefs.SetFloat(PrefsUtils.customerSpeedUpgradePrice, (float)currentPrice);
        PlayerPrefs.Save();
    }

    public override void LoadData()
    {
        Level = PlayerPrefs.GetInt(PrefsUtils.customerSpeedUpgrade);
        float price = PlayerPrefs.GetFloat(PrefsUtils.customerSpeedUpgradePrice);
        if (price != 0)
        {
            startPrice = price;
        }
    }
}
