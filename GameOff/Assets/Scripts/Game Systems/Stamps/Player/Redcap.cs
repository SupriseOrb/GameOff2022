using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redcap : Cannonry
{
    #region Redcap Info
    private int _pierceValue;
    private float _stunDuration;
    private bool _canStun = false;
    private enum RedcapUpgrades
    {
        Base = 0,
        LeadInk = 1,
        StickyInk = 2
    }
    #endregion

    #region Redcap Stats
    /*
    =================
    RNG Stats
    =================
    */
    private float _pierceUpgradeValue;
    private float _stunDurationUpgradeValue;
    /*
    =================
    Upgrade Path Stats
    =================
    */
    private float _stunDurationBaseValue; //Upgrade 2
    private float _pierceBaseValue; //Upgrade 1
    #endregion

    #region Properties
    public int PierceValue {get {return _pierceValue;}}
    public float StunDuration {get {return _stunDuration;}}
    public bool CanStun {get {return _canStun;}}
    #endregion

}
