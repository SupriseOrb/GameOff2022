using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTile : BoardTile
{
    public override bool SetHeldStamp(GameObject stamp)
    {
        if(stamp.TryGetComponent(out IUnitStamp stampScript))
        {
            if(_heldStamp == null || stampScript.GetStampName() != _heldStamp.GetComponent<IUnitStamp>().GetStampName()) //If it's a different stamp
            {
                if(gameObject.transform.childCount == 1)
                {
                    Destroy(gameObject.transform.GetChild(0).gameObject);
                }
                _heldStamp = Instantiate(stamp, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                _heldStamp.GetComponent<IUnitStamp>().SetLane(_laneNumber);
                //Tell the BoardLane that this unit is in this tile
                BoardManager.Instance.GetLane(_laneNumber).SetLaneUnit(_heldStamp, _tileNumber);
                return true;
            }
            else
            {
                //Upgrade the current unit on the tile
                _heldStamp.GetComponent<IUnitStamp>().OpenUnitUpgrade();
                return true;
            }
        }
        return false;
    }

}
