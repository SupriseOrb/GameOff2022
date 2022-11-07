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
            if(stampScript.GetUnitName() != _heldStamp.GetComponent<IUnitStamp>().GetUnitName())
            {
                _heldStamp = stamp;
            }
            else
            {
                //do upgrade magic here
            }
            return true;
        }
        return false;
    }
}
