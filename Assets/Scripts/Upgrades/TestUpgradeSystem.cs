using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUpgradeSystem : MonoBehaviour
{
    public BaseUpgrade[] all_upgradable;
    public Text name;
    public Text description;
    public Action<int> buttonBuyOneFunctionOne;
    public Action buttonBuyOneFunctionMax;
    void Start()
    {
        all_upgradable = FindObjectsOfType<BaseUpgrade>();

        foreach (BaseUpgrade item in all_upgradable)
        {
            description.text = item.GetDescription();
            buttonBuyOneFunctionOne = (x) => item.ApplyUpgrade(x);
            buttonBuyOneFunctionMax = () =>
            {
                double cost;
                int count = item.GetPossibleUpgradeCount(1000, out cost);
                item.ApplyUpgrade(count);
            };
        }
    }


    public void ClickHandler()
    {
        buttonBuyOneFunctionOne?.Invoke(1);
    }

    public void ClickHandlerMax()
    {
        buttonBuyOneFunctionMax?.Invoke();
    }
}
