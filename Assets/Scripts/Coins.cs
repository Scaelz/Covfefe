using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Coins : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinsText;
    //public static Coins Instance;
    [SerializeField] private double coins;
    //public double CurrentCoins { get => coins; }
    IdleConfig _coins;

    private void Awake()
    {
        /*
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        */
        coins = PlayerPrefs.GetFloat(PrefsUtils.money);
        _coins = FindObjectOfType<IdleConfig>();
        _coins.OnAddCoins += AddCoins;
        _coins.OnMinusCoins += MinusCoins;
        _coins.OnAddCoins += SaveProgress;
        _coins.OnMinusCoins += SaveProgress;
        SetCoinsText();
    }
    

    void SaveProgress(double value)
    {
        PlayerPrefs.SetFloat(PrefsUtils.money, (float)coins);
        PlayerPrefs.Save();
    }

    public double GetCoins()
    {
        //Debug.Log("Coins: " + coins);
        return coins;
    }

    public void SetCoins(double value)
    {
        //Debug.Log("Set coins: " + coins);
        coins = value;
    }

    private void MinusCoins(double value)
    {
        coins -= value;
        SetCoinsText();
    }

    private void AddCoins(double value)
    {
        //Debug.Log(value);
        coins += value;
        SetCoinsText();
    }
    
    private void SetCoinsText()
    {
        _coins.SetExponentText(coins, coinsText);
        //coinsText.text = coins.ToString("F0");
    }
}
