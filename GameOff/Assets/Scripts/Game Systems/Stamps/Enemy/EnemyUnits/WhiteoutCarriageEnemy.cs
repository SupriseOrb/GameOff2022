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
    [SerializeField] private float _carriageDamage;
    [SerializeField] private float _carriageAttackSpeed;
    [SerializeField] private float _carriageMovementSpeed;
    [SerializeField] private float _carriageBaseMovementSpeed;
    [SerializeField] private int _playerHealthDamage;
#endregion

#region Lerping Variables
    private Vector3 _startingPosition;
    private Vector3 _endingPosition;
    private float _moveDistance;
    private float _startTime;
    private float _forcedMoveSpeed;
    private float _distanceCheck = 0.02f;
#endregion

    [SerializeField] private GameObject _attackTarget;
    [SerializeField] private bool _isStunned = false;
    [SerializeField] private bool _isMoveSlowed = false;
    [SerializeField] private bool _isDead = false;
    [SerializeField] private bool _isLerping = false;
    [SerializeField] private Rigidbody2D _carriageRigidBody;
    [SerializeField] private BoxCollider2D _carriageCollider;

    [SerializeField] private float _currentStunDuration;
    [SerializeField] private float _currentMoveSlowDuration;
    [SerializeField] private float _currentSlowMultiplier;
    [SerializeField] private int _laneNumber;
    [SerializeField] private Animator _carriageAnimator;
    [SerializeField] private float _carriageWalkAnimationLength;
    [SerializeField] private float _carriageDieAnimationLength;
    [SerializeField] private float _carriageDieAnimationHalfLength;
    [SerializeField] private string _carriageWalkAnimationName = "Carriage_Walk";
    [SerializeField] private string _carriageDieAnimationName = "Carriage_Disappear";

    [SerializeField] private SpriteRenderer _carriageSpriteRenderer;
    [SerializeField] private Vector2Int _spriteLayerRange;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _damageFlashMaterial;
    [SerializeField] private float _flashTime = .125f;
    [SerializeField] private Coroutine _damageFlashCoroutine = null;

    private void Awake() 
    {
        _carriageRigidBody = GetComponent<Rigidbody2D>();    
        _carriageCollider = GetComponent<BoxCollider2D>();
    }

    private void Start() 
    {
        _carriageSpriteRenderer.sortingOrder = Random.Range(_spriteLayerRange.x, _spriteLayerRange.y + 1);
        AkSoundEngine.PostEvent("Play_CarriageEntrance", gameObject);
        AkSoundEngine.PostEvent("Play_CarriageMovement", gameObject);
        LoadBaseStats();
        _isAttacking = true;
        
        foreach(AnimationClip clip in _carriageAnimator.runtimeAnimatorController.animationClips)
        {
            if(clip.name == _carriageWalkAnimationName)
            {
                _carriageWalkAnimationLength = clip.length;
            }
            else if(clip.name == _carriageDieAnimationName)
            {
                _carriageDieAnimationLength = clip.length;
            }
        }
        _carriageDieAnimationHalfLength = _carriageDieAnimationLength/2;
        
        SetAnimationSpeeds();
    }

    public void LoadBaseStats()
    {
        _spawnedUnit = _carriageSO.SpawnedUnit;
        _carriageHealth = _carriageSO.EnemyHealth;
        _carriageDamage = _carriageSO.EnemyDamage;
        _carriageAttackSpeed = _carriageSO.EnemyAttackSpeed;
        _carriageBaseMovementSpeed = _carriageSO.EnemyMovementSpeed;
        _carriageMovementSpeed = _carriageBaseMovementSpeed;
        _carriageAttackCooldown = 1 / _carriageAttackSpeed;

        _playerHealthDamage = _carriageSO.PlayerHealthDamage;
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
        _carriageHealth -= damage;
        if(_carriageHealth <= 0)
        {
            /*
            If we want the Whiteout Carriage to summon Regular Soliders:
            *IMPORTANT*: We also need to add them to their respective Lane's _laneEnemies
            Instantiate(_soliderPrefab,gameObject.transform.position, Quaternion.identity);
            */


            if(!_isDead)
            {
                AkSoundEngine.PostEvent("Play_DeathAnimation", gameObject);
                AkSoundEngine.PostEvent("Play_BossDeath", gameObject);
                AkSoundEngine.PostEvent("Stop_CarriageMovement",gameObject);
                _carriageAnimator.Play(_carriageDieAnimationName);
                _isDead = true;
                _carriageCollider.enabled = false;
            }
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
                _carriageMovementSpeed = _carriageMovementSpeed / _currentSlowMultiplier;
                _isMoveSlowed = false;
                _isAttacking = true;
                SetAnimationSpeeds();
            }
        }

        if (_isLerping)
        {
            float timePassed = Time.time - _startTime;
            float distanceMoved = timePassed * _forcedMoveSpeed;
            float movementPercentage = distanceMoved / _moveDistance;
            float distanceRemaining = Mathf.Abs(Vector3.Distance(transform.position, _endingPosition));
            if (timePassed > 1.2f)
            {
                _isLerping = false;
            }
            //Check done this way to prevent floating point inaccuracy
            else if (_startingPosition.y == _endingPosition.y && distanceRemaining > _distanceCheck)
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
                _carriageAnimator.speed = 1;
                _carriageCollider.enabled = true;
                _isLerping = false;
            }    
        }

        if(_isDead)
        {
            _carriageAnimator.speed = 1;
            _carriageDieAnimationLength -= Time.deltaTime;
            if(_carriageDieAnimationLength < 0)
            {
                Destroy(gameObject);
            } 
            else if(_carriageDieAnimationLength < _carriageDieAnimationHalfLength)
            {
                GameObject spawnedEnemy = Instantiate(_spawnedUnit, gameObject.transform.position, Quaternion.identity);
                BoardManager.Instance.GetLane(_laneNumber).AddEnemyToList(spawnedEnemy);
                BoardManager.Instance.GetLane(_laneNumber).RemoveEnemyFromList(gameObject);
                _carriageDieAnimationHalfLength = -100;
            }
        }
        else if(_isStunned)
        {
            _currentStunDuration -= Time.deltaTime;
            if (_currentStunDuration <= 0)
            {
                _carriageAnimator.speed = 1;
                _isStunned = false;
                _isAttacking = true;
            }
        }
        else if(_isAttacking)
        {
            if (_carriageAttackCooldown <= 0)
            {
                if(_attackTarget == null || _attackTarget.GetComponent<IItemStamp>().IsDead())
                {
                    _isAttacking = false;
                    _carriageRigidBody.velocity = Vector2.left * _carriageMovementSpeed;
                }
                else
                {
                    ActivateStampAttack();
                    _carriageAttackCooldown = 1 / _carriageAttackSpeed;
                }
            }
            _carriageAttackCooldown -= Time.deltaTime;
        }
    }

    public void GetAttackTarget(GameObject target)
    {
        _attackTarget = target;
        _isAttacking = true;
    }

    public void ActivateStampAttack()
    {
        _attackTarget.GetComponent<IItemStamp>().TakeDamage(_carriageDamage);
        //Play attack animation
    }

    public void ReduceSpeeds(float movementModifier, float moveDuration = 0, float attackSpeedModifier = 0, float attackDuration = 0)
    {
        if(movementModifier > 0)
        {
            if(_carriageMovementSpeed > _carriageBaseMovementSpeed * movementModifier)
            {
                _carriageMovementSpeed = _carriageBaseMovementSpeed * movementModifier;
                _isAttacking = true;
                if(moveDuration > 0)
                {
                    _currentMoveSlowDuration = moveDuration;
                    _currentSlowMultiplier = movementModifier;
                    _isMoveSlowed = true;
                }
            }
        }
        
        SetAnimationSpeeds();
    }

    public void IncreaseSpeeds(float movementModifier, float attackSpeedModifier)
    {
        if(movementModifier > 0)
        {
            _carriageMovementSpeed = _carriageMovementSpeed * movementModifier;
            _isAttacking = true;
        }
        //_soldierRigidBody.velocity = Vector2.left * _soldierMovementSpeed;
        SetAnimationSpeeds();
        Debug.Log(_carriageMovementSpeed);
    }

    private void SetAnimationSpeeds()
    {
        _carriageAnimator.SetFloat("WalkSpeedModifier", _carriageMovementSpeed / (1/_carriageWalkAnimationLength));
    }

    public void Stun(float stunDuration)
    {
        _isAttacking = false;
        _isStunned = true;
        _carriageAnimator.speed = 0;
        _carriageRigidBody.velocity = Vector3.zero;
        _currentStunDuration = stunDuration;
    }

    public int GetPlayerHealthDamage()
    {
        return _playerHealthDamage;
    }
    public void ForcedMove(Vector3 startPos, Vector3 endPos, float forcedMoveSpeed)
    {
        _startingPosition = startPos;
        if (endPos.x > 12f) 
        {
            if (transform.position.x < 12.25)
            {
                _endingPosition = new Vector3(12.25f, endPos.y, endPos.z);
            }
            else
            {
                return; //Don't push back if they're not visible on the board
            }
        }
        else
        {
            _endingPosition = endPos;
        }
        
        _carriageAnimator.speed = 0;
        _isAttacking = false;
        _attackTarget = null;
        _carriageRigidBody.velocity = Vector2.zero;
        _carriageCollider.enabled = false;
        _forcedMoveSpeed = forcedMoveSpeed;
        _moveDistance = Vector3.Distance(_startingPosition, _endingPosition);
        _startTime = Time.time;
        _isLerping = true;
    }

    private IEnumerator DamageFlashCoroutine()
    {
        _carriageSpriteRenderer.material = _damageFlashMaterial;

        yield return new WaitForSeconds(_flashTime);
        _carriageSpriteRenderer.material = _defaultMaterial;
        _damageFlashCoroutine = null;
    }
}
