using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularSoldierEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private EnemyScriptableObject _soldierSO;
    [SerializeField] private bool _isAttacking = false;
    [SerializeField] private float _soldierAttackCooldown;

#region UnitStats
    [SerializeField] private GameObject _spawnedUnit;
    [SerializeField] private float _soldierHealth;
    [SerializeField] private float _soldierAbilityCooldown;
    [SerializeField] private float _soldierCooldownReduction;
    [SerializeField] private float _soldierDamage;
    [SerializeField] private float _soldierAttackSpeed;
    [SerializeField] private float _soldierBaseAttackSpeed;
    [SerializeField] private float _soldierMovementSpeed;
    [SerializeField] private float _soldierBaseMovementSpeed;
#endregion

    [SerializeField] private GameObject _attackTarget;
    [SerializeField] private bool _isStunned = false;
    [SerializeField] private bool _isMoveSlowed = false;
    [SerializeField] private bool _isAttackSlowed = false;
    [SerializeField] private bool _isDead = false;
    [SerializeField] private Rigidbody2D _soldierRigidBody;
    [SerializeField] private BoxCollider2D _soldierCollider;

    [SerializeField] private float _currentStunDuration;
    [SerializeField] private float _currentMoveSlowDuration;
    [SerializeField] private float _currentAttackSlowDuration;
    [SerializeField] private int _laneNumber;
    [SerializeField] private Animator _soldierAnimator;
    [SerializeField] private float _soldierAttackAnimationLength;
    [SerializeField] private float _soldierWalkAnimationLength;
    [SerializeField] private float _soldierDieAnimationLength;
    [SerializeField] private string _soldierAppearAnimationName = "Soldier_Pink_Appear";
    [SerializeField] private string _soldierAttackAnimationName = "Soldier_Pink_Attack";
    [SerializeField] private string _soldierWalkAnimationName = "Soldier_Pink_Walk";
    [SerializeField] private string _soldierDieAnimationName = "Soldier_Pink_Death";

    private void Awake() 
    {
        _soldierRigidBody = GetComponent<Rigidbody2D>();    
        _soldierCollider = GetComponent<BoxCollider2D>();
    }

    private void Start() 
    {
        LoadBaseStats();
        _isAttacking = true;
        
        foreach(AnimationClip clip in _soldierAnimator.runtimeAnimatorController.animationClips)
        {
            if(clip.name == _soldierAttackAnimationName)
            {
                _soldierAttackAnimationLength = clip.length;
            }
            else if(clip.name == _soldierWalkAnimationName)
            {
                _soldierWalkAnimationLength = clip.length;
            }
            else if(clip.name == _soldierDieAnimationName)
            {
                _soldierDieAnimationLength = clip.length;
            }
        }
        
        SetAnimationSpeeds();
        _soldierAnimator.Play(_soldierAppearAnimationName);
    }

    public void LoadBaseStats()
    {
        _spawnedUnit = _soldierSO.SpawnedUnit;
        _soldierHealth = _soldierSO.EnemyHealth;
        _soldierAbilityCooldown = _soldierSO.EnemyAbilityCooldown;
        _soldierCooldownReduction = _soldierSO.EnemyCooldownReduction;
        _soldierDamage = _soldierSO.EnemyDamage;
        _soldierBaseAttackSpeed = _soldierSO.EnemyAttackSpeed;
        _soldierAttackSpeed = _soldierBaseAttackSpeed;

        _soldierBaseMovementSpeed = _soldierSO.EnemyMovementSpeed;
        _soldierMovementSpeed = _soldierBaseMovementSpeed;

        _soldierAttackCooldown = 1 / _soldierAttackSpeed;
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
            _soldierAnimator.Play(_soldierDieAnimationName);
            _isDead = true;
            _soldierCollider.enabled = false;
            /*TODO:
                Disable enemy collider
                Play death animation
                Destroy after animation completes
            */
        }
    }

    private void FixedUpdate() 
    {
        if(_isMoveSlowed)
        {
            if(_currentMoveSlowDuration > 0)
            {
                _currentMoveSlowDuration -= Time.deltaTime;
            }
            else
            {
                _soldierMovementSpeed = _soldierBaseMovementSpeed;
                _isAttacking = true;
                SetAnimationSpeeds();
            }
        }
        if(_isAttackSlowed)
        {
            if(_currentAttackSlowDuration > 0)
            {
                _currentAttackSlowDuration -= Time.deltaTime;
            }
            else
            {
                _soldierAttackSpeed = _soldierBaseAttackSpeed;
                SetAnimationSpeeds();
            }
        }

        if(_isDead)
        {
            _soldierAnimator.speed = 1;
            _soldierDieAnimationLength -= Time.deltaTime;
            if(_soldierDieAnimationLength < 0)
            {
                Destroy(gameObject);
            }
        }
        else if (_isStunned)
        {
            _currentStunDuration -= Time.deltaTime;
            if(_currentStunDuration <= 0)
            {
                _soldierAnimator.speed = 1;
                _isStunned = false;
                _isAttacking = true;
            }
        }
        else if(_isAttacking)
        {
            if(_soldierAttackCooldown <= 0)
            {
                if(_attackTarget == null || _attackTarget.GetComponent<IItemStamp>().IsDead())
                {
                    _isAttacking = false;
                    _soldierRigidBody.velocity = Vector2.left * _soldierMovementSpeed;
                    _soldierAnimator.Play(_soldierWalkAnimationName);
                }
                else
                {
                    Debug.Log("Target is Dead = " + _attackTarget.GetComponent<IItemStamp>().IsDead());
                    ActivateStampAttack();
                    _soldierAttackCooldown = 1 / _soldierAttackSpeed;
                }
            }
            _soldierAttackCooldown -= Time.deltaTime;
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
            _soldierAnimator.Play(_soldierAttackAnimationName);
            _attackTarget.GetComponent<IItemStamp>().TakeDamage(_soldierDamage);
        }
        //Play attack animation
    }

    public void ModifySpeeds(float movementModifier, float moveDuration = 0, float attackSpeedModifier = 0, float attackDuration = 0)
    {
        _soldierMovementSpeed = _soldierMovementSpeed * movementModifier;
        _currentMoveSlowDuration = moveDuration;
        _isAttacking = true;
        if(moveDuration > 0)
        {
            _isMoveSlowed = true;
        }

        _soldierAttackSpeed = _soldierAttackSpeed * attackSpeedModifier;
        _currentAttackSlowDuration = attackDuration;
        if(attackDuration > 0)
        {
            _isAttackSlowed = true;
        }
        //_soldierRigidBody.velocity = Vector2.left * _soldierMovementSpeed;
        SetAnimationSpeeds();
    }

    private void SetAnimationSpeeds()
    {
        _soldierAnimator.SetFloat("WalkSpeedMultiplier", _soldierMovementSpeed / (1/_soldierWalkAnimationLength));
        _soldierAnimator.SetFloat("AttackSpeedMultiplier", _soldierAttackSpeed / (1/_soldierAttackAnimationLength));
    }

    public void Stun(float stunDuration)
    {
        _isAttacking = false;
        _isStunned = true;
        _soldierAnimator.speed = 0;
        _soldierRigidBody.velocity = Vector3.zero;
        _currentStunDuration = stunDuration;
    }
}
