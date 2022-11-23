using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpell : MonoBehaviour, ISpellStamp
{
    [SerializeField] private bool _isDead = false;
    [SerializeField] private int _potionHealAmount;
    [SerializeField] private int _empoweredPotionHealAmount;
    [SerializeField] private bool _isEmpowered = false;
    [SerializeField] private Sprite _potionSprite;
    [SerializeField] private int _laneNumber;
    [SerializeField] private GameObject _affectedItem;
    [SerializeField] private SpellStampScriptableObject _potionSpellSO;

    #region Animation
    [SerializeField] private Animator _potionAnimator; 
    [SerializeField] private string _potionDisappearAnim;
    [SerializeField] private float _potionDisappearAnimLength;
    #endregion

    private void Start() 
    {
        _potionSprite = _potionSpellSO.StampSprite;
        _potionHealAmount = (int)_potionSpellSO.SpellValue;
        _empoweredPotionHealAmount = 999;

        foreach (AnimationClip clip in _potionAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == _potionDisappearAnim)
            {
                _potionDisappearAnimLength = clip.length;
            }
        }

        ActivateStampAbility();
    }

    private void FixedUpdate() 
    {
        if (_isDead)
        {
            _potionDisappearAnimLength -= Time.deltaTime;
            if (_potionDisappearAnimLength <= -1)
            {
                Destroy(gameObject);
            }
        }
    }

    public void ActivateStampAbility()
    {
        if (_affectedItem.TryGetComponent(out IItemStamp itemStamp))
        {
            if (_isEmpowered)
            {
                itemStamp.HealHealth(_empoweredPotionHealAmount); 
                //This value will likely just be 999
            }
            else
            {
                itemStamp.HealHealth(_potionHealAmount);    
            }
            _isDead = true;
            _potionAnimator.Play(_potionDisappearAnim);
        }
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

    public void SetAffectedItem(GameObject item)
    {
        _affectedItem = item;
    }

    public bool IsDead()
    {
        return _isDead;
    }
}
