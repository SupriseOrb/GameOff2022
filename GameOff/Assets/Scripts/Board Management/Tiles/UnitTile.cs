using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTile : MonoBehaviour
{
    [SerializeField] private GameObject _heldStamp;

    public GameObject GetHeldStamp()
    {
        return _heldStamp;
    }

    public bool SetHeldStamp(GameObject stamp)
    {
        if(stamp.TryGetComponent(out IUnitStamp stampScript))
        {
            if(stampScript.GetUnitName() != _heldStamp.GetComponent<IUnitStamp>().GetUnitName()) //If it's a different stamp
            {
                _heldStamp = stamp; //Replace the unit on the tile
            }
            else
            {
                //Upgrade the current unit on the tile
            }
            return true;
        }
        return false;
    }
}
