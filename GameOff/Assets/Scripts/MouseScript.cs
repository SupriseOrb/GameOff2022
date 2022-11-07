using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    private Ray _raycast ;
    private RaycastHit _raycastHit;
    [SerializeField] private GameObject _heldObject = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Probably replace with new unity input system if needed
        //Primary button
        if (Input.GetMouseButtonDown(0))
        {
            _raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(_raycast, out _raycastHit))
            {
                if(_heldObject != null)
                {
                    if(_raycastHit.transform.gameObject.TryGetComponent(out BoardTile selectedBoardTile))
                    {
                        //attempt to place stamp in tile
                        if(!selectedBoardTile.SetHeldStamp(_heldObject))
                        {
                            //Do error stuff
                        }
                    }
                    else if(_raycastHit.transform.gameObject.TryGetComponent(out UnitTile selectedUnitTile))
                    {
                        if(!selectedUnitTile.SetHeldStamp(_heldObject))
                        {
                            //Do error stuff
                        }
                    }
                }
            }
        }
        //Secondary button
        if (Input.GetMouseButtonDown(1))
        {

        }
        //Middle button
        if (Input.GetMouseButtonDown(2))
        {

        }
    }
}
