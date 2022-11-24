using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    [SerializeField] protected GameObject _heldStamp;
    [SerializeField] protected GameObject _currentSpell = null;
    [SerializeField] public int _laneNumber;
    [SerializeField] public int _tileNumber;

    public GameObject GetHeldStamp()
    {
        if(gameObject.transform.childCount == 1)
        {
            return _heldStamp;
        }
        return _heldStamp = null;
    }

    public virtual bool SetHeldStamp(GameObject stamp)
    {
        if(stamp.TryGetComponent(out IStamp stampScript))
        {
            if(gameObject.transform.childCount == 1)
            {
                Destroy(gameObject.transform.GetChild(0).gameObject);
            }
            _heldStamp = Instantiate(stamp, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            _heldStamp.GetComponent<IStamp>().SetLane(_laneNumber);
            return true;
        }
        return false;
    }

    public virtual bool PlaySpell(GameObject spell)
    {
        if (spell.TryGetComponent(out ISpellStamp spellStamp))
        {
            //spellStamp.SetAffectedItem(gameObject.transform.GetChild(0).gameObject);
            _currentSpell = Instantiate(spell, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            _currentSpell.GetComponent<IStamp>().SetLane(_laneNumber);
            _currentSpell.GetComponent<ISpellStamp>().SetTile(gameObject.GetComponent<BoardTile>());
            _currentSpell.GetComponent<ISpellStamp>().ActivateStampAbility();
            return true;
        }
        return false;
    }
}
