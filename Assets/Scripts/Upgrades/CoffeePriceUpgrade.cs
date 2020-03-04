using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeePriceUpgrade : Upgrade<IdleConfig>
{
    private void Awake()
    {
        LoadData();
    }

    public override void SaveData()
    {
        PlayerPrefs.SetInt(PrefsUtils.coffeeCostUpgrade, Level);
        PlayerPrefs.Save();
    }

    public override void LoadData()
    {
        Level = PlayerPrefs.GetInt(PrefsUtils.coffeeCostUpgrade);
    }
}
