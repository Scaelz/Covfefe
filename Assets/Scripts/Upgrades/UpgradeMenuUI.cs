using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenuUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TextMeshProUGUI tmName;
    [SerializeField] TextMeshProUGUI tmDescription;
    [SerializeField] TextMeshProUGUI tmPriceForOne;
    [SerializeField] TextMeshProUGUI tmPriceForMax;
    [SerializeField] TextMeshProUGUI tmLvl;
    [SerializeField] TextMeshProUGUI tmBuyMaxButtonText;
    [Header("")]
    [SerializeField] GameObject progressBarFill;
    [SerializeField] Button buttonSingle;
    [SerializeField] Button buttonMax;
    [SerializeField] Image icon;
    [SerializeField] Image blockImage;
    [SerializeField] AudioClip buySound;

    public event Action<UpgradeMenuUI> OnSingleUpgradeClicked;
    public event Action<UpgradeMenuUI> OnMaxUpgradeClicked;

    public void SetBlockScreenState(bool state)
    {
        blockImage.enabled = state;
    }

    public void SetInfo(int lvl, int maxLvl, string name, string description, 
        double price, int maxUpgradesPossible, double maxPrice, Sprite sprite)
    {
        UpdateLevelText(lvl, maxLvl);
        UpdateProgressBar(lvl, maxLvl);
        tmName.text = name;
        tmDescription.text = description;
        UpgradePricesTexts(price, maxPrice, maxUpgradesPossible);
        icon.sprite = sprite;
    }

    public void UpdateLevelText(int lvl, int maxLvl)
    {
        tmLvl.text = $"{lvl}/{maxLvl}";
    }

    public void UpdateProgressBar(int lvl, int maxLvl)
    {
        float percent = lvl * 100 / maxLvl;
        progressBarFill.GetComponent<Image>().fillAmount = percent/100;
        progressBarFill.GetComponent<Image>().color = new Color((1 - percent / 100)/3, 0.45f, 0.13f);
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
        tmPriceForOne.text = Exponent.SetExponentText(single);
        tmPriceForMax.text = Exponent.SetExponentText(max);
        tmBuyMaxButtonText.text = $"Buy max ({upgradesCount.ToString()})";
    }

    public void SingleUpgradeHandler()
    {
        OnSingleUpgradeClicked?.Invoke(this);
        PlayBuySound();
    }

    public void MaxUpgradeHandler()
    {
        OnMaxUpgradeClicked?.Invoke(this);
        PlayBuySound();
    }

    void PlayBuySound()
    {
        AudioSource.PlayClipAtPoint(buySound, Camera.main.transform.position, 1.0f);
    }
}
