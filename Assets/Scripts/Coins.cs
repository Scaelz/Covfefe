using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Coins : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinsText;
    //public static Coins Instance;
    private double coins;
    //public double CurrentCoins { get => coins; }

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
        IdleConfig _coins = FindObjectOfType<IdleConfig>();
        _coins.OnAddCoins += AddCoins;
        _coins.OnMinusCoins += MinusCoins;
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
        coinsText.text = coins.ToString("F0");
    }
}
