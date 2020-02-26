using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class IdleConfig : MonoBehaviour
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
    public double coinsPerSecond;

    [Header("Upgrade Fields")]
    public double clickUpgradeCost = 10;
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
        _coins = FindObjectOfType<Coins>();
        FindObjectOfType<CustomerSpawner>().OnSpawnFrequencyChanged += UpdateFrequencyText;
        SetTextValue();
    }

    void UpdateFrequencyText(float value)
    {
        spawnFrequincyText.text = Math.Round(value, 2).ToString() + " s";
    }

    private void SetTextValue()
    {
        buyMaxLevels = Math.Floor(Math.Log(_coins.GetCoins() * (multiply - 1) / (clickUpgradeCost) + 1, multiply)); // * Math.Pow(multiply, 0)

        currentLevelText.text = stringFieldsText[0] + clickUpgradeLevel;
        //SetExponentText(Math.Round(coins), coinsText, stringFieldsText[1]);
        SetExponentText(coinsClickValue, coinsPerCoffeeText, stringFieldsText[2]);
        SetExponentText(Math.Round(clickUpgradeCost), clickUpgradeText, stringFieldsText[3]);
        SetExponentText(Math.Round(buyMaxLevels), buyMaxText, stringFieldsText[4]);
    }

    // Exponent
    private void SetExponentText(double numberChange, TextMeshProUGUI fieldText, string formatString = "{0}")
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
                defaultCoinsPrice *= multyplyUpgrade[0];
                coinsClickValue *= multyplyUpgrade[0];
                break;
            case int n when (clickUpgradeLevel % 100 == 75):
                defaultCoinsPrice *= multyplyUpgrade[0];
                coinsClickValue *= multyplyUpgrade[0];
                break;
            case int n when (n >= 100 & clickUpgradeLevel % 100 == 0):
                defaultCoinsPrice *= multyplyUpgrade[0];
                coinsClickValue *= multyplyUpgrade[0];
                break;
        }
    }

    // Buttons
    public void Click()
    {
        //Debug.Log(_coins.GetCoins());
        OnAddCoins?.Invoke(coinsClickValue);

        //_coins.coins += coinsClickValue;
        //coins += coinsClickValue;
        SetTextValue();
    }

    public void BuyClickUpgrade1()
    {
        if (Math.Round(_coins.GetCoins()) >= Math.Round(clickUpgradeCost)) 
        { 
            clickUpgradeLevel++;
            OnMinusCoins?.Invoke(clickUpgradeCost);

            if (_coins.GetCoins() < 0) _coins.SetCoins(0);
            clickUpgradeCost *= multiply;
            coinsClickValue += defaultCoinsPrice;
            ClickUpgradeMultyplyCost();
            SetTextValue();
        }
    }

    public void BuyMaxLevels()
    {
        var tempLevel = clickUpgradeLevel + Math.Round(buyMaxLevels);
        while (clickUpgradeLevel < tempLevel)
        {
            BuyClickUpgrade1();
        }
    }
}
