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
    [SerializeField] private int _playerHealthDamage;
#endregion

#region Lerping Variables
    [Header("Lerping Variables")]
    [SerializeField] private Vector3 _startingPosition;
    [SerializeField] private Vector3 _endingPosition;
    [SerializeField] private float _moveDistance;
    [SerializeField] private float _startTime;
    [SerializeField] private float _forcedMoveSpeed;
    [SerializeField] private float _distanceCheck = 0.01f;
#endregion

    [SerializeField] private GameObject _attackTarget;
    [SerializeField] private bool _isStunned = false;
    [SerializeField] private bool _isMoveSlowed = false;
    [SerializeField] private bool _isAttackSlowed = false;
    [SerializeField] private bool _isDead = false;
    [SerializeField] private bool _isLerping = false;
    private bool _hasDamaged = true;
    [SerializeField] private Rigidbody2D _soldierRigidBody;
    [SerializeField] private BoxCollider2D _soldierCollider;

    [SerializeField] private float _currentStunDuration;
    [SerializeField] private float _currentMoveSlowDuration;
    [SerializeField] private float _currentAttackSlowDuration;
    [SerializeField] private float _currentSlowMultiplier;
    [SerializeField] private int _laneNumber;
    [SerializeField] private Animator _soldierAnimator;
    [SerializeField] private float _soldierAttackAnimationLength;
    [SerializeField] private float _soldierWalkAnimationLength;
    [SerializeField] private float _soldierDieAnimationLength;
    [SerializeField] private string _soldierAppearAnimationName = "Soldier_Pink_Appear";
    [SerializeField] private string _soldierAttackAnimationName = "Soldier_Pink_Attack";
    [SerializeField] private string _soldierWalkAnimationName = "Soldier_Pink_Walk";
    [SerializeField] private string _soldierDieAnimationName = "Soldier_Pink_Death";

    [SerializeField] private SpriteRenderer _soldierSpriteRenderer;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _damageFlashMaterial;
    [SerializeField] private float _flashTime = .125f;
    [SerializeField] private Coroutine _damageFlashCoroutine = null;

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

        _playerHealthDamage = _soldierSO.PlayerHealthDamage;
    }

    public void SetLane(int laneNumber)
    {
        _laneNumber = laneNumber;
    }
    
    public int GetLane()
    {
        return _laneNumber;
    }

    public void TakeDamage(float damage)
    {
        _soldierHealth -= damage;
        if(_soldierHealth <= 0)
        {
            AkSoundEngine.PostEvent("Play_DeathAnimation", gameObject);
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
        else
        {
            if(_damageFlashCoroutine != null)
            {
                StopCoroutine(_damageFlashCoroutine);
            }
            
            _damageFlashCoroutine = StartCoroutine(DamageFlashCoroutine());
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
                _soldierMovementSpeed = _soldierMovementSpeed / _currentSlowMultiplier;
                _isMoveSlowed = false;
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

        if (_isLerping)
        {
            float distanceMoved = (Time.time - _startTime) * _forcedMoveSpeed;
            float movementPercentage = distanceMoved / _moveDistance;
            float distanceRemaining = Mathf.Abs(Vector3.Distance(transform.position, _endingPosition));
            //Check done this way to prevent floating point inaccuracy
            if (_startingPosition.y == _endingPosition.y && distanceRemaining > _distanceCheck)
            {
                transform.position = Vector3.Lerp(_startingPosition, _endingPosition, movementPercentage);
            }
            //This is here because sometimes while swapping lanes, the x value changes causing distance to not go to 0 or below;
            else if(_startingPosition.y != _endingPosition.y && transform.position.y - _endingPosition.y != 0)
            {
                transform.position = Vector3.Lerp(_startingPosition, _endingPosition, movementPercentage);
            }
            else
            {
                _isAttacking = true;
                _soldierAnimator.speed = 1;
                _soldierCollider.enabled = true;
                _isLerping = false;
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
            else if(!_hasDamaged && _soldierAttackCooldown <= (1 / _soldierAttackSpeed) * .4)
            {
                ActivateStampDamage();
            }
            _soldierAttackCooldown -= Time.deltaTime;
        }
        
    }

    public void GetAttackTarget(GameObject target)
    {
        if(!_isLerping)
        {
            _attackTarget = target;
            _isAttacking = true;
            _soldierRigidBody.velocity = Vector2.zero;
        }
    }

    public void ActivateStampAttack()
    {
        if(_attackTarget != null)
        {
            _hasDamaged = false;
            _soldierAnimator.Play(_soldierAttackAnimationName);
            // COLLIN TODO: Soldier attack sfx
            // Opted to not make a soldier attack sfx, rather a ally take damage sound - Collin
        }
        //Play attack animation
    }

    public void ActivateStampDamage()
    {
        if(_attackTarget != null)
        {
            _hasDamaged = true;
            _attackTarget.GetComponent<IItemStamp>().TakeDamage(_soldierDamage);
        }
    }

    public void ReduceSpeeds(float movementModifier, float moveDuration = 0, float attackSpeedModifier = 0, float attackDuration = 0)
    {
        if(movementModifier > 0)
        {
            if(_soldierMovementSpeed > _soldierBaseMovementSpeed * movementModifier)
            {
                _soldierMovementSpeed = _soldierBaseMovementSpeed * movementModifier;
                _isAttacking = true;
                if(moveDuration > 0)
                {
                    _currentMoveSlowDuration = moveDuration;
                    _currentSlowMultiplier = movementModifier;
                    _isMoveSlowed = true;
                }
            }
        }

        if(attackSpeedModifier > 0)
        {
            _soldierAttackSpeed = _soldierAttackSpeed * attackSpeedModifier;
            if(attackDuration > 0)
            {
                _currentAttackSlowDuration = attackDuration;
                _isAttackSlowed = true;
            }
        }
        //_soldierRigidBody.velocity = Vector2.left * _soldierMovementSpeed;
        SetAnimationSpeeds();
    }

    public void IncreaseSpeeds(float movementModifier, float attackSpeedModifier)
    {
        if(movementModifier > 0)
        {
            _soldierMovementSpeed = _soldierMovementSpeed * movementModifier;
            _isAttacking = true;
        }

        if(attackSpeedModifier > 0)
        {
            _soldierAttackSpeed = _soldierAttackSpeed * attackSpeedModifier;
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

    public int GetPlayerHealthDamage()
    {
        return _playerHealthDamage;
    }
    public void ForcedMove(Vector3 startPos, Vector3 endPos, float forcedMoveSpeed)
    {
        _soldierAnimator.speed = 0;
        _isAttacking = false;
        _attackTarget = null;
        _soldierRigidBody.velocity = Vector2.zero;
        _soldierCollider.enabled = false;

        _startingPosition = startPos;
        //checking to see if the endingPosition would go past the end of the board (12.25f)
        //Their current spawn value is 13.87f
        if (endPos.x > 12.25f) 
        {
            _endingPosition = new Vector3(12.25f, endPos.y, endPos.z);
        }
        else
        {
            _endingPosition = endPos;
        }
        _forcedMoveSpeed = forcedMoveSpeed;
        _moveDistance = Vector3.Distance(_startingPosition, _endingPosition);
        _startTime = Time.time;
        _isLerping = true;
    }

    private IEnumerator DamageFlashCoroutine()
    {
        _soldierSpriteRenderer.material = _damageFlashMaterial;

        yield return new WaitForSeconds(_flashTime);
        _soldierSpriteRenderer.material = _defaultMaterial;
        _damageFlashCoroutine = null;
    }
}
