using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedcapInkBallProjectile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _upgradeBaseSprite;
    [SerializeField] private Sprite _upgradeOneSprite;
    [SerializeField] private Sprite _upgradeTwoSprite;
    private float _spriteRotationSpeed;
    [SerializeField] private Vector3 _projectileScale;
    [SerializeField] private float _projectileScaleTime;
    private float _projectileScaleSpeed;
    [SerializeField] private float _projectileMovementSpeed;
    [SerializeField] private Rigidbody2D _projectileRigidbody;
    [SerializeField] private float _destroyTime;
    [SerializeField] private float _projectileDamage;
    [SerializeField] private int _projectilePiercingAmt = 0;
    [SerializeField] private float _projectileStunDuration;
    [SerializeField] private bool _projectileStunActive = false;
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Play_Placeholder", gameObject);
        _projectileRigidbody = GetComponent<Rigidbody2D>(); 
        _projectileRigidbody.velocity = Vector2.right * _projectileMovementSpeed;
        _spriteRotationSpeed = _projectileMovementSpeed * 100;
        Destroy(gameObject, _destroyTime);
        _projectileScale = transform.localScale;
        _projectileScaleSpeed = _projectileScale.x/_projectileScaleTime;
        transform.localScale = Vector3.forward;
    }

    private void Update() 
    {
        if(transform.localScale.x < _projectileScale.x)
        {
            float scalar = transform.localScale.x + (_projectileScaleSpeed * Time.deltaTime);
            transform.localScale = new Vector3(scalar, scalar, transform.localScale.z);
        }   
        else if(transform.localScale.x > _projectileScale.x)
        {
            transform.localScale = _projectileScale;
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
            //If upgrade 2 is active
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
        _projectileDamage = damage;
    }

    public void SetPiercing(int pierce)
    {
        _projectilePiercingAmt = pierce;
    }

    public void SetStunValues(bool isStunned, float duration)
    {
        _projectileStunActive = isStunned;
        _projectileStunDuration = duration;
    }

    public void SetSprite(int upgrade)
    {
        switch (upgrade)
        {
            case 0:
                _spriteRenderer.sprite = _upgradeBaseSprite;
                break;
            case 1:
                _spriteRenderer.sprite = _upgradeOneSprite;
                break;
            default:
                _spriteRenderer.sprite = _upgradeTwoSprite;
                break;
        }
    }
}
