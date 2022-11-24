using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkBlotSpell : MonoBehaviour, ISpellStamp
{
    [SerializeField] private bool _isDead = false;
    [SerializeField] private bool _isOffensive = false;
    [SerializeField] private int _blotDamageAmount;
    [SerializeField] private BoardTile _affectedTile;
    //[SerializeField] private int _empoweredBlotHealAmount;
    [SerializeField] private Sprite _blotSprite;
    [SerializeField] private int _laneNumber;
    [SerializeField] private SpellStampScriptableObject _blotSpellSO;

    #region Animation
    [SerializeField] private Animator _blotAnimator; 
    [SerializeField] private string _blotDisappearAnim;
    [SerializeField] private float _blotDisappearAnimLength;
    #endregion

    private void Start() 
    {
        _blotSprite = _blotSpellSO.StampSprite;
        _blotDamageAmount = (int)_blotSpellSO.SpellValue;
        _isOffensive = _blotSpellSO.IsOffensive;  

        foreach (AnimationClip clip in _blotAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == _blotDisappearAnim)
            {
                _blotDisappearAnimLength = clip.length;
            }
        }
        ActivateStampAbility();
    }

    public void ActivateStampAbility()
    {

    }

    public void SetTile(BoardTile tile)
    {
        _affectedTile = tile;
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public string GetStampName()
    {
        return _blotSpellSO.StampName;
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
}
