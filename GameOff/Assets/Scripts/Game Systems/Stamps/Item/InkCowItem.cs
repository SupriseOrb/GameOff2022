using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkCowItem : MonoBehaviour, IItemStamp
{
    [SerializeField] private float _cowBaseHealth;
    [SerializeField] private float _cowCurrentHealth;
    [SerializeField] private float _cowBaseCooldown;
    [SerializeField] private float _cowCurrentCooldown;
    [SerializeField] private bool _isOnCooldown = true;
    [SerializeField] private bool _isDead = false;
    [SerializeField] private int _cowInkGeneration;
    [SerializeField] private Sprite _cowSprite;
    [SerializeField] private BoxCollider2D _cowCollider;
    [SerializeField] private ItemStampScriptableObject _cowItemSO;
    [SerializeField] private int _laneNumber;

    #region Animation
    [SerializeField] private Animator _cowAnimator; 
    [SerializeField] private string _cowDisappearAnim;
    [SerializeField] private float _cowDisappearAnimLength;
    #endregion

    private void Start() 
    {
        _cowBaseHealth = _cowItemSO.ItemHealth;
        _cowCurrentHealth = _cowBaseHealth;
        _cowSprite = _cowItemSO.StampSprite;
        _cowInkGeneration = (int)_cowItemSO.ItemStampValue;
        _cowBaseCooldown = _cowItemSO.ItemCooldown;
        _cowCurrentCooldown = _cowBaseCooldown;

        foreach (AnimationClip clip in _cowAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == _cowDisappearAnim)
            {
                _cowDisappearAnimLength = clip.length;
            }
        }

    }

    private void FixedUpdate() 
    {
        if (_isDead)
        {
            _cowDisappearAnimLength -= Time.deltaTime;
            if (_cowDisappearAnimLength <= -1)
            {
                Destroy(gameObject);
            }
        }

        if (_isOnCooldown)
        {
            _cowCurrentCooldown -= Time.deltaTime;
        }

        if (_cowCurrentCooldown <= 0)
        {
            _isOnCooldown = false;
            _cowCurrentCooldown = _cowBaseCooldown;
            ActivateStampAbility();
        }     
    }

    public void TakeDamage(float damage)
    {
        _cowCurrentHealth -= damage;
        if (_cowCurrentHealth <= 0)
        {
            _isDead = true;
            _cowAnimator.Play(_cowDisappearAnim);
            _cowCollider.enabled = false;
        }
    }

    public void HealHealth(float health)
    {
        _cowCurrentHealth += health;
        if(_cowCurrentHealth > _cowBaseHealth)
        {
            _cowCurrentHealth = _cowBaseHealth;
        }
    }

    public void ActivateStampAbility()
    {
        if(BoardManager.Instance.GetLane(_laneNumber).GetLeylineStatus())
        {
            float multiplier = BoardManager.Instance.GetLane(_laneNumber).GetLeylineMultiplier();
            DeckManager.Instance.AddInk((int)(_cowInkGeneration * multiplier));
        }
        else
        {
            DeckManager.Instance.AddInk(_cowInkGeneration);
        }
        _isOnCooldown = true;
    }
 
    public string GetStampName()
    {
        return _cowItemSO.StampName;
    }

    public void EnableStamp()
    {
        
    }

    public void DisableStamp()
    {

    }

    public void SetLane(int lane)
    {
        _laneNumber = lane;
    }

    public bool IsDead()
    {
        return _isDead;
    }

}
