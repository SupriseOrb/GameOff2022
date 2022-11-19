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
    [SerializeField] private string _inkMinionAttackAnimationName = "InkDemon_Attack";
    [SerializeField] private string _inkMinionAppearAnimationName = "InkDemon_Appear";
    //Might be needed idk
    public int _inkMinionLaneNumber;

    // Start is called before the first frame update
    void Start()
    {
        _inkMinionAnimator.Play(_inkMinionAppearAnimationName);
    }

    public void UpdateMinionStats
    (
        int health = 0, 
        float attackSpeed = 0, 
        float attackDamage = 0, 
        int deathMultiplier = 0, 
        float slowAmount = 0, 
        float slowDuration = 0,
        int laneNumber = 0
    )
    {
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
    }        

    // Update is called once per frame
    void Update()
    {
        if(_isActive)
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
                enemyScript.TakeDamage(_inkMinionAttackDamage);
            }
        }
        _inkMinionAnimator.Play(_inkMinionAttackAnimationName);
    }

    public void TakeDamage(float damage)
    {
        _inkMinionCurrentHealth -= damage;
        if(_inkMinionCurrentHealth <= 0)
        {
            ActivateStampAbility();
            Destroy(gameObject);

            /*TODO:
                Disable minion collider
                Play death animation
                Destroy after animation completes
            */
        }
    }

    public void DisableStamp()
    {

    }

    public void EnableStamp()
    {

    }
}
