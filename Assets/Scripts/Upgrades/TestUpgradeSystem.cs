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
        CoinsUpdatedHandler(coins.GetCoins());
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

            int upgradesCount = upgrade.GetPossibleUpgradeCount(coins.GetCoins(), out double cost);
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
        BaseUpgrade upgrade = GetUpgradeByUI(menuUI);
        upgrade.ApplyUpgrade();
        double current_cost = upgrade.GetPrice();
        IncreaseUpgradeLevel(upgrade, menuUI, 1);
        config.SpentCoins(current_cost);
    }

    void MaxClickHandler(UpgradeMenuUI menuUI)
    {
        BaseUpgrade upgrade = GetUpgradeByUI(menuUI);
        int count = upgrade.GetPossibleUpgradeCount(coins.GetCoins(), out double cost);
        IncreaseUpgradeLevel(upgrade, menuUI, count);
        upgrade.ApplyUpgrade(count);
        config.SpentCoins(cost);
    }

    void IncreaseUpgradeLevel(BaseUpgrade upgrade, UpgradeMenuUI menuUI, int value)
    {
        bool maxReached = upgrade.IncreaseLevel(value);
        if (maxReached)
        {
            ChangeUpgradeButtonsState(menuUI, false);
            upgrades.Remove(upgrade);
        }
        menuUI.UpdateLevelText(upgrade.GetLevel(), upgrade.GetMaxLevel());
    }

    void CoinsUpdatedHandler(double value)
    {

        foreach (KeyValuePair<BaseUpgrade, UpgradeMenuUI> upgrade in upgrades)
        {
            if (!QuerySingleUpgradePossibility(upgrade.Key, value))
            {
                ChangeUpgradeButtonsState(upgrade.Value, false);
                upgrade.Value.UpgradePricesTexts(upgrade.Key.GetPrice(), upgrade.Key.GetPrice(), 1);
                continue;
            }

            int count = upgrade.Key.GetPossibleUpgradeCount(coins.GetCoins(), out double cost);
            ChangeUpgradeButtonsState(upgrade.Value, true);
            upgrade.Value.UpgradePricesTexts(upgrade.Key.GetPrice(), cost, count);
        }
    }

    void ChangeUpgradeButtonsState(UpgradeMenuUI menuUi, bool state)
    {
        menuUi.MaxUpgradeButtonState(state);
        menuUi.SingleUpgradeButtonState(state);
    }

    bool QuerySingleUpgradePossibility(BaseUpgrade upgrade, double value)
    {
        return upgrade.GetPrice() <= value;
    }

    public static void UpgradeRequest(Type type, IUpgradeable instance)
    {
        var baseUpgrades = FindObjectsOfType<BaseUpgrade>().Where(x => x.GetUserType() == type);
        foreach (BaseUpgrade upgrade in baseUpgrades)
        {
            upgrade.UpgradeInstance(instance);
        }
    }
}
