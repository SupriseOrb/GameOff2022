using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LandStampScriptableObject", menuName = "GameOff2022/Stamps/LandStampSO", order = 1)]
public class LandStampScriptableObject : StampScriptableObject
{
#region SO Backing Fields
    [SerializeField] private GameObject _spawnedLand;
    //not super important to have in here
    [SerializeField] private IStamp _stampAbilityScript;
#endregion

#region SO Getters
    public GameObject SpawnedLand
    {
        get {return _spawnedLand;}
    }
    public IStamp StampAbilityScript
    {
        get {return _stampAbilityScript;}
    }
#endregion
}
