using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CustomUpgrade
{
    CustomerSpeed,
    CustomerHappyRate,
    CustomerHappyPower,
    WorkSpeed,
    LineLength,
    Popularity,
}

abstract public class BaseUpgrade: MonoBehaviour
{
    [SerializeField] protected CustomUpgrade upgradeType;
    public int Level { get; protected set; } = 1;
    [SerializeField] protected int maxLevel;
    [SerializeField] protected string upgradeName;
    [SerializeField, TextArea()] protected string description;
    [SerializeField] protected Sprite sprite;
    [SerializeField] protected double startPrice;
    [SerializeField] protected List<IUpgradeable> objectsToUpgrade;

    public int GetLevel() => Level;
    public int GetMaxLevel() => maxLevel;
    public string GetName() => upgradeName;
    public string GetDescription() => description;
    public Sprite GetIconSprite() => sprite;

    public void IncreaseLevel(int value)
    {
        Level += value;
    }

    public bool IsActive()
    {
        return default;
    }
    public double GetPrice()
    {
        return startPrice * Level;
    }

    public int GetPossibleUpgradeCount(double currentCoins, out double cost)
    {
        double price = 0;
        cost = 0;
        int result = 0;
        int times = 0;
        for (int i = Level; i < maxLevel + 1; i++)
        {
            price += startPrice * i;
            if(price >= currentCoins)
            {
                if (times != 0)
                {
                    price -= startPrice * i;
                }
                else
                {
                    times = 1;
                }
                cost = price;
                break;
            }
            times++;
        }
        return times;
    }

    public virtual void ApplyUpgrade(int times = 1)
    {
        Debug.Log($"Upgrade ({upgradeName}) applied {times} times");
    }
}

abstract public class Upgrade<T> : BaseUpgrade where T: Object, IUpgradeable
{
    public virtual List<IUpgradeable> UpdateObjectsList()
    {
        if (objectsToUpgrade == null)
        {
            objectsToUpgrade = new List<T>(FindObjectsOfType<T>()) as List<IUpgradeable>;
        }
        return objectsToUpgrade;
    }
}
