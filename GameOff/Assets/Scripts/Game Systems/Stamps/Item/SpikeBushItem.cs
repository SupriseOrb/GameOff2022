using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBushItem : MonoBehaviour, IItemStamp
{
    [SerializeField] private float _bushBaseHealth;
    [SerializeField] private float _bushCurrentHealth;
    [SerializeField] private float _bushBaseCooldown;
    [SerializeField] private float _bushCurrentCooldown;
    [SerializeField] private bool _isOnCooldown = true;
    [SerializeField] private bool _isDead = false;
    [SerializeField] private float _bushDamage;
    [SerializeField] private Sprite _bushSprite;
    [SerializeField] private float _bushAttackRange;
    [SerializeField] private BoxCollider2D _bushCollider;
    [SerializeField] private ItemStampScriptableObject _bushItemSO;
    [SerializeField] private CardScriptableObject _cardSO;
    private Collider2D[] _bushColliders;

    #region Animation
    [SerializeField] private Animator _bushAnimator; 
    [SerializeField] private string _bushDisappearAnim;
    [SerializeField] private float _bushDisappearAnimLength;
    #endregion

    [SerializeField] private SpriteRenderer _bushSpriteRenderer;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _damageFlashMaterial;
    [SerializeField] private float _flashTime = .125f;
    [SerializeField] private Coroutine _damageFlashCoroutine = null;

    private void Start() 
    {
        AkSoundEngine.PostEvent("Play_StampSpikeyBush", gameObject);
        _bushBaseHealth = _bushItemSO.ItemHealth;
        _bushCurrentHealth = _bushBaseHealth;
        _bushSprite = _bushItemSO.StampSprite;
        _bushDamage = _bushItemSO.ItemStampValue;
        _bushBaseCooldown = _bushItemSO.ItemCooldown;
        _bushCurrentCooldown = _bushBaseCooldown;
        //Debug.DrawLine(transform.position, transform.position + (Vector3.one * _bushAttackRange), Color.cyan, 100f);
        //Debug.DrawLine(transform.position, transform.position - (Vector3.one * _bushAttackRange), Color.cyan, 100f);

        foreach(AnimationClip clip in _bushAnimator.runtimeAnimatorController.animationClips)
        {
            if(clip.name == _bushDisappearAnim)
            {
                _bushDisappearAnimLength = clip.length;
            }
        }
    }

    private void FixedUpdate() 
    {
        if (_isDead)
        {
            _bushDisappearAnimLength -= Time.deltaTime;
            if (_bushDisappearAnimLength <= -1)
            {
                Destroy(gameObject);
            }
        }
        if (_isOnCooldown)
        {
            _bushCurrentCooldown -= Time.deltaTime;
        }
        if (_bushCurrentCooldown <= 0)
        {
            _isOnCooldown = false;
            _bushCurrentCooldown = _bushBaseCooldown;
        }
    }

    public void TakeDamage(float damage)
    {
        AkSoundEngine.PostEvent("Play_ItemTakeDamage", gameObject);
        _bushCurrentHealth -= damage;
        if (!_isOnCooldown)
        {
            ActivateStampAbility();
        }
        if (_bushCurrentHealth <= 0)
        {
            
            _isDead = true;
            AkSoundEngine.PostEvent("Play_DeathAnimation", gameObject);
            _bushAnimator.Play(_bushDisappearAnim);
            _bushCollider.enabled = false;
        }
        else
        {
            if(_damageFlashCoroutine != null)
            {
                StopCoroutine(_damageFlashCoroutine);
            }
            
            _damageFlashCoroutine = StartCoroutine(DamageFlashCoroutine());
        }
    }

    public void HealHealth(float health)
    {
        _bushCurrentHealth += health;
        if(_bushCurrentHealth > _bushBaseHealth)
        {
            _bushCurrentHealth = _bushBaseHealth;
        }
    }

    public void ActivateStampAbility()
    {
        AkSoundEngine.PostEvent("Play_SpikeyBushHit", gameObject);
        _bushColliders = Physics2D.OverlapCircleAll(transform.position, _bushAttackRange);
        if (_bushColliders != null && _bushCollider != null)
        {
            foreach (Collider2D collider in _bushColliders)
            {
                if (collider.TryGetComponent(out IEnemy enemy))
                {
                    enemy.TakeDamage(_bushDamage);
                }
            }
        }
        _isOnCooldown = true;
    }

    public string GetStampName()
    {
        return _bushItemSO.StampName;
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
    
    private IEnumerator DamageFlashCoroutine()
    {
        _bushSpriteRenderer.material = _damageFlashMaterial;

        yield return new WaitForSeconds(_flashTime);
        _bushSpriteRenderer.material = _defaultMaterial;
        _damageFlashCoroutine = null;
    }

    public string GetTileDescription()
    {
        string name = Vocab.SEPARATE(new string[] {_cardSO.CardName, Vocab.ITEM, Vocab.INKCOST(_cardSO.InkCost)});
        string description = _cardSO.CardDescription;
        string stats = Vocab.SEPARATE(new string[] {Vocab.HEALTH(_bushCurrentHealth), Vocab.DAMAGE(_bushDamage)});
        return name + "\n" + description + "\n" + stats;
    }
}
