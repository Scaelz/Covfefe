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
    public int Level { get; protected set; }
    [SerializeField] protected int maxLevel;
    [SerializeField] protected string upgradeName;
    [SerializeField, TextArea()] protected string description;
    [SerializeField] protected Sprite sprite;
    [SerializeField] protected List<IUpgradeable> objectsToUpgrade;

    public string GetDescription() => description;

    public bool IsActive()
    {
        return default;
    }
    public double GetPrice()
    {
        return default;
    }

    public int GetPossibleUpgradeCount(double currentCoins, out double cost)
    {
        cost = default;
        return default;
    }

    public virtual void ApplyUpgrade(int times = 1)
    {
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
