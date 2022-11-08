using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStampScriptableObject", menuName = "GameOff2022/UnitStampSO", order = 0)]
public class UnitStampScriptableObject : ScriptableObject
{
#region SO Backing Fields
    [SerializeField] private string _stampName;
    [SerializeField] private int _stampCost;
    [SerializeField] private Sprite _stampImage;

    //Unit Stats
    [SerializeField] private GameObject _spawnedUnit;
    [SerializeField] private string _unitType;
    [SerializeField] private float _unitAbilityCooldown;
    [SerializeField] private float _unitCooldownReduction;
    [SerializeField] private float _unitDamage;
    [SerializeField] private float _unitAttackSpeed;
    [SerializeField] private float _unitSlowAmount;
    //not super important to have in here
    [SerializeField] private IStamp _stampAbilityScript;
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

    public Sprite StampImage
    {
        get {return _stampImage;}
    }

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

    public IStamp StampAbilityScript
    {
        get {return _stampAbilityScript;}
    }
#endregion
}