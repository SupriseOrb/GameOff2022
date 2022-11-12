using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private GameObject _heldObject = null;
    [SerializeField] private Vector3 _mousePositionScreen;
    [SerializeField] private Vector3 _mousePositionWorld;
    [SerializeField] private CardScriptLoader _selectedCard = null;
    [SerializeField] private SpriteDrag _spriteDragScript;
    private Ray _raycast;
    private RaycastHit _raycastHit;

    public Vector3 MousePositionWorld
    {
        get {return _mousePositionWorld;}
    }

    #region Input Action Strings
    [Header("Input Strings")]
    [SerializeField] private string _lClickString;
    [SerializeField] private string _lClickHoldString;
    [SerializeField] private string _rClickString;
    [SerializeField] private string _mClickString;
    [SerializeField] private string _mousePosString;
    #endregion

    #region Input Actions
    private InputAction _lClickAction;
    private InputAction _lClickHoldAction;
    private InputAction _rClickAction;
    private InputAction _mClickAction;
    private InputAction _mousePosAction;
    #endregion

    private void Awake() 
    {
        _lClickAction = _playerInput.actions[_lClickString];
        _lClickHoldAction = _playerInput.actions[_lClickHoldString];
        _rClickAction = _playerInput.actions[_rClickString];
        _mClickAction = _playerInput.actions[_mClickString];
        _mousePosAction = _playerInput.actions[_mousePosString];
    }

    private void OnEnable() 
    {
        _lClickAction.performed += OnLeftClick;
        _lClickAction.canceled += OnLeftClickRelease;
        _lClickHoldAction.performed += OnLeftClickHold;

        _rClickAction.performed += OnRightClick;
        _mClickAction.performed += OnMiddleClick;
        _mousePosAction.performed += MousePosition;
    }

    private void OnDisable() 
    {
        _lClickAction.performed -= OnLeftClick;
        _lClickAction.canceled -= OnLeftClickRelease;
        _lClickHoldAction.performed -= OnLeftClickHold;

        _rClickAction.performed -= OnRightClick;
        _mClickAction.performed -= OnMiddleClick;
        _mousePosAction.performed -= MousePosition;
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        _raycast = Camera.main.ScreenPointToRay(_mousePositionWorld);
        if(Physics.Raycast(_raycast, out _raycastHit))
        {
            if (_raycastHit.transform.gameObject.TryGetComponent(out CardScriptLoader card))
            {
                //Do something to make it clear to the player that the card has been selected
                //Make it bigger, push it up, highlight it somehow, etc etc
                _selectedCard = card;
            }

            //Might want to separate LandTiles and the others (Unit, Land) as they function quite differently
            //if (_raycastHit.transform.gameObject.TryGetComponent(out LandTile landTile))
            {
                if (_selectedCard != null && _selectedCard.GetCardType() == CardScriptableObject.Type.LAND)
                {
                    //_selectedCard.CardSO.StampObjectRef.SetActive = true;
                    //_selectedCard.HasBeenUsed = true;  
                    //landTile.heldStamp =  _selectedCard.CardSO.StampObjectRef;
                    //Need some kind of reference to the lane to know which lane to put the land tile on
                }
            }
            //else if (_raycastHit.transform.gameObject.TryGetComponent(out BoardTile boardTile)
            {
                if (_selectedCard != null && (_selectedCard.GetCardType() == CardScriptableObject.Type.UNIT
                || _selectedCard.GetCardType() == CardScriptableObject.Type.ITEM))
                {
                    //_selectedCard.CardSO.StampObjectRef.SetActive = true;
                    //_selectedCard.HasBeenUsed = true; 
                    //boardTile.heldStamp =  _selectedCard.CardSO.StampObjectRef;
                }
            }
            //else
            {
                _spriteDragScript.gameObject.SetActive(false);
                //Undo effects to _selectedCard
                _selectedCard = null;
            }
        }



        /*
        _raycast = Camera.main.ScreenPointToRay(_mousePositionWorld);
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
            }*/
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        _spriteDragScript.gameObject.SetActive(false);
        //Undo effects to _selectedCard
        _selectedCard = null;
    }

    public void OnLeftClickRelease(InputAction.CallbackContext context)
    {
        _raycast = Camera.main.ScreenPointToRay(_mousePositionWorld);
        if(Physics.Raycast(_raycast, out _raycastHit))
        {
           //if (_raycastHit.transform.gameObject.TryGetComponent(out LandTile landTile))
            {
                if (_selectedCard != null && _selectedCard.GetCardType() == CardScriptableObject.Type.LAND)
                {
                    //_selectedCard.CardSO.StampObjectRef.SetActive = true;
                    //_selectedCard.HasBeenUsed = true;  
                    //landTile.heldStamp =  _selectedCard.CardSO.StampObjectRef;
                    //Need some kind of reference to the lane to know which lane to put the land tile on
                }
            }
            //else if (_raycastHit.transform.gameObject.TryGetComponent(out BoardTile boardTile)
            {
                if (_selectedCard != null && (_selectedCard.GetCardType() == CardScriptableObject.Type.UNIT
                || _selectedCard.GetCardType() == CardScriptableObject.Type.ITEM))
                {
                    //_selectedCard.CardSO.StampObjectRef.SetActive = true;
                    //_selectedCard.HasBeenUsed = true; 
                    //boardTile.heldStamp =  _selectedCard.CardSO.StampObjectRef;
                }
            }
            //else
            {
                _spriteDragScript.gameObject.SetActive(false);
                //Undo effects to _selectedCard
                _selectedCard = null;
            }
        }
    }

    public void OnLeftClickHold(InputAction.CallbackContext context)
    {
        _raycast = Camera.main.ScreenPointToRay(_mousePositionWorld);
        if(Physics.Raycast(_raycast, out _raycastHit))
        {
            if (_raycastHit.transform.gameObject.TryGetComponent(out CardScriptLoader card))
            {
                //If OnLeftClick isn't also happening here: 
                    //Do something to make it clear to the player that the card has been selected
                    //Make it bigger, push it up, highlight it somehow, etc etc
                    _selectedCard = card;
                //Sprite _cardSprite = _selectedCard.CardSO.StampImage
                _spriteDragScript.gameObject.SetActive(true);
                //_spriteDragScript.SpriteReference = _cardSprite;
                //_spriteDragScript.IsDragging = true;
            }
        }
    }

    public void MousePosition(InputAction.CallbackContext context)
    {
        //Need to test to see if having this be checked only on "performed" is actually getting the value consistently
        //Also wtf is the z value if I passed it in as a Vector3? Can a Vector2 work anyways for the raycast?
        _mousePositionScreen = context.ReadValue<Vector2>();
        _mousePositionWorld = Camera.main.ScreenToWorldPoint(_mousePositionScreen);
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {

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
