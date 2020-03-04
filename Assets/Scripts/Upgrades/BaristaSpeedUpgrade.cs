using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaristaSpeedUpgrade : Upgrade<CoffeeWorker>
{
    [SerializeField] int baristaIndex;

    public override void SaveData()
    {
        string prefName = String.Format(PrefsUtils.baristaSpeedUpgrade, baristaIndex.ToString());
        PlayerPrefs.SetInt(prefName, Level);
        PlayerPrefs.Save();
    }

    public override void LoadData()
    {
        throw new NotImplementedException();
    }
}
