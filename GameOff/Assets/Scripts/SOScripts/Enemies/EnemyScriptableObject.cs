using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "GameOff2022/Enemies/EnemySO")]
public class EnemyScriptableObject : ScriptableObject
{
#region SO Backing Fields
    //Enemy Stats
    [SerializeField] private GameObject _spawnedUnit;
    [SerializeField] private float _enemyHealth;
    [SerializeField] private float _enemyAbilityCooldown;
    [SerializeField] private float _enemyCooldownReduction;
    [SerializeField] private float _enemyDamage;
    [SerializeField] private float _enemyAttackSpeed;
    [SerializeField] private float _enemyMovementSpeed;
    [SerializeField] private int _playerHealthDamage = 1;

#endregion

#region SO Getters

    public GameObject SpawnedUnit
    {
        get {return _spawnedUnit;}
    }

    public float EnemyHealth
    {
        get {return _enemyHealth;}
    }

    public float EnemyAbilityCooldown
    {
        get {return _enemyAbilityCooldown;}
    }

    public float EnemyCooldownReduction
    {
        get {return _enemyCooldownReduction;}
    }

    public float EnemyDamage
    {
        get {return _enemyDamage;}
    }

    public float EnemyAttackSpeed
    {
        get {return _enemyAttackSpeed;}
    }

    public float EnemyMovementSpeed
    {
        get {return _enemyMovementSpeed;}
    }

    public int PlayerHealthDamage
    {
        get {return _playerHealthDamage;}
    }
#endregion
}
