using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : PlayerUnit
{
    #region Summoner Info
    private float _cooldownReduction;
    // TODO : Don't make this a generic. Create a parent class for summons instead.
    //private List<Summons> _activeMinions; 
    #endregion

    #region Summons Info
    [SerializeField] private GameObject _prefab;
    private float _health;
    private float _damage;
    private float _abilityCooldown;
    private float _damageMultiplier;
    private float _slowAmount;
    private float _slowDuration;
    #endregion

    #region Properties
    /*
    =================
    Summoner Stats
    =================
    */
    protected float CooldownReduction {get {return _cooldownReduction;} set {_cooldownReduction = value;}}
    //protected List<Summons> ActiveMinions {get {return _activeMinions;} set {_activeMinions = value;}}
    /*
    =================
    Summons Info
    =================
    */
    protected GameObject Prefab {get {return _prefab;}}
    protected float Health {get {return _health;} set {_health = value;}}
    protected float Damage {get {return _damage;} set {_damage = value;}}
    protected float AbilityCooldown {get {return _abilityCooldown;} set {_abilityCooldown = value;}}
    protected float DamageMultiplier {get {return _damageMultiplier;} set {_damageMultiplier = value;}}
    protected float SlowAmount {get {return _slowAmount;} set {_slowAmount = value;}}
    protected float SlowDuration {get {return _slowDuration;} set {_slowDuration = value;}}
    #endregion

}
