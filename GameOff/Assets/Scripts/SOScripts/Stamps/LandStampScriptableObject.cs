using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LandStampScriptableObject", menuName = "GameOff2022/Stamps/LandStampSO", order = 1)]
public class LandStampScriptableObject : StampScriptableObject
{
#region SO Backing Fields
    [SerializeField] private float _landStampAbilityCooldown;
    [SerializeField] private float _landStampAbilityValue;
#endregion

#region SO Getters
    public float LandAbilityCooldown
    {
        get {return _landStampAbilityCooldown;}
    }
    public float LandAbilityValue
    {
        get {return _landStampAbilityValue;}
    }
#endregion
}
