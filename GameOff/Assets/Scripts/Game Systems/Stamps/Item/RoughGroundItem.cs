using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoughGroundItem : MonoBehaviour, IItemStamp
{
    [SerializeField] private float _groundBaseHealth;
    [SerializeField] private float _groundCurrentHealth;
    [SerializeField] private bool _isDead = false;
    [SerializeField] private float _groundSlowPercentage;
    [SerializeField] private BoxCollider2D _groundCollider;
    [SerializeField] private ItemStampScriptableObject _groundItemSO;
    [SerializeField] private CardScriptableObject _cardSO;

    #region Animation
    [SerializeField] private Animator _groundAnimator; 
    [SerializeField] private string _groundDisappearAnimation = "RoughGround_Dissapear";
    [SerializeField] private float _groundDisappearAnimationLength;
    #endregion

    private void Start() 
    {
        //AkSoundEngine.PostEvent("Play_StampSpikeyBush", gameObject);
        _groundBaseHealth = _groundItemSO.ItemHealth;
        _groundCurrentHealth = _groundBaseHealth;
        _groundSlowPercentage = _groundItemSO.ItemStampValue;
        //Debug.DrawLine(transform.position, transform.position + (Vector3.one * _bushAttackRange), Color.cyan, 100f);
        //Debug.DrawLine(transform.position, transform.position - (Vector3.one * _bushAttackRange), Color.cyan, 100f);
        AkSoundEngine.PostEvent("Play_StampRoughGround", gameObject);

        foreach (AnimationClip clip in _groundAnimator.runtimeAnimatorController.animationClips)
        {
            if(clip.name == _groundDisappearAnimation)
            {
                _groundDisappearAnimationLength = clip.length;
            }
        }
    }

    private void FixedUpdate() 
    {
        if (_isDead)
        {
            _groundDisappearAnimationLength -= Time.deltaTime;
            if (_groundDisappearAnimationLength <= -1)
            {
                AkSoundEngine.PostEvent("Play_DeathAnimation", gameObject);
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        _isDead = true;
        _groundAnimator.Play(_groundDisappearAnimation);
        _groundCollider.enabled = false;
    }

    public void HealHealth(float health)
    {

    }

    public void ActivateStampAbility()
    {

    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.TryGetComponent(out IEnemy enemy))
        {
            enemy.ReduceSpeeds(1 - _groundSlowPercentage, 0, 1 - _groundSlowPercentage, 0);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.TryGetComponent(out IEnemy enemy))
        {
            enemy.IncreaseSpeeds(1/(1 - _groundSlowPercentage), 1/(1 - _groundSlowPercentage));
        }    
    }

    public string GetStampName()
    {
        return _groundItemSO.StampName;
    }

    public void EnableStamp()
    {
        //SetActive true? or have stamp ability play based on _isActive?
    }

    public void DisableStamp()
    {
        //SetActive false / _isActive = false 
    }

    public void SetLane(int lane)
    {
        
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public string GetTileDescription()
    {
        string name = Vocab.SEPARATE(new string[] {_cardSO.CardName, Vocab.ITEM, Vocab.INKCOST(_cardSO.InkCost)});
        string description = _cardSO.CardDescription;
        return name + "\n" + description;
    }

}
