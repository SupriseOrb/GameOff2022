using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampScriptableObject : ScriptableObject
{
    #region SO Backing Fields
    [SerializeField] private string _stampName;
    [SerializeField] private Sprite _stampSprite;
    [SerializeField] private int _stampHealth;
    #endregion

    #region SO Getters
    public string StampName
    {
        get {return _stampName;}
    }

    public Sprite StampSprite
    {
        get {return _stampSprite;}
    }

    public int StampHealth
    {
        get {return _stampHealth;}
    }
    #endregion
}
