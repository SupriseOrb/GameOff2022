using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularSoldierEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private EnemyScriptableObject _soldierSO;
    [SerializeField] private bool _isAttacking = false;
    [SerializeField] private float _soliderAttackCooldown;

#region UnitStats
    [SerializeField] private GameObject _spawnedUnit;
    [SerializeField] private float _soldierHealth;
    [SerializeField] private float _soldierAbilityCooldown;
    [SerializeField] private float _soldierCooldownReduction;
    [SerializeField] private float _soldierDamage;
    [SerializeField] private float _soldierAttackSpeed;
    [SerializeField] private float _soldierMovementSpeed;
#endregion

    [SerializeField] private GameObject _attackTarget;
    [SerializeField] private bool _isStunned = false;
    [SerializeField] private Rigidbody2D _soldierRigidBody;

    [SerializeField] private float _currentStunDuration;
    [SerializeField] private int _laneNumber;

    private void Start() 
    {
        LoadBaseStats();
        _soldierRigidBody = GetComponent<Rigidbody2D>();    
        _soldierRigidBody.velocity = Vector2.left * _soldierMovementSpeed;
    }

    public void LoadBaseStats()
    {
        _spawnedUnit = _soldierSO.SpawnedUnit;
        _soldierHealth = _soldierSO.EnemyHealth;
        _soldierAbilityCooldown = _soldierSO.EnemyAbilityCooldown;
        _soldierCooldownReduction = _soldierSO.EnemyCooldownReduction;
        _soldierDamage = _soldierSO.EnemyDamage;
        _soldierAttackSpeed = _soldierSO.EnemyAttackSpeed;
        _soldierMovementSpeed = _soldierSO.EnemyMovementSpeed;
        _soliderAttackCooldown = 1 / _soldierAttackSpeed;
    }

    public void SetLane(int laneNumber)
    {
        _laneNumber = laneNumber;
    }

    public void TakeDamage(float damage)
    {
        _soldierHealth -= damage;
        if(_soldierHealth <= 0)
        {
            BoardManager.Instance.GetLane(_laneNumber).RemoveEnemyFromList(gameObject);
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
                _soldierRigidBody.velocity = Vector2.left * _soldierMovementSpeed;
            }
            if(_soliderAttackCooldown <= 0)
            {
                ActivateStampAttack();
                _soliderAttackCooldown = 1 / _soldierAttackSpeed;
            }
            _soliderAttackCooldown -= Time.deltaTime;
        }
    }

    public void GetAttackTarget(GameObject target)
    {
        _attackTarget = target;
        _isAttacking = true;
        _soldierRigidBody.velocity = Vector2.zero;
    }

    public void ActivateStampAttack()
    {
        if(_attackTarget != null)
        {
            _attackTarget.GetComponent<IItemStamp>().TakeDamage(_soldierDamage);
        }
        //Play attack animation
    }

    public void ModifyStat()
    {

    }

    public void Stun(float stunDuration)
    {
        _isAttacking = false;
        _isStunned = true;
        _soldierRigidBody.velocity = Vector3.zero;
        _currentStunDuration = stunDuration;
    }
}
