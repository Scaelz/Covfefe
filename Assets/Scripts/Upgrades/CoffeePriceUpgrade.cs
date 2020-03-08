using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeePriceUpgrade : Upgrade<IdleConfig>
{
    new void Awake()
    {
        LoadData();
        base.Awake();
    }

    public override void SaveData()
    {
        PlayerPrefs.SetInt(PrefsUtils.coffeeCostUpgrade, Level);
        PlayerPrefs.SetFloat(PrefsUtils.coffeeCostUpgradePrice, (float)currentPrice);
        PlayerPrefs.Save();
    }

    public override void LoadData()
    {
        Level = PlayerPrefs.GetInt(PrefsUtils.coffeeCostUpgrade);
        float price = PlayerPrefs.GetFloat(PrefsUtils.coffeeCostUpgradePrice);
        if (price != 0)
        {
            startPrice = price;
        }
    }
}
