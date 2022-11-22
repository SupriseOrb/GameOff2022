using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitStamp : IStamp
{
    void ActivateStampAttack();
    void LoadBaseStats();
    string GetStampName();
    void OpenUnitUpgrade();
    void UpgradeUnit(int upgradePath);
}
