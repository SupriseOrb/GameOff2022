using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    [SerializeField] protected GameObject _heldStamp;
    [SerializeField] protected GameObject _currentSpell;
    [SerializeField] public int _laneNumber;
    [SerializeField] public int _tileNumber;

    public GameObject GetHeldStamp()
    {
        if(gameObject.transform.childCount >= 1)
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
            if (true)
            {
                if (gameObject.transform.childCount == 1) //If there is an item (as to not break other functionality)
                {
                    _currentSpell = Instantiate(spell, gameObject.transform.position, Quaternion.identity, gameObject.transform.GetChild(0).gameObject.transform);
                }
                else //If there is no item; empty tile
                {
                    _currentSpell = Instantiate(spell, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                }
                _currentSpell.GetComponent<IStamp>().SetLane(_laneNumber);
                return true;
            }
            else if(gameObject.transform.childCount == 1)
            {
                //This is technically setting the item before the item is actually instantiated, which, while is working, is concerning?
                spellStamp.SetAffectedItem(gameObject.transform.GetChild(0).gameObject);
                _currentSpell = Instantiate(spell, gameObject.transform.position, Quaternion.identity, gameObject.transform.GetChild(0).gameObject.transform);
                _currentSpell.GetComponent<IStamp>().SetLane(_laneNumber);
                return true;
            }
        }
        return false;
    }
}
