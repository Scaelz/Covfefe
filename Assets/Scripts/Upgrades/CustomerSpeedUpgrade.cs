using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpeedUpgrade : Upgrade<Customer>
{
    public override void SaveData()
    {
        PlayerPrefs.SetInt(PrefsUtils.customerSpeedUpgrade, Level);
        PlayerPrefs.Save();
    }

    public override void LoadData()
    {
        
    }
}
