using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneStampScriptableObject : ScriptableObject
{
#region SO Backing Fields
    [SerializeField] private string _stampName;
    [SerializeField] private int _stampCost;
    //not super important to have in here
    [SerializeField] private IStamp _stampAbilityScript;
    [SerializeField] private Sprite _stampImage;
#endregion

#region SO Getters
    public string StampName
    {
        get {return _stampName;}
    }

    public int StampCost
    {
        get {return _stampCost;}
    }

    public IStamp StampAbilityScript
    {
        get {return _stampAbilityScript;}
    }

    public Sprite StampImage
    {
        get {return _stampImage;}
    }
#endregion
}
