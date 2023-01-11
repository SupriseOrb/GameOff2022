using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonry : PlayerUnit
{
    #region Cannonry Info
    private float _abilityCooldown;
    private float _cooldownReduction;
    private float _damage;
    #endregion

    #region Projectile Info
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _spawnLocation;
    private float _destroyTime;
    #endregion

    #region Properties
    /*
    =================
    Cannonry Info
    =================
    */
    protected float AbilityCooldown {get {return _abilityCooldown;} set {_abilityCooldown = value;}}
    protected float CooldownReduction {get {return _cooldownReduction;} set {_cooldownReduction = value;}}
    protected float Damage {get {return _damage;} set {_damage = value;}}
    /*
    =================
    Projectile Info
    =================
    */
    protected GameObject Prefab {get {return _prefab;}}
    protected Transform SpawnLocation {get {return _spawnLocation;}}
    #endregion

}
