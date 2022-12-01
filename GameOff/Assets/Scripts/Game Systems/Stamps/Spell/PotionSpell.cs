using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpell : MonoBehaviour, ISpellStamp
{
    [SerializeField] private bool _isDeactivated = true;
    [SerializeField] private bool _isActivated = false;
    [SerializeField] private int _potionHealAmount;
    [SerializeField] private int _empoweredPotionHealAmount;
    [SerializeField] private Sprite _potionSprite;
    [SerializeField] private int _laneNumber;
    [SerializeField] private BoardTile _affectedTile;
    [SerializeField] private SpellStampScriptableObject _potionSpellSO;

    #region Animation
    [SerializeField] private Animator _potionAnimator; 
    [SerializeField] private string _potionAppearAnimation = "Potion_Appear";
    [SerializeField] private float _potionAppearAnimationLength;
    [SerializeField] private string _potionDisappearAnimation = "Potion_Disappear";
    [SerializeField] private float _potionDisappearAnimationLength;
    #endregion

    private void Start() 
    {
        
    }

    private void FixedUpdate() 
    {
        if(_isDeactivated)
        {
            _potionDisappearAnimationLength -= Time.deltaTime;
            if(_potionDisappearAnimationLength <= -1)
            {
                Destroy(gameObject);
            }
        }
        else if(_isActivated)
        {
            _potionAppearAnimationLength -= Time.deltaTime;
            if(_potionAppearAnimationLength <= 0)
            {
                _isDeactivated = true;
            }
        } 
    }

    public void LoadBaseStats()
    {
        _potionSprite = _potionSpellSO.StampSprite;
        _potionHealAmount = (int)_potionSpellSO.SpellValue;
        _empoweredPotionHealAmount = 999;

        foreach (AnimationClip clip in _potionAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == _potionDisappearAnimation)
            {
                _potionDisappearAnimationLength = clip.length;
            }
            else if(clip.name == _potionAppearAnimation)
            {
                _potionAppearAnimationLength = clip.length;
            }
        }
    }

    public void ActivateStampAbility()
    {
        LoadBaseStats();
        if(_affectedTile.GetHeldStamp() != null)
        {
            if (_affectedTile.GetHeldStamp().TryGetComponent(out IItemStamp itemStamp))
            {
                if(DeckManager.Instance.RemoveInk(DeckManager.Instance.SelectedCard.CardSO.InkCost))
                {
                    if (BoardManager.Instance.GetLane(_laneNumber).GetLeylineStatus())
                    {
                        //float multiplier = BoardManager.Instance.GetLane(_laneNumber).GetLeylineMultiplier();
                        //itemStamp.HealHealth(_potionHealAmount * multiplier); 
                        itemStamp.HealHealth(_empoweredPotionHealAmount); 
                        //This value will likely just be 999
                    }
                    else
                    {
                        itemStamp.HealHealth(_potionHealAmount);    
                    }
                    _isDeactivated = false;
                    _isActivated = true;
                    AkSoundEngine.PostEvent("Play_StampPotion", gameObject);
                    _potionAnimator.Play(_potionAppearAnimation);
                }
            }
        }
    }
    
    public string GetStampName()
    {
        return _potionSpellSO.StampName;
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

    public void SetTile(BoardTile tile)
    {
        _affectedTile = tile;
    }

    public string GetTileDescription()
    {
        return "";
    }
}
