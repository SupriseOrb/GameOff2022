using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkDemonMinion : MonoBehaviour, IItemStamp
{
    [SerializeField] private bool _isActive = false;

#region ItemStats
    [SerializeField] private int _inkMinionMaxHealth;
    [SerializeField] private float _inkMinionCurrentHealth;
    [SerializeField] private float _inkMinionAttackSpeed;
    [SerializeField] private float _inkMinionAttackDamage;
    [SerializeField] private int _inkMinionDeathDamageMultiplier;
    [SerializeField] private float _inkMinionSlowAmount;
    [SerializeField] private float _inkMinionSlowDuration;
#endregion
    [SerializeField] private float _inkMinionAttackCooldown;
    [SerializeField] private float _inkMinionAttackRange = 2;
    [SerializeField] private Collider2D[] _inkMinionHitColliders;
    [SerializeField] private Animator _inkMinionAnimator;
    [SerializeField] private string _inkMinionGreenAttackAnimationName = "InkDemon_Green_Attack";
    [SerializeField] private string _inkMinionGreenAppearAnimationName = "InkDemon_Green_Appear";
    [SerializeField] private string _inkMinionGreenDieAnimationName = "InkDemon_Green_Death";
    [SerializeField] private string _inkMinionBlueAttackAnimationName = "InkDemon_Blue_Attack";
    [SerializeField] private string _inkMinionBlueAppearAnimationName = "InkDemon_Blue_Appear";
    [SerializeField] private string _inkMinionBlueDieAnimationName = "InkDemon_Blue_Death";
    [SerializeField] private string _inkMinionRedAttackAnimationName = "InkDemon_Red_Attack";
    [SerializeField] private string _inkMinionRedAppearAnimationName = "InkDemon_Red_Appear";
    [SerializeField] private string _inkMinionRedDieAnimationName = "InkDemon_Red_Death";
    [SerializeField] private float _inkMinionAttackAnimationLength;
    [SerializeField] private float _inkMinionDeathAnimationLength;
    
    //Might be needed idk
    [SerializeField] private int _inkMinionLaneNumber;
    [SerializeField] private string _inkMinionName = "Ink Demon Minion";
    [SerializeField] private InkDemonUnitScript.InkDemonUpgradePaths _upgradePath;
    [SerializeField] private bool _isDead = false;
    [SerializeField] private BoxCollider2D _inkMinionCollider;

    // Start is called before the first frame update
    void Start()
    {
        foreach(AnimationClip clip in _inkMinionAnimator.runtimeAnimatorController.animationClips)
        {
            if(clip.name == _inkMinionGreenAttackAnimationName)
            {
                _inkMinionAttackAnimationLength = clip.length;
            }
            else if(clip.name == _inkMinionGreenDieAnimationName)
            {
                _inkMinionDeathAnimationLength = clip.length;
            }
        }
        _inkMinionCollider = GetComponent<BoxCollider2D>();
    }

    public void UpdateMinionStats
    (
        InkDemonUnitScript.InkDemonUpgradePaths upgradePath,
        int health = 0, 
        float attackSpeed = 0, 
        float attackDamage = 0, 
        int deathMultiplier = 0, 
        float slowAmount = 0, 
        float slowDuration = 0,
        int laneNumber = 0
    )
    {
        _upgradePath = upgradePath;
        health = health - _inkMinionMaxHealth;
        _inkMinionMaxHealth += health;
        _inkMinionCurrentHealth += health;
        _inkMinionAttackSpeed = attackSpeed;
        _inkMinionAttackDamage = attackDamage;
        _inkMinionDeathDamageMultiplier = deathMultiplier;
        _inkMinionSlowAmount = slowAmount;
        _inkMinionSlowDuration = slowDuration;

        _inkMinionLaneNumber = laneNumber;
        _inkMinionAttackCooldown = 1 / _inkMinionAttackSpeed;

        switch (_upgradePath)
        {
            case InkDemonUnitScript.InkDemonUpgradePaths.upgradeBase:
                _inkMinionAnimator.Play(_inkMinionGreenAppearAnimationName);
                break;
            case InkDemonUnitScript.InkDemonUpgradePaths.upgradeOne:
                _inkMinionAnimator.Play(_inkMinionBlueAppearAnimationName);
                break;        
            default:
                _inkMinionAnimator.Play(_inkMinionRedAppearAnimationName);
                break;
        }

        if(_inkMinionAttackSpeed > 1/_inkMinionAttackAnimationLength)
        {
            _inkMinionAnimator.SetFloat("AttackSpeedMultiplier", _inkMinionAttackSpeed / (1/_inkMinionAttackAnimationLength));
        }
    }        

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_isDead)
        {
            _inkMinionDeathAnimationLength -= Time.deltaTime;
            if(_inkMinionDeathAnimationLength < 0)
            {
                Destroy(gameObject);
            }
        }
        else if(_isActive)
        {
            if(_inkMinionAttackCooldown <= 0)
            {
                ActivateStampAttack();
                _inkMinionAttackCooldown = 1 / _inkMinionAttackSpeed;
            }
            _inkMinionAttackCooldown -= Time.deltaTime;
        }
    }

    public void ActivateStampAbility()
    {
        if(_inkMinionDeathDamageMultiplier != 0)
        {
            //Find nearby enemies
            //Deal damage = _inkMinionAttackDamage * _inkMinionDeathDamageMultiplier + _inkMinionMaxHealth
            //Apply a _inkMinionSlowAmount slow for _inkMinionSlowDuration seconds
        }
    }

    public void ActivateStampAttack()
    {
        //Debug to test ink demon attack range
        Vector3 DebugLeft = new Vector3(gameObject.transform.position.x - _inkMinionAttackRange, gameObject.transform.position.y, gameObject.transform.position.z);
        Vector3 DebugRight = new Vector3(gameObject.transform.position.x + _inkMinionAttackRange, gameObject.transform.position.y, gameObject.transform.position.z);
        Vector3 DebugUp = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + _inkMinionAttackRange, gameObject.transform.position.z);
        Vector3 DebugDown = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - _inkMinionAttackRange, gameObject.transform.position.z);
        Debug.DrawLine(DebugLeft, DebugRight, Color.cyan, 2f);
        Debug.DrawLine(DebugUp, DebugDown, Color.cyan, 2f);
        //Actual damage logic
        _inkMinionHitColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, _inkMinionAttackRange);
        foreach(Collider2D collider in _inkMinionHitColliders)
        {
            if(collider.transform.gameObject.TryGetComponent(out IEnemy enemyScript))
            {
                AkSoundEngine.PostEvent("Play_EnemyTakeDamage", gameObject);
                enemyScript.TakeDamage(_inkMinionAttackDamage);
            }
        }
        switch (_upgradePath)
        {
            case InkDemonUnitScript.InkDemonUpgradePaths.upgradeBase:
                _inkMinionAnimator.Play(_inkMinionGreenAttackAnimationName);
                break;
            case InkDemonUnitScript.InkDemonUpgradePaths.upgradeOne:
                _inkMinionAnimator.Play(_inkMinionBlueAttackAnimationName);
                break;        
            default:
                _inkMinionAnimator.Play(_inkMinionRedAttackAnimationName);
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        _inkMinionCurrentHealth -= damage;
        if(_inkMinionCurrentHealth <= 0)
        {
            AkSoundEngine.PostEvent("Play_DeathAnimation", gameObject);
            ActivateStampAbility();
            switch (_upgradePath)
            {
                case InkDemonUnitScript.InkDemonUpgradePaths.upgradeBase:
                    _inkMinionAnimator.Play(_inkMinionGreenDieAnimationName);
                    break;
                case InkDemonUnitScript.InkDemonUpgradePaths.upgradeOne:
                    _inkMinionAnimator.Play(_inkMinionBlueDieAnimationName);
                    break;        
                default:
                    _inkMinionAnimator.Play(_inkMinionRedDieAnimationName);
                    break;
            }
            _isDead = true;
            _inkMinionCollider.enabled = false;
            //Destroy(gameObject);

            /*TODO:
                Disable minion collider
                Play death animation
                Destroy after animation completes
            */
        }
    }

    public void HealHealth(float health)
    {
        _inkMinionCurrentHealth += health;
        if(_inkMinionCurrentHealth > _inkMinionMaxHealth)
        {
            _inkMinionCurrentHealth = _inkMinionMaxHealth;
        }
    }    

    public bool IsDead()
    {
        return _isDead;
    }

    public void SetLane(int laneNumber)
    {
        _inkMinionLaneNumber = laneNumber;
    }

    public string GetStampName()
    {
        return _inkMinionName;
    }

    public void DisableStamp()
    {

    }

    public void EnableStamp()
    {

    }
}
