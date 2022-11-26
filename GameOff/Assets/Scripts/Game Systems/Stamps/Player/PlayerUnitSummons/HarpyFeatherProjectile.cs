using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpyFeatherProjectile : MonoBehaviour
{
    [SerializeField] private HarpyUnitScript _harpyScript;
    [SerializeField] private HarpyUnitScript.HarpyUpgradePaths _currentUpgradePath;

#region Projectile Stats
    [SerializeField] private float _featherDamage;
    [SerializeField] private float _featherPushDistance;
    [SerializeField] private float _featherForcedMoveSpeed;
    [Range(0f, 1f)]
    [SerializeField] private float _featherSlowIntensity;
    [SerializeField] private float _featherSlowDuration;
#endregion

/*#region Projectile Visuals
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _upgradeBaseSprite;
    [SerializeField] private Sprite _upgradeOneSprite;
    [SerializeField] private Sprite _upgradeTwoSprite;
#endregion*/
//Shouldn't need any visuals
    
#region Projectile Variables
    [SerializeField] private float _featherMovementSpeed;
    [SerializeField] private Rigidbody2D _featherRigidbody;
    [SerializeField] private float _featherDestroyTime;
#endregion
    
    void Start()
    {
        //AkSoundEngine.PostEvent("Play_HarpyAbility", gameObject);
        _featherRigidbody = GetComponent<Rigidbody2D>();
        _harpyScript = gameObject.GetComponentInParent<HarpyUnitScript>();

        LoadBaseStats(); 
        Destroy(gameObject, _featherDestroyTime);
    }

    private void LoadBaseStats()
    {
        _featherDamage = _harpyScript.Damage;
        _featherPushDistance = _harpyScript.PushDistance;
        _featherSlowIntensity = _harpyScript.SlowIntensity;
        _featherSlowDuration = _harpyScript.SlowDuration;
        _featherPushDistance = _harpyScript.PushDistance;
        _currentUpgradePath = _harpyScript._currentUpgradePath;

        _featherRigidbody.velocity = Vector2.right * _featherMovementSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //deal damage here
        if(other.gameObject.TryGetComponent(out IEnemy enemy))
        {
            AkSoundEngine.PostEvent("Play_EnemyTakeDamage", gameObject);
            
            if (_featherDamage > 0)
            {
                if(_currentUpgradePath == HarpyUnitScript.HarpyUpgradePaths.upgradeDisorientingSong)
                {
                    enemy.TakeDamage(_featherDamage);
                    //_featherForcedMoveSpeed = _harpyScript.
                    /*
                    Based on the lane of the enemy:
                    [1] If it's the top or bottom lane:
                        Vector3 newLocation = new Vector3(other.gameObject.transform.position.x, (y value representing middle lane), other.gameObject.transform.position.z);
                        enemy.ForcedMove(other.gameObject.transform.position, newLocation, timer);
                    [2] If it's the middle lane:
                    - int poopy = Random.Range (0,2)



                    */
                }
                else if(_currentUpgradePath == HarpyUnitScript.HarpyUpgradePaths.upgradeBoomingSong)
                {
                    if (_featherDamage > 0)
                    {
                        Vector3 newLocation = new Vector3(_featherPushDistance, 0, 0);
                        _featherForcedMoveSpeed = _harpyScript.ForcedMoveSpeed;
                        enemy.TakeDamage(_featherDamage);
                        enemy.ModifySpeeds(1 - _featherSlowIntensity, _featherSlowDuration);
                        enemy.ForcedMove(other.gameObject.transform.position, other.gameObject.transform.position + newLocation, _featherForcedMoveSpeed);
                    }
                }
                else
                {
                    enemy.TakeDamage(_featherDamage);
                    enemy.ModifySpeeds(1 - _featherSlowIntensity, _featherSlowDuration);
                }
                _featherDamage = 0;
                Destroy(gameObject); 
            }     
        }
    }
}
