using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStamp
{
    void ActivateStampAbility();
    // TODO : Make sure naming conventions are the same
    // TODO : Move this to a stamp class because functionality is the same
    void SetLane(int laneNumber);
    // TODO : Make sure naming conventions are the same
    // TODO : Move this to a stamp class because functionality is the same
    string GetStampName();
    string GetTileDescription();
}
