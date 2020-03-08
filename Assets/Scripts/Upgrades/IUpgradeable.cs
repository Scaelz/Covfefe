using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradeable
{
    int UpgradeIndex { get; }
    void Upgrade(CustomUpgrade upgrade, int lvl, int maxLvl);
}
