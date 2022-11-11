using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    [SerializeField] private GameObject _heldStamp;

    public GameObject GetHeldStamp()
    {
        return _heldStamp;
    }

    public bool SetHeldStamp(GameObject stamp)
    {
        if(stamp.TryGetComponent(out IStamp stampScript))
        {
            _heldStamp = stamp;
            return true;
        }
        return false;
    }
}
