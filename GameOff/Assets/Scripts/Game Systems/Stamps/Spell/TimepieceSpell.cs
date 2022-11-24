using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimepieceSpell : MonoBehaviour, ISpellStamp
{
    [SerializeField] private bool _isDead = false;
    [SerializeField] private int _timepieceReductionAmount;
    [SerializeField] private Sprite _timepieceSprite;
    [SerializeField] private int _laneNumber;
    [SerializeField] private BoardTile _affectedTile;
    [SerializeField] private SpellStampScriptableObject _timepieceSpellSO;

    #region Animation
    [SerializeField] private Animator _timepieceAnimator; 
    [SerializeField] private string _timepieceDisappearAnim;
    [SerializeField] private float _timepieceDisappearAnimLength;
    #endregion

    private void Start() 
    {
        _timepieceSprite = _timepieceSpellSO.StampSprite;
        _timepieceReductionAmount = (int)_timepieceSpellSO.SpellValue;

        foreach (AnimationClip clip in _timepieceAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == _timepieceDisappearAnim)
            {
                _timepieceDisappearAnimLength = clip.length;
            }
        }

        ActivateStampAbility();
    }

    private void FixedUpdate() 
    {
        if (_isDead)
        {
            _timepieceDisappearAnimLength -= Time.deltaTime;
            if (_timepieceDisappearAnimLength <= -1)
            {
                Destroy(gameObject);
            }
        }
    }

    public void ActivateStampAbility()
    {
        if (_affectedTile.TryGetComponent(out IItemStamp itemStamp))
        {
            
            _isDead = true;
            _timepieceAnimator.Play(_timepieceDisappearAnim);
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

    public bool IsDead()
    {
        return _isDead;
    }
}
