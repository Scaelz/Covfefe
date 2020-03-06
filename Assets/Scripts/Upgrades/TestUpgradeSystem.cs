using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TestUpgradeSystem : MonoBehaviour
{
    BaseUpgrade[] all_upgradable;
    [SerializeField] GameObject menuPrefab;
    [SerializeField] Transform MenuContainer;
    Dictionary<BaseUpgrade, UpgradeMenuUI> upgrades = new Dictionary<BaseUpgrade, UpgradeMenuUI>();
    Coins coins;
    IdleConfig config;

    void Start()
    {
        coins = FindObjectOfType<Coins>();
        config = FindObjectOfType<IdleConfig>();
        
        InitializeUpgrades();
        coins.OnCoinsChanged += CoinsUpdatedHandler;
    }

    void InitializeUpgrades()
    {
        all_upgradable = FindObjectsOfType<BaseUpgrade>();
        foreach (BaseUpgrade upgrade in all_upgradable)
        {
            GameObject menu = Instantiate(menuPrefab);
            menu.transform.SetParent(MenuContainer);
            menu.transform.localScale = Vector3.one;

            UpgradeMenuUI menuUI = menu.GetComponent<UpgradeMenuUI>();
            upgrades.Add(upgrade, menuUI);

            int upgradesCount = upgrade.GetPossibleUpgradeCount(1500, out double cost);
            menuUI.SetInfo(upgrade.GetLevel(), upgrade.GetMaxLevel(), upgrade.GetName(), upgrade.GetDescription(),
                upgrade.GetPrice(), upgradesCount, cost, upgrade.GetIconSprite());
            menuUI.OnSingleUpgradeClicked += SingleClickHandler;
            menuUI.OnMaxUpgradeClicked += MaxClickHandler;
        }
    }

    BaseUpgrade GetUpgradeByUI(UpgradeMenuUI menuUI)
    {
        return upgrades.FirstOrDefault(x => x.Value == menuUI).Key;
    }

    void SingleClickHandler(UpgradeMenuUI menuUI)
    {
        GetUpgradeByUI(menuUI).ApplyUpgrade();
    }

    void MaxClickHandler(UpgradeMenuUI menuUI)
    {
        BaseUpgrade upgrade = GetUpgradeByUI(menuUI);
        int count = upgrade.GetPossibleUpgradeCount(coins.GetCoins(), out double cost);
        upgrade.ApplyUpgrade(count);
    }

    void CoinsUpdatedHandler(double value)
    {
        foreach (KeyValuePair<BaseUpgrade, UpgradeMenuUI> upgrade in upgrades)
        {
            if (!QuerySingleUpgradePossibility(upgrade.Key, value))
            {
                ChangeUpgradeButtonsState(upgrade.Value, false);
                upgrade.Value.UpdateBuyMaxCountAndPrice(1, upgrade.Key.GetPrice());
                continue;
            }

            int count = upgrade.Key.GetPossibleUpgradeCount(coins.GetCoins(), out double cost);
            ChangeUpgradeButtonsState(upgrade.Value, true);
            upgrade.Value.UpdateBuyMaxCountAndPrice(count, cost);
        }
    }

    void ChangeUpgradeButtonsState(UpgradeMenuUI menuUi, bool state)
    {
        menuUi.MaxUpgradeButtonState(state);
        menuUi.SingleUpgradeButtonState(state);
    }

    bool QuerySingleUpgradePossibility(BaseUpgrade upgrade, double value)
    {
        return upgrade.GetPrice() < value;
    }
}
