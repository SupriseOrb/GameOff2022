using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitStamp : IStamp
{
    void ActivateStampAttack();
    void LoadBaseStats();
    string GetUnitName();
    void UpgradeUnit();
    void SetLane(int lane);
}
