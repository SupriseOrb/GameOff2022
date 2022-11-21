using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStamp
{
    void ActivateStampAbility();
    void SetLane(int laneNumber);
    void DisableStamp();
    void EnableStamp();
}
