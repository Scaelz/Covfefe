using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHappyPower : Upgrade<Customer>
{
    public override void SaveData()
    {
        PlayerPrefs.SetInt(PrefsUtils.customerHappyPower, Level);
        PlayerPrefs.Save();
    }

    public override void LoadData()
    {

    }
}
