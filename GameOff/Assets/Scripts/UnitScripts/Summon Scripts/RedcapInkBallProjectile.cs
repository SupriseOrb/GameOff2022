using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedcapInkBallProjectile : MonoBehaviour
{
    [SerializeField] private Transform _spriteTransform;
    private float _spriteRotationSpeed;
    [SerializeField] private Vector3 _projectileScale;
    [SerializeField] private float _projectileScaleTime;
    private float _projectileScaleSpeed;
    [SerializeField] private float _projectileMovementSpeed;
    [SerializeField] private Rigidbody2D _projectileRigidbody;
    [SerializeField] private float _destroyTime;
    [SerializeField] private float _projectileDamage;
    [SerializeField] private int _projectilePiercingAmt = 0;
    [SerializeField] private bool _projectileStunActive = false;
    // Start is called before the first frame update
    void Start()
    {
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
        _spriteTransform.Rotate(Vector3.back * (_spriteRotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //deal damage here
        if(other.gameObject.TryGetComponent(out IEnemy enemy))
        {
            if(_projectilePiercingAmt + 1 > 0)
            {
                enemy.TakeDamage(_projectileDamage);
                _projectilePiercingAmt--;
            }
            if(_projectilePiercingAmt <= 0)
            {
                Destroy(gameObject);
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

    public void SetStun(bool stun)
    {
        _projectileStunActive = stun;
    }
}
