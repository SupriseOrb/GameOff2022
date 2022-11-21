using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LandStampScriptableObject", menuName = "GameOff2022/Stamps/LandStampSO", order = 1)]
public class LandStampScriptableObject : StampScriptableObject
{
#region SO Backing Fields
    //not super important to have in here
    [SerializeField] private float _stampAbilityCooldown;
    [SerializeField] private float _stampAbilityValue;
#endregion

#region SO Getters
    public float StampAbilityCooldown
    {
        get {return _stampAbilityCooldown;}
    }
    public float StampAbilityValue
    {
        get {return _stampAbilityValue;}
    }
#endregion
}
