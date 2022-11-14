using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemStampScriptableObject", menuName = "GameOff2022/Stamps/ItemStampSO", order = 2)]
public class ItemStampScriptableObject : StampScriptableObject
{
    #region SO Backing Fields
    [SerializeField] private GameObject _spawnedItem;
    [SerializeField] private IStamp _stampAbilityScript;
    #endregion

    #region SO Getters
    public GameObject SpawnedItem
    {
        get {return _spawnedItem;}
    } 
    public IStamp StampAbilityScript
    {
        get {return _stampAbilityScript;}
    }
    #endregion

}
