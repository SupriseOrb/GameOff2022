using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitStamp : IStamp
{
    void ActivateStampAttack();
    void LoadBaseStats();
    string GetUnitName();
    void OpenUnitUpgrade();
    void UpgradeUnit(int upgradePath);
    void SetLane(int lane);
}