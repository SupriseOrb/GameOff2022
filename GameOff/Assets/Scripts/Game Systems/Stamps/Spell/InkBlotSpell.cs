using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkBlotSpell : MonoBehaviour, ISpellStamp
{
    [SerializeField] private bool _isDead = false;
    [SerializeField] private int _blotDamage;
    [SerializeField] private BoardTile _affectedTile;
    [SerializeField] private Sprite _blotSprite;
    [SerializeField] private int _laneNumber;
    [SerializeField] private Collider2D[] _blotColliders;
    [SerializeField] private float _blotRange;
    [SerializeField] private SpellStampScriptableObject _blotSpellSO;

    #region Animation
    [SerializeField] private Animator _blotAnimator; 
    [SerializeField] private string _blotDisappearAnim;
    [SerializeField] private float _blotDisappearAnimLength;
    #endregion

    private void LoadBaseStats() 
    {
        _blotSprite = _blotSpellSO.StampSprite;
        _blotDamage = (int)_blotSpellSO.SpellValue;
        foreach (AnimationClip clip in _blotAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == _blotDisappearAnim)
            {
                _blotDisappearAnimLength = clip.length;
            }
        }
    }

    public void ActivateStampAbility()
    {
        // COLLIN TODO: ADD INKBLOT SFX
        LoadBaseStats();
        _blotColliders = Physics2D.OverlapCircleAll(transform.position, _blotRange);
        if (_blotColliders != null)
        {
            if(DeckManager.Instance.RemoveInk(DeckManager.Instance.SelectedCard.CardSO.InkCost))
            {
                foreach (Collider2D collider in _blotColliders)
                {
                    if (collider.gameObject.TryGetComponent(out IEnemy enemy))
                    {
                        if (BoardManager.Instance.GetLane(_laneNumber).GetLeylineStatus())
                        {
                            float multiplier = BoardManager.Instance.GetLane(_laneNumber).GetLeylineMultiplier();
                            enemy.TakeDamage(_blotDamage * multiplier); 
                        }
                        else
                        {
                            enemy.TakeDamage(_blotDamage); 
                        }    
                    }
                }
            }   
        }
        _isDead = true;
    }      
        
    private void FixedUpdate() 
    {
        if (_isDead)
        {
            _blotDisappearAnimLength -= Time.deltaTime;
            if (_blotDisappearAnimLength <= -1)
            {
                Destroy(gameObject);
            }
        }  
    }
    

    public void SetTile(BoardTile tile)
    {
        _affectedTile = tile;
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

    public string GetTileDescription()
    {
        return "";
    }
}
