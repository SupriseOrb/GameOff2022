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
    [SerializeField] private CardScriptableObject _cardSO;
    [SerializeField] private int _laneNumber;

    #region Animation
    [SerializeField] private Animator _cowAnimator; 
    [SerializeField] private string _cowDisappearAnim;
    [SerializeField] private float _cowDisappearAnimLength;
    #endregion

    [SerializeField] private SpriteRenderer _cowSpriteRenderer;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _damageFlashMaterial;
    [SerializeField] private float _flashTime = .125f;
    [SerializeField] private Coroutine _damageFlashCoroutine = null;

    private void Start() 
    {
        _cowBaseHealth = _cowItemSO.ItemHealth;
        _cowCurrentHealth = _cowBaseHealth;
        _cowSprite = _cowItemSO.StampSprite;
        _cowInkGeneration = (int)_cowItemSO.ItemStampValue;
        _cowBaseCooldown = _cowItemSO.ItemCooldown;
        _cowCurrentCooldown = _cowBaseCooldown;
        AkSoundEngine.PostEvent("Play_StampCow", gameObject);

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
        // COLLIN TODO: PLAYER STAMP TAKE DAMAGE
        _cowCurrentHealth -= damage;
        AkSoundEngine.PostEvent("Play_ItemTakeDamage", gameObject);
        if (_cowCurrentHealth <= 0)
        {
            // Collin todo: play dead sfx
            _isDead = true;
            AkSoundEngine.PostEvent("Play_DeathAnimation", gameObject);
            _cowAnimator.Play(_cowDisappearAnim);
            _cowCollider.enabled = false;
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
        _cowCurrentHealth += health;
        if(_cowCurrentHealth > _cowBaseHealth)
        {
            _cowCurrentHealth = _cowBaseHealth;
        }
    }

    public void ActivateStampAbility()
    {
        // COLLIN TODO: INK COW ACTION
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

    private IEnumerator DamageFlashCoroutine()
    {
        _cowSpriteRenderer.material = _damageFlashMaterial;

        yield return new WaitForSeconds(_flashTime);
        _cowSpriteRenderer.material = _defaultMaterial;
        _damageFlashCoroutine = null;
    }

    public string GetTileDescription()
    {
        string name = Vocab.SEPARATE(new string[] {_cardSO.CardName, Vocab.ITEM, Vocab.INKCOST(_cardSO.InkCost)});
        string description = _cardSO.CardDescription;
        string stats = Vocab.SEPARATE(new string[] {Vocab.HEALTH(_cowCurrentHealth), Vocab.INKDELTA(_cowInkGeneration)});
        return name + "\n" + description + "\n" + stats;
    }
}
