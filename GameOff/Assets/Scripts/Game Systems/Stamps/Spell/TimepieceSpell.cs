using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimepieceSpell : MonoBehaviour, ISpellStamp
{
    [SerializeField] private bool _isDeactivated = false;
    [SerializeField] private int _timepieceReductionAmount;
    [SerializeField] private Sprite _timepieceSprite;
    [SerializeField] private int _laneNumber;
    [SerializeField] private BoardTile _affectedTile;
    [SerializeField] private SpellStampScriptableObject _timepieceSpellSO;
    [SerializeField] private GameObject[] _affectedUnits;

    #region Animation
    [SerializeField] private Animator _timepieceAnimator; 
    [SerializeField] private string _timepieceDisappearAnimation = "TimePiece_Disappear";
    [SerializeField] private float _timepieceDisappearAnimationLength;
    #endregion

    private void Start() 
    {
        
    }

    private void FixedUpdate() 
    {
        if (_isDeactivated)
        {
            _timepieceDisappearAnimationLength -= Time.deltaTime;
            if (_timepieceDisappearAnimationLength <= -1)
            {
                Destroy(gameObject);
            }
        }
    }

    public void LoadBaseStats()
    {
        _timepieceSprite = _timepieceSpellSO.StampSprite;
        _timepieceReductionAmount = (int)_timepieceSpellSO.SpellValue;

        foreach (AnimationClip clip in _timepieceAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == _timepieceDisappearAnimation)
            {
                _timepieceDisappearAnimationLength = clip.length;
            }
        }
    }

    public void ActivateStampAbility()
    {
        LoadBaseStats();
        _affectedUnits = BoardManager.Instance.GetLane(_laneNumber).GetLaneUnits();
        bool paidInkCost = false;
        foreach(GameObject unit in _affectedUnits)
        {
            if(unit != null && unit.TryGetComponent(out IUnitStamp unitScript))
            {
                if(!paidInkCost)
                {
                    paidInkCost = true;
                    if(!DeckManager.Instance.RemoveInk(DeckManager.Instance.SelectedCard.CardSO.InkCost))
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        AkSoundEngine.PostEvent("Play_StampTimePiece", gameObject);
                        unitScript.ReduceCooldown(_timepieceReductionAmount);
                        _isDeactivated = true;
                    }
                }
                else 
                {
                    unitScript.ReduceCooldown(_timepieceReductionAmount);
                    _isDeactivated = true;
                }
            }
        }
        if(!paidInkCost)
        {
            Destroy(gameObject);
        }
    }
    
    public string GetStampName()
    {
        return _timepieceSpellSO.StampName;
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
