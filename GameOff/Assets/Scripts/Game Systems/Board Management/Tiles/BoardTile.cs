using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    [SerializeField] protected GameObject _heldStamp;
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
            _heldStamp = stamp;
            if(gameObject.transform.childCount == 1)
            {
                Destroy(gameObject.transform.GetChild(0));
            }
            Instantiate(stamp, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            return true;
        }
        return false;
    }
}