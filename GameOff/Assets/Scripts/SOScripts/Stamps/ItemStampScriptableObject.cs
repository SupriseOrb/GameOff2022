using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemStampScriptableObject", menuName = "GameOff2022/Stamps/ItemStampSO", order = 2)]
public class ItemStampScriptableObject : StampScriptableObject
{
    #region SO Backing Fields
    [SerializeField] private float _itemStampValue;
    [SerializeField] private float _itemCooldown;
    [SerializeField] private int _itemHealth;
    #endregion

    #region SO Getters
    public float ItemStampValue
    {
        get {return _itemStampValue;}
    }
    public float ItemCooldown
    {
        get {return _itemCooldown;}
    }
    public int ItemHealth
    {
        get {return _itemHealth;}
    }
    #endregion

}
