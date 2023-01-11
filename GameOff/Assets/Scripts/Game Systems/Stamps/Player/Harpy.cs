using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpy : Cannonry
{
    #region Harpy Info
    private float _moveSlowIntensity;
    private float _moveSlowDuration;
    private float _pushDistance;
    private float _pushSpeed;
    private enum HarpyUpgrades
    {
        Base = 0,
        DisorientingSong = 1,
        BoomingSong = 2
    }
    #endregion

    #region Harpy Stats
    /*
    =================
    RNG Stats
    =================
    */
    private float _slowIntensityUpgradeValue;
    private float _pushDistanceUpgradeValue;
    /*
    =================
    Upgrade Path Stats
    =================
    */
    private float _cooldownReductionDowngradeValue; //Upgrade 1
    #endregion

    #region Properties
    /*
    =================
    Harpy Info
    =================
    */
    public float MoveSlowIntensity {get {return _moveSlowIntensity;}}
    public float MoveSlowDuration {get {return _moveSlowDuration;}}
    public float PushDistance {get {return _pushDistance;}}
    public float PushSpeed {get {return _pushSpeed;}}
    #endregion

}
