using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class IdleConfig : MonoBehaviour, IUpgradeable
{
    [Header("Text Fields")]
    //[SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] TextMeshProUGUI clickUpgradeText;
    [SerializeField] TextMeshProUGUI currentLevelText;
    [SerializeField] TextMeshProUGUI coinsPerCoffeeText;
    [SerializeField] TextMeshProUGUI buyMaxText;
    [SerializeField] TextMeshProUGUI spawnFrequincyText;

    [Header("Coins Fields")]
    public double coinsClickValue = 1;
    public int defaultCoinsPrice = 1;
    //public double coinsPerSecond;

    [Header("Upgrade Fields")]
    private double clickUpgradeCost;
    public double clickUpgradeCostStarting = 10;
    public int clickUpgradeLevel;

    [Header("Multyply")]
    public double multiply = 1.07;
    public int[] multyplyUpgrade;

    [Header("String Format")]

    [TextArea]
    public string[] stringFieldsText;

    Coins _coins;

    // private
    private double buyMaxLevels;
    List<string> charExponent = new List<string>() { "k", "M", "G", "T", "P", "E", "Z", "Y" };

    // Event
    public event Action<double> OnAddCoins;
    public event Action<double> OnMinusCoins;

    private void Start()
    {
        //_coins = Coins.Instance;
        clickUpgradeCost = clickUpgradeCostStarting;
        _coins = FindObjectOfType<Coins>();
        FindObjectOfType<CustomerSpawner>().OnSpawnFrequencyChanged += UpdateFrequencyText;
        clickUpgradeLevel = PlayerPrefs.GetInt(PrefsUtils.coffee_lvl);
        //ClickUpgradeMultyplyCost();
        SetCoinsPriceViaLvl();
        SetUpgradeCostViaLvl();
        SetTextValue();
        TestUpgradeSystem.UpgradeRequest(GetType(), this);
    }

    void UpdateFrequencyText(float value)
    {
        spawnFrequincyText.text = Math.Round(value, 2).ToString() + " s";
    }

    private void SetTextValue()
    {
        buyMaxLevels = Math.Floor(Math.Log(_coins.GetCoins() * (multiply - 1) / (clickUpgradeCost) + 1, multiply)); // * Math.Pow(multiply, 0)

        //currentLevelText.text = stringFieldsText[0] + clickUpgradeLevel;
        //SetExponentText(Math.Round(coins), coinsText, stringFieldsText[1]);
        SetExponentText(coinsClickValue, coinsPerCoffeeText, stringFieldsText[2]);
        //SetExponentText(Math.Round(clickUpgradeCost), clickUpgradeText, stringFieldsText[3]);
        //SetExponentText(Math.Round(buyMaxLevels), buyMaxText, stringFieldsText[4]);
    }

    // Exponent
    public void SetExponentText(double numberChange, TextMeshProUGUI fieldText, string formatString = "{0}")
    {
        if (numberChange >= 1000)
        {
            var exponent = (Math.Floor(Math.Log10(Math.Abs(numberChange))));
            Math.DivRem((int)exponent, 3, out int ostatok);
            exponent -= ostatok;
            var multyplyExponent = (numberChange / Math.Pow(10, exponent));

            string tempCoinsText = string.Format(formatString, multyplyExponent.ToString("F2"));
            ExponentView(exponent, tempCoinsText, fieldText);
        }
        else
        {
            fieldText.text = string.Format(formatString, numberChange.ToString("F0"));
        }
    }
    private void ExponentView(double exponent, string tempCoinsText, TextMeshProUGUI coinsChange)
    {
        var divisionExponent = (exponent / 3) - 1;
        if (divisionExponent < charExponent.Count)
        {
            coinsChange.text = tempCoinsText + charExponent[(int)divisionExponent];
        }
        else
        {
            coinsChange.text = tempCoinsText + "e" + exponent;
        }
    }

    private void ClickUpgradeMultyplyCost()
    {
        switch (clickUpgradeLevel)
        {
            case int n when (clickUpgradeLevel % 100 == 25):
                defaultCoinsPrice *= multyplyUpgrade[0];
                coinsClickValue *= multyplyUpgrade[0];
                break;
            case int n when (clickUpgradeLevel % 100 == 50):
                defaultCoinsPrice *= multyplyUpgrade[1];
                coinsClickValue *= multyplyUpgrade[1];
                break;
            case int n when (clickUpgradeLevel % 100 == 75):
                defaultCoinsPrice *= multyplyUpgrade[2];
                coinsClickValue *= multyplyUpgrade[2];
                break;
            case int n when (n >= 100 & clickUpgradeLevel % 100 == 0):
                defaultCoinsPrice *= multyplyUpgrade[3];
                coinsClickValue *= multyplyUpgrade[3];
                break;
        }
    }

    // Buttons
    public void Click(double value = default)
    {
        if (value == default)
        {
            value = coinsClickValue;
        }

        OnAddCoins?.Invoke(value);
        //_coins.coins += coinsClickValue;
        //coins += coinsClickValue;
        SetTextValue();
    }

    void SetCoinsPriceViaLvl()
    {
        for (int i = 0; i < clickUpgradeLevel; i++)
        {
            coinsClickValue += defaultCoinsPrice;
        }
    }

    void SetUpgradeCostViaLvl()
    {
        for (int i = 0; i < clickUpgradeLevel; i++)
        {
            clickUpgradeCost *= multiply;
        }
    }

    public void SpentCoins(double value)
    {
        OnMinusCoins?.Invoke(value);
    }

    public void BuyClickUpgrade1(bool save_progress=true)
    {
        if (Math.Round(_coins.GetCoins()) >= Math.Round(clickUpgradeCost)) 
        { 
            OnMinusCoins?.Invoke(clickUpgradeCost);

            if (_coins.GetCoins() < 0) _coins.SetCoins(0);
            clickUpgradeCost *= multiply;
            //clickUpgradeCost += clickUpgradeCostStarting * clickUpgradeLevel + clickUpgradeCostStarting / Math.Pow(clickUpgradeLevel, multiply);
            coinsClickValue += defaultCoinsPrice;
            clickUpgradeLevel++;
            ClickUpgradeMultyplyCost();
            SetTextValue();
        }
        if (save_progress)
        {
            SaveProgress();
        }
    }

    public void BuyMaxLevels()
    {
        var tempLevel = clickUpgradeLevel + Math.Round(buyMaxLevels);
        while (clickUpgradeLevel < tempLevel)
        {
            BuyClickUpgrade1(save_progress: false);
        }
        SaveProgress();
    }

    void SaveProgress()
    {
        PlayerPrefs.SetInt(PrefsUtils.coffee_lvl, clickUpgradeLevel);
        PlayerPrefs.Save();
    }

    public void ClearProgress()
    {
        PlayerPrefs.SetInt(PrefsUtils.coffee_lvl, 0);
        PlayerPrefs.SetFloat(PrefsUtils.money, 0);
        PlayerPrefs.SetInt(PrefsUtils.cashbox, 0);
        PlayerPrefs.SetString(PrefsUtils.onlineDate, "");
        PlayerPrefs.SetInt(PrefsUtils.customerSpeedUpgrade, 1);
        PlayerPrefs.SetInt(PrefsUtils.coffeeCostUpgrade, 1);
        PlayerPrefs.SetInt(PrefsUtils.baristaSpeedUpgrade, 1);
        PlayerPrefs.SetInt(PrefsUtils.customerHappyPower, 1);
        PlayerPrefs.SetInt(PrefsUtils.customerHappyPercent, 1);

        PlayerPrefs.Save();
    }

    public void Upgrade(CustomUpgrade upgrade, int lvl, int maxLvl)
    {
        coinsClickValue = lvl * 2.43f;
        SetTextValue();
    }
}
