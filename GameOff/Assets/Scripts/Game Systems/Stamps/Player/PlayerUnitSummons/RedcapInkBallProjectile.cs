using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedcapInkBallProjectile : MonoBehaviour
{
    [SerializeField] private RedcapUnitScript _redcapScript;
    [SerializeField] private RedcapUnitScript.RedcapUpgradePaths _currentUpgradePath;

#region Projectile Stats
    [SerializeField] private float _inkballDamage;
    [SerializeField] private int _inkballPiercingAmount = 0;
    [SerializeField] private float _inkballStunDuration;
    [SerializeField] private bool _inkballStunActive = false;
#endregion

#region Projectile Visuals
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _upgradeBaseSprite;
    [SerializeField] private Sprite _upgradeOneSprite;
    [SerializeField] private Sprite _upgradeTwoSprite;
    [SerializeField] private Vector3 _inkballScale;
#endregion
    
#region Projectile Variables
    [SerializeField] private float _inkballMovementSpeed;
    [SerializeField] private Rigidbody2D _inkballRigidbody;
    [SerializeField] private float _inkballDestroyTime;
    [SerializeField] private float _inkballScaleTime;
    private float _spriteRotationSpeed;
    private float _inkballScaleSpeed;
#endregion
    
    void Start()
    {
        AkSoundEngine.PostEvent("Play_RedcapAbility", gameObject);
        _inkballRigidbody = GetComponent<Rigidbody2D>();
        _redcapScript = gameObject.GetComponentInParent<RedcapUnitScript>();

        LoadBaseStats(); 
        Destroy(gameObject, _inkballDestroyTime);
    }

    private void LoadBaseStats()
    {
        _inkballDamage = _redcapScript.Damage;
        _inkballPiercingAmount = _redcapScript.PierceAmount;
        _inkballStunActive = _redcapScript.CanStun;
        _inkballStunDuration = _redcapScript.StunDuration;
        _currentUpgradePath = _redcapScript._currentUpgradePath;

        _spriteRenderer.sprite = SetSprite();
        _inkballRigidbody.velocity = Vector2.right * _inkballMovementSpeed;
        _spriteRotationSpeed = _inkballMovementSpeed * 100;
        _inkballScale = transform.localScale;
        _inkballScaleSpeed = _inkballScale.x/_inkballScaleTime;
        transform.localScale = Vector3.forward;
    }

    private void Update() 
    {
        if(transform.localScale.x < _inkballScale.x)
        {
            float scalar = transform.localScale.x + (_inkballScaleSpeed * Time.deltaTime);
            transform.localScale = new Vector3(scalar, scalar, transform.localScale.z);
        }   
        else if(transform.localScale.x > _inkballScale.x)
        {
            transform.localScale = _inkballScale;
        }
    }

    private void FixedUpdate() 
    {
        _spriteRenderer.gameObject.transform.Rotate(Vector3.back * (_spriteRotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //deal damage here
        if(other.gameObject.TryGetComponent(out IEnemy enemy))
        {
            AkSoundEngine.PostEvent("Play_EnemyTakeDamage", gameObject);
            
            if(_currentUpgradePath == RedcapUnitScript.RedcapUpgradePaths.upgradeLeadInk)
            {
                if(_inkballStunActive == true && _inkballDamage > 0)
                {   
                    enemy.TakeDamage(_inkballDamage);
                    enemy.Stun(_inkballStunDuration);
                }
            }
            else if(_currentUpgradePath == RedcapUnitScript.RedcapUpgradePaths.upgradeStickyInk)
            {
                if(_inkballPiercingAmount > 0)
                {
                    enemy.TakeDamage(_inkballDamage);
                    _inkballPiercingAmount--;
                }
            }
            else
            {
                //I don't think this if check matters anymore given we make sure that there is a specific upgrade for pierce to go through
                if(_inkballPiercingAmount <= 0)
                {
                    enemy.TakeDamage(_inkballDamage);
                    _inkballDamage = 0;
                    Destroy(gameObject);
                }
            }     
        }
    }

    public Sprite SetSprite()
    {
        if (_currentUpgradePath == RedcapUnitScript.RedcapUpgradePaths.upgradeLeadInk)
        {
            return _upgradeOneSprite;
        }
        else if (_currentUpgradePath == RedcapUnitScript.RedcapUpgradePaths.upgradeStickyInk)
        {
            return _upgradeTwoSprite;
        }
        else //if no upgrade
        {
            return _upgradeBaseSprite;
        }
    }
}
