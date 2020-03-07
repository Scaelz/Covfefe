using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public enum CustomUpgrade
{
    CustomerSpeed,
    CustomerHappyRate,
    CustomerHappyPower,
    WorkSpeed,
    LineLength,
    Popularity,
    CoffeeCost
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
    public CustomUpgrade GetCustomType() => upgradeType;

    public bool IncreaseLevel(int value)
    {
        Level += value;
        if (Level >= maxLevel)
        {
            return true;
        }

        return false;
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
        UpdateObjectsList();
        foreach (IUpgradeable item in objectsToUpgrade)
        {
            item.Upgrade(upgradeType);
        }

        SaveData();
    }

    abstract public List<IUpgradeable> UpdateObjectsList();

    abstract public Type GetUserType();

    abstract public void SaveData();

    abstract public void LoadData();
}

abstract public class Upgrade<T> : BaseUpgrade where T: Object, IUpgradeable
{
    private void Start()
    {
        UpdateObjectsList();
    }

    public override List<IUpgradeable> UpdateObjectsList()
    {
        objectsToUpgrade = new List<IUpgradeable>();
        foreach (IUpgradeable item in FindObjectsOfType<T>())
        {
            objectsToUpgrade.Add(item);
        } 
        return objectsToUpgrade;
    }

    public override Type GetUserType()
    {
        return typeof(T);
    }
}
