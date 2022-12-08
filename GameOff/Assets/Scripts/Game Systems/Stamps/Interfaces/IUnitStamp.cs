using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitStamp : IStamp
{
    // TODO : Consolidate with ActivateStampAbility()
    void ActivateStampAttack();
    // TODO : Make sure naming conventions are the same
    void LoadBaseStats();
    // TODO : Make sure naming conventions are the same
    // TODO : Move this to a base unit stamp class because functionality is the same
    void OpenUnitUpgrade();
    // TODO : Make sure naming conventions are the same
    // TODO : Move this to a base unit stamp class because functionality is the same
    void ReduceCooldown(float reductionAmount);
    void UpgradeUnit(int upgradePath);
}
