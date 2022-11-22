using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTile : BoardTile
{
    public override bool SetHeldStamp(GameObject stamp)
    {
        if(stamp.TryGetComponent(out IUnitStamp stampScript))
        {
            if(_heldStamp != null)
            {
                Debug.Log("Held Stamp Name: " + stampScript.GetStampName() +"| Placed Stamp Name: " + _heldStamp.GetComponent<IUnitStamp>().GetStampName());
            }
            if(_heldStamp == null || stampScript.GetStampName() != _heldStamp.GetComponent<IUnitStamp>().GetStampName()) //If it's a different stamp
            {
                _heldStamp = stamp; //Replace the unit on the tile
                if(gameObject.transform.childCount == 1)
                {
                    Destroy(gameObject.transform.GetChild(0).gameObject);
                }
                GameObject unit = Instantiate(stamp, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                unit.GetComponent<IUnitStamp>().SetLane(_laneNumber);
                //Tell the BoardLane that this unit is in this tile
                BoardManager.Instance.GetLane(_laneNumber).SetLaneUnit(unit, _tileNumber);
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
