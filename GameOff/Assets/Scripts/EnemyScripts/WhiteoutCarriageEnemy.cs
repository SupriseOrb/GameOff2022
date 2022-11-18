using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteoutCarriageEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private EnemyScriptableObject _carriageSO;
    [SerializeField] private bool _isAttacking = false;
    [SerializeField] private float _carriageAttackCooldown;

#region UnitStats
    [SerializeField] private GameObject _spawnedUnit; //Regular Solider prefab
    [SerializeField] private float _carriageHealth;
    [SerializeField] private float _carriageAbilityCooldown;
    [SerializeField] private float _carriageCooldownReduction;
    [SerializeField] private float _carriageDamage;
    [SerializeField] private float _carriageAttackSpeed;
    [SerializeField] private float _carriageMovementSpeed;
#endregion

    [SerializeField] private GameObject _attackTarget;
    [SerializeField] private bool _isStunned = false;
    [SerializeField] private Rigidbody2D _carriageRigidBody;

    [SerializeField] private float _currentStunDuration;

    private void Start() 
    {
        LoadBaseStats();
        _carriageRigidBody = GetComponent<Rigidbody2D>();    
        _carriageRigidBody.velocity = Vector2.left * _carriageMovementSpeed;
    }

    public void LoadBaseStats()
    {
        _spawnedUnit = _carriageSO.SpawnedUnit;
        _carriageHealth = _carriageSO.EnemyHealth;
        _carriageAbilityCooldown = _carriageSO.EnemyAbilityCooldown;
        _carriageCooldownReduction = _carriageSO.EnemyCooldownReduction;
        _carriageDamage = _carriageSO.EnemyDamage;
        _carriageAttackSpeed = _carriageSO.EnemyAttackSpeed;
        _carriageMovementSpeed = _carriageSO.EnemyMovementSpeed;
        _carriageAttackCooldown = 1 / _carriageAttackSpeed;
    }

    public void TakeDamage(float damage)
    {
        _carriageHealth -= damage;
        if(_carriageHealth <= 0)
        {
            /*
            If we want the Whiteout Carriage to summon Regular Soliders:
            Instantiate(_soliderPrefab,gameObject.transform.position, Quaternion.identity);
            */

            //Temp Destroy
            Destroy(gameObject);

            /*TODO:
                Disable enemy collider
                Play death animation
                Destroy after animation completes
            */
        }
    }

    private void FixedUpdate() 
    {
        if (_isStunned)
        {
            _currentStunDuration -= Time.deltaTime;
            if (_currentStunDuration <= 0)
            {
                _isStunned = false;
                _isAttacking = true;
            }
        }

        if(_isAttacking)
        {
            if(_attackTarget == null)
            {
                _isAttacking = false;
                _carriageRigidBody.velocity = Vector2.left * _carriageMovementSpeed;
            }
            if(_carriageAttackCooldown <= 0)
            {
                ActivateStampAttack();
                _carriageAttackCooldown = 1 / _carriageAttackSpeed;
            }
            _carriageAttackCooldown -= Time.deltaTime;
        }
    }

    public void GetAttackTarget(GameObject target)
    {
        _attackTarget = target;
        _isAttacking = true;
        _carriageRigidBody.velocity = Vector2.zero;
    }

    public void ActivateStampAttack()
    {
        _attackTarget.GetComponent<IItemStamp>().TakeDamage(_carriageDamage);
        //Play attack animation
    }

    public void ModifyStat()
    {

    }

    public void Stun(float stunDuration)
    {
        _isAttacking = false;
        _isStunned = true;
        _carriageRigidBody.velocity = Vector3.zero;
        _currentStunDuration = stunDuration;
    }
}