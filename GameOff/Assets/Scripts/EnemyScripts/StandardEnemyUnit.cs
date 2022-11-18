using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnemyUnit : MonoBehaviour, IEnemy
{
    [SerializeField] private EnemyScriptableObject _enemySO;
    [SerializeField] private bool _isAttacking = false;
    [SerializeField] private float _enemyAttackCooldown;

#region UnitStats
    [SerializeField] private GameObject _spawnedUnit;
    [SerializeField] private float _enemyHealth;
    [SerializeField] private float _enemyAbilityCooldown;
    [SerializeField] private float _enemyCooldownReduction;
    [SerializeField] private float _enemyDamage;
    [SerializeField] private float _enemyAttackSpeed;
    [SerializeField] private float _enemyMovementSpeed;
#endregion

    [SerializeField] private GameObject _attackTarget;
    [SerializeField] private bool _isStunned = false;
    [SerializeField] private Rigidbody2D _enemyRigidBody;

    [SerializeField] private float _currentStunDuration;

    private void Start() 
    {
        LoadBaseStats();
        _enemyRigidBody = GetComponent<Rigidbody2D>();    
        _enemyRigidBody.velocity = Vector2.left * _enemyMovementSpeed;
    }

    public void LoadBaseStats()
    {
        _spawnedUnit = _enemySO.SpawnedUnit;
        _enemyHealth = _enemySO.EnemyHealth;
        _enemyAbilityCooldown = _enemySO.EnemyAbilityCooldown;
        _enemyCooldownReduction = _enemySO.EnemyCooldownReduction;
        _enemyDamage = _enemySO.EnemyDamage;
        _enemyAttackSpeed = _enemySO.EnemyAttackSpeed;
        _enemyMovementSpeed = _enemySO.EnemyMovementSpeed;
        _enemyAttackCooldown = 1 / _enemyAttackSpeed;
    }

    public void TakeDamage(float damage)
    {
        _enemyHealth -= damage;
        if(_enemyHealth <= 0)
        {
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
                _enemyRigidBody.velocity = Vector2.left * _enemyMovementSpeed;
            }
            if(_enemyAttackCooldown <= 0)
            {
                ActivateStampAttack();
                _enemyAttackCooldown = 1 / _enemyAttackSpeed;
            }
            _enemyAttackCooldown -= Time.deltaTime;
        }
    }

    public void GetAttackTarget(GameObject target)
    {
        _attackTarget = target;
        _isAttacking = true;
        _enemyRigidBody.velocity = Vector2.zero;
    }

    public void ActivateStampAttack()
    {
        _attackTarget.GetComponent<IItemStamp>().TakeDamage(_enemyDamage);
        //Play attack animation
    }

    public void ModifyStat()
    {

    }

    public void Stun(float stunDuration)
    {
        _isAttacking = false;
        _isStunned = true;
        _enemyRigidBody.velocity = Vector3.zero;
        _currentStunDuration = stunDuration;
    }
}
