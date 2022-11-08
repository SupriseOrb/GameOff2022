using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private GameObject _heldObject = null;
    [SerializeField] private Vector3 _mousePosition;
    private Ray _raycast;
    private RaycastHit _raycastHit;

    #region Input Action Strings
    [Header("Input Strings")]
    [SerializeField] private string _lClickString;
    [SerializeField] private string _rClickString;
    [SerializeField] private string _mClickString;
    [SerializeField] private string _mousePosString;
    #endregion

    #region Input Actions
    private InputAction _lClickAction;
    private InputAction _rClickAction;
    private InputAction _mClickAction;
    private InputAction _mousePosAction;
    #endregion

    private void Awake() 
    {
        _lClickAction = _playerInput.actions[_lClickString];
        _rClickAction = _playerInput.actions[_rClickString];
        _mClickAction = _playerInput.actions[_mClickString];
        _mousePosAction = _playerInput.actions[_mousePosString];
    }

    private void OnEnable() 
    {
        _lClickAction.performed += OnLeftClick;
        _rClickAction.performed += OnRightClick;
        _mClickAction.performed += OnMiddleClick;
        _mousePosAction.performed += MousePosition;
    }

    private void OnDisable() 
    {
        _lClickAction.performed -= OnLeftClick;
        _rClickAction.performed -= OnRightClick;
        _mClickAction.performed -= OnMiddleClick;
        _mousePosAction.performed -= MousePosition;
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        //If something is funky, might need to convert _mousePosition to world space (using ScreenToWorldPoint)
        _raycast = Camera.main.ScreenPointToRay(_mousePosition);
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

    public void OnRightClick(InputAction.CallbackContext context)
    {

    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {

    }

    public void MousePosition(InputAction.CallbackContext context)
    {
        //Need to test to see if having this be checked only on "performed" is actually getting the value consistently
        //Also wtf is the z value if I passed it in as a Vector3? Can a Vector2 work anyways for the raycast?
        _mousePosition = context.ReadValue<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
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
        */
    }
}
