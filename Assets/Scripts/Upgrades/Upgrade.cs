using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] protected float multiply;
    [SerializeField, TextArea()] protected string description;
    [SerializeField] protected Sprite sprite;
    [SerializeField] protected double startPrice;
    [SerializeField] protected double currentPrice;
    [SerializeField] protected List<IUpgradeable> objectsToUpgrade;
    [SerializeField] protected int Index;
    [SerializeField] bool isLocked = false;
    public event Action<bool> OnLockIsOff;

    public int GetLevel() => Level;
    public int GetMaxLevel() => maxLevel;
    public string GetName() => upgradeName;
    public string GetDescription() => description;
    public Sprite GetIconSprite() => sprite;
    public CustomUpgrade GetCustomType() => upgradeType;

    protected void Awake()
    {
        currentPrice = startPrice;
    }
    public bool IncreaseLevel(int value)
    {
        Level += value;
        if (Level >= maxLevel)
        {
            return true;
        }

        return false;
    }

    public void UnlockUpgrade()
    {
        isLocked = false;
        OnLockIsOff?.Invoke(isLocked);
    }

    public int GetIndex() => Index;

    public bool IsLocked()
    {
        return isLocked;
    }
    public double GetPrice()
    {
        return Math.Floor(currentPrice);
    }

    public int GetPossibleUpgradeCount(double currentCoins, out double cost)
    {
        int times = (int) Math.Floor(Math.Log(currentCoins * (multiply - 1) / (currentPrice) + 1, multiply));
        if (times > maxLevel - Level)
        {
            times = maxLevel - Level;
        }
        double proxyPrice = currentPrice;
        cost = currentPrice;
        if (times > 1)
        {
            for (int i = 0; i < times-1; i++)
            {
                proxyPrice *= multiply;
                cost += proxyPrice;
            }
        }
        cost = Math.Floor(cost);
        return times;
    }

    public virtual void ApplyUpgrade(int times = 1)
    {
        UpdateObjectsList();
        for (int i = 0; i < times; i++)
        {
            currentPrice *= multiply;
        }
        foreach (IUpgradeable item in objectsToUpgrade)
        {
            item.Upgrade(upgradeType, Level, maxLevel);
        }

        SaveData();
    }

    abstract public List<IUpgradeable> UpdateObjectsList();

    abstract public Type GetUserType();

    abstract public void SaveData();

    abstract public void LoadData();

    public void UpgradeInstance(IUpgradeable instance)
    {
        instance.Upgrade(upgradeType, Level, maxLevel);
    }
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
        var items = FindObjectsOfType<T>().Where(x => x.UpgradeIndex == Index);

        foreach (IUpgradeable item in items)
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
