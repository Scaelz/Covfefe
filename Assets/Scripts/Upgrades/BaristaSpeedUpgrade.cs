using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaristaSpeedUpgrade : Upgrade<CoffeeWorker>
{
    string prefName;
    string pricePrefName;

    new void Awake()
    {
        prefName = String.Format(PrefsUtils.baristaSpeedUpgrade, Index.ToString());
        pricePrefName = String.Format(PrefsUtils.baristaSpeedUpgradePrice, Index.ToString());
        LoadData();
        base.Awake();
    }

    public override void SaveData()
    {
        PlayerPrefs.SetInt(prefName, Level);
        PlayerPrefs.SetFloat(pricePrefName, (float)currentPrice);
        PlayerPrefs.Save();
    }

    public override void LoadData()
    {
        Level = PlayerPrefs.GetInt(prefName);
        float price = PlayerPrefs.GetFloat(pricePrefName);
        if (price != 0)
        {
            startPrice = price;
        }
    }
}
