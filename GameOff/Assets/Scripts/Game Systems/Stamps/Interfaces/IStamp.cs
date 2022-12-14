using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStamp
{
    void ActivateStampAbility();
    void SetLane(int laneNumber);
    string GetStampName();
    string GetTileDescription();
}
