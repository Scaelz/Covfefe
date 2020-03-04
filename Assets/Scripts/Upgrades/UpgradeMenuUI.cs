using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenuUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmName;
    [SerializeField] TextMeshProUGUI tmDescription;
    [SerializeField] TextMeshProUGUI tmPriceForOne;
    [SerializeField] TextMeshProUGUI tmPriceForMax;
    [SerializeField] TextMeshProUGUI tmLvl;
    [SerializeField] TextMeshProUGUI tmBuyMaxButtonText;
    [SerializeField] Button buttonSingle;
    [SerializeField] Button buttonMax;
    [SerializeField] Image icon;
    public event Action<UpgradeMenuUI> OnSingleUpgradeClicked;
    public event Action<UpgradeMenuUI> OnMaxUpgradeClicked;

    public void SetInfo(int lvl, int maxLvl, string name, string description, 
        double price, int maxUpgradesPossible, double maxPrice, Sprite sprite)
    {
        UpdateLevelText(lvl, maxLvl);
        tmName.text = name;
        tmDescription.text = description;
        UpgradePricesTexts(price, maxPrice, maxUpgradesPossible);
        icon.sprite = sprite;
    }

    public void UpdateLevelText(int lvl, int maxLvl)
    {
        tmLvl.text = $"{lvl}/{maxLvl}";
    }

    public void SingleUpgradeButtonState(bool state)
    {
        buttonSingle.interactable = state;
    }

    public void MaxUpgradeButtonState(bool state)
    {
        buttonMax.interactable = state;
    }

    public void UpgradePricesTexts(double single, double max, int upgradesCount)
    {
        tmPriceForOne.text = single.ToString();
        tmPriceForMax.text = max.ToString();
        tmBuyMaxButtonText.text = $"Buy max ({upgradesCount.ToString()})";
    }

    public void SingleUpgradeHandler()
    {
        OnSingleUpgradeClicked?.Invoke(this);
    }

    public void MaxUpgradeHandler()
    {
        OnMaxUpgradeClicked?.Invoke(this);
    }
}
