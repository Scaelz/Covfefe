using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradeable
{
    void Upgrade(CustomUpgrade upgrade, int lvl, int maxLvl);
}
