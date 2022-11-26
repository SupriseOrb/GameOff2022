using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStampScriptableObject", menuName = "GameOff2022/Stamps/UnitStampSO", order = 0)]
public class UnitStampScriptableObject : StampScriptableObject
{
#region SO Backing Fields
    [Header("Unit Stats")]
    [SerializeField] private GameObject _spawnedUnit;
    [SerializeField] private string _unitType;
    [SerializeField] private float _unitAbilityCooldown;
    [SerializeField] private float _unitCooldownReduction;
    [SerializeField] private float _unitDamage;
    [SerializeField] private float _unitAttackSpeed;
    [SerializeField] private float _unitSlowAmount;
    [SerializeField] private int _unitHealth;
    //not super important to have in here
    [SerializeField] private IStamp _stampAbilityScript;

    [Header("Upgrade Menu")]
    [SerializeField] private UpgradeInfo[] _upgrades;
    [System.Serializable]
    public struct UpgradeInfo
    {
        [SerializeField] public string name;
        [SerializeField] [TextArea] public string descriptionBase;
        [SerializeField] [TextArea] public string descriptionRandom;
        [SerializeField] [TextArea] public string descriptionResetWarning;
        
    }
#endregion

#region SO Getters

    public GameObject SpawnedUnit
    {
        get {return _spawnedUnit;}
    }

    public string UnitType
    {
        get {return _unitType;}
    } 

    public float UnitAbilityCooldown
    {
        get {return _unitAbilityCooldown;}
    }

    public float UnitCooldownReduction
    {
        get {return _unitCooldownReduction;}
    }

    public float UnitDamage
    {
        get {return _unitDamage;}
    }

    public float UnitAttackSpeed
    {
        get {return _unitAttackSpeed;}
    }

    public float UnitSlowAmount
    {
        get {return _unitSlowAmount;}
    }
    
    public int UnitHealth
    {
        get {return _unitHealth;}
    }

    public IStamp StampAbilityScript
    {
        get {return _stampAbilityScript;}
    }

    public UpgradeInfo[] Upgrades
    {
        get {return _upgrades;}
    }
#endregion
}