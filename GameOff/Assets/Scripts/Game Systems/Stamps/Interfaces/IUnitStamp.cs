using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitStamp : IStamp
{
    void ActivateStampAttack();
    void LoadBaseStats();
    void OpenUnitUpgrade();
    void ReduceCooldown(float reductionAmount);
    void UpgradeUnit(int upgradePath);
    string GetTileDescription();
}
