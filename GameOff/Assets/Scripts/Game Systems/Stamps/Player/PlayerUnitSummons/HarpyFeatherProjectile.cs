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
                    _featherForcedMoveSpeed = _harpyScript.ForcedMoveSpeed;
                    float currentYValue = 0.2f;
                    enemy.TakeDamage(_featherDamage);
                    int currentLane = enemy.GetLane();
                    if (currentLane == 0 || currentLane == 2) //If bottom or top lane
                    {
                        BoardManager.Instance.GetLane(currentLane).RemoveEnemyFromList(other.gameObject);
                        Vector3 newLocation = new Vector3(other.gameObject.transform.position.x, currentYValue, 0);
                        enemy.ForcedMove(other.gameObject.transform.position, newLocation, _featherForcedMoveSpeed);
                        BoardManager.Instance.GetLane(1).AddEnemyToList(other.gameObject); //This already sets the lane of the enemy
                    }
                    else if (currentLane == 1) //If middle lane
                    {
                        BoardManager.Instance.GetLane(currentLane).RemoveEnemyFromList(other.gameObject);
                        int randomLane = Random.Range(0,2);
                        switch (randomLane)
                        {
                            case 0:
                                currentYValue = 2.72f;
                                BoardManager.Instance.GetLane(0).AddEnemyToList(other.gameObject);
                                break;
                            default:
                                currentYValue = -2.32f;
                                BoardManager.Instance.GetLane(2).AddEnemyToList(other.gameObject);
                                break;
                        }
                        Vector3 newLocation = new Vector3(other.gameObject.transform.position.x, currentYValue, 0);
                        enemy.ForcedMove(other.gameObject.transform.position, newLocation, _featherForcedMoveSpeed);
                    }
                }
                else if(_currentUpgradePath == HarpyUnitScript.HarpyUpgradePaths.upgradeBoomingSong)
                {
                        Vector3 newLocation = new Vector3(_featherPushDistance, 0, 0);
                        _featherForcedMoveSpeed = _harpyScript.ForcedMoveSpeed;
                        enemy.TakeDamage(_featherDamage);
                        enemy.ReduceSpeeds(1 - _featherSlowIntensity, _featherSlowDuration);
                        enemy.ForcedMove(other.gameObject.transform.position, other.gameObject.transform.position + newLocation, _featherForcedMoveSpeed);
                }
                else
                {
                    enemy.TakeDamage(_featherDamage);
                    enemy.ReduceSpeeds(1 - _featherSlowIntensity, _featherSlowDuration);
                }
                _featherDamage = 0;
                Destroy(gameObject); 
            }     
        }
    }
}
