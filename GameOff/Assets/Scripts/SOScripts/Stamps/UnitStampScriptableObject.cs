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
    [SerializeField] private float _unitSlowDuration;
    [SerializeField] private int _unitHealth;
    //not super important to have in here
    [SerializeField] private IStamp _stampAbilityScript;

    [SerializeField] private float _attackDamageIncreaseAmount;
    [SerializeField] private float _attackSpeedIncreaseAmount;
    [Tooltip("Redcap: Pierce Amount (rng), Ink Demon: Max HP (for Minions; rng), Harpy: Slow Intensity (rng)")]
    [SerializeField] private float _uniqueUpgradeOneIncreaseAmount;
    [Tooltip("Redcap: Stun Duration (rng), Ink Demon: CD Increase (Debuff; 1st Upgrade), Harpy: Push Distance (rng)")]
    [SerializeField] private float _uniqueUpgradeTwoIncreaseAmount;
    [Tooltip("Redcap: Stun Duration (Buff; 2nd Upgrade), Ink Demon: CD Decrease (Buff; 2nd Upgrade), Harpy: Attack Speed Decrease (Debuff; 1st Upgrade)")]
    [SerializeField] private float _uniqueUpgradeThreeIncreaseAmount;

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

    public float UnitSlowDuration
    {
        get {return _unitSlowDuration;}
    }
    
    public int UnitHealth
    {
        get {return _unitHealth;}
    }

    public IStamp StampAbilityScript
    {
        get {return _stampAbilityScript;}
    }

    public float AttackDamageIncreaseAmount
    {
        get {return _attackDamageIncreaseAmount;}
    }

    public float AttackSpeedIncreaseAmount
    {
        get {return _attackSpeedIncreaseAmount;}
    }

    public float UniqueUpgradeOneIncreaseAmount
    {
        get {return _uniqueUpgradeOneIncreaseAmount;}
    }

    public float UniqueUpgradeTwoIncreaseAmount
    {
        get {return _uniqueUpgradeTwoIncreaseAmount;}
    }

    public float UniqueUpgradeThreeIncreaseAmount
    {
        get {return _uniqueUpgradeThreeIncreaseAmount;}
    }

    public UpgradeInfo[] Upgrades
    {
        get {return _upgrades;}
    }
#endregion
}