using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Combine with CardScriptableObject.cs
public class StampScriptableObject : ScriptableObject
{
    #region SO Backing Fields
    [SerializeField] private string _stampName;
    [SerializeField] private Sprite _stampSprite;
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
    #endregion
}
