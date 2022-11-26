using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherProjectile : MonoBehaviour
{
    [SerializeField] private float _projectileMovementSpeed;
    [SerializeField] private Rigidbody2D _projectileRigidbody;
    [SerializeField] private float _destroyTime;
    [SerializeField] 
    void Start()
    {
        AkSoundEngine.PostEvent("Play_RedcapAbility",gameObject);
        _projectileRigidbody = GetComponent<Rigidbody2D>(); 
        _projectileRigidbody.velocity = Vector2.right * _projectileMovementSpeed;
        Destroy(gameObject, _destroyTime);
        transform.localScale = Vector3.forward;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //deal damage here
        if(other.gameObject.TryGetComponent(out IEnemy enemy))
        {
            AkSoundEngine.PostEvent("Play_EnemyTakeDamage", gameObject);

            //If upgrade 1 is active
            if(_projectilePiercingAmt > 0)
            {
                Debug.Log("Pierced!");
                enemy.TakeDamage(_projectileDamage);
                _projectilePiercingAmt--;
                Debug.Log(_projectilePiercingAmt);
            }
            else if(_projectilePiercingAmt <= 0)
            {
                enemy.TakeDamage(_projectileDamage);
                _projectileDamage = 0;
                Destroy(gameObject);
            }

            //If upgrade 3 is active
            if (_projectileStunActive == true)
            {
                enemy.Stun(_projectileStunDuration);
            }
        }
    }

    public void SetDamage(float damage)
    {
        _featherDamage = damage;
    }

    public void SetPushDistance(int pushDistance)
    {
        _featherPushDistance = pushDistance;
    }
} 
