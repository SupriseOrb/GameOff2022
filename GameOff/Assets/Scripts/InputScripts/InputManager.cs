using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private GameObject _heldObject = null;
    private Vector3 _mousePositionScreen;
    [SerializeField] private Vector3 _mousePositionWorld;
    [SerializeField] private CardScriptLoader _selectedCard;
    [SerializeField] private SpriteDrag _spriteDragScript;
    private Ray2D _raycast;
    private RaycastHit2D _raycastHit;

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

    //TODO: Need to check before clicking if _selectedCard is not null. If so, deselect the other card and update 
    //..._selectedCard to current card
    public void OnLeftClick(InputAction.CallbackContext context)
    {
        _raycastHit = Physics2D.Raycast(_mousePositionWorld, _mousePositionWorld, 100f);
        if (_raycastHit.transform.gameObject.TryGetComponent(out CardScriptLoader card))
        {
            Debug.Log("Clicked Card!");
            //Do something to make it clear to the player that the card has been selected
            //Make it bigger, push it up, highlight it somehow, etc etc
            _selectedCard = card;
            _selectedCard.transform.localScale = (Vector3.one * 2); //Selected Indicator (for now)
        }
        else if (_raycastHit.transform.gameObject.TryGetComponent(out UnitTile unitTile))
        {
            if (_selectedCard != null && _selectedCard.GetCardType() == CardScriptableObject.Type.UNIT)
            {
                UnitStampScriptableObject unitSO = (UnitStampScriptableObject)_selectedCard.GetCardSO.StampObjectRef;
                unitSO.SpawnedUnit.SetActive(true);
                _selectedCard.HasBeenUsed = true;  
                unitTile.SetHeldStamp(unitSO.SpawnedUnit);
                ResetCardSelection();
            }
        }
        else if (_raycastHit.transform.gameObject.TryGetComponent(out BoardTile boardTile))
        {
            if (_selectedCard != null)
            {
                if (_selectedCard.GetCardType() == CardScriptableObject.Type.ITEM)
                {   
                    ItemStampScriptableObject itemSO = (ItemStampScriptableObject)_selectedCard.GetCardSO.StampObjectRef;
                    itemSO.SpawnedItem.SetActive(true);
                    _selectedCard.HasBeenUsed = true; 
                    boardTile.SetHeldStamp(itemSO.SpawnedItem);
                    ResetCardSelection();
                }
                else if (_selectedCard.GetCardType() == CardScriptableObject.Type.LAND)
                {
                    LandStampScriptableObject itemSO = (LandStampScriptableObject)_selectedCard.GetCardSO.StampObjectRef;
                    itemSO.SpawnedLand.SetActive(true);
                    _selectedCard.HasBeenUsed = true; 
                    boardTile.SetHeldStamp(itemSO.SpawnedLand);
                    ResetCardSelection();
                }
            }
                
        }
        else
        {
            ResetCardSelection();
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

    public void OnRightClick(InputAction.CallbackContext context)
    {
        ResetCardSelection();
    }

    public void OnLeftClickRelease(InputAction.CallbackContext context)
    {
        _raycastHit = Physics2D.Raycast(_mousePositionWorld, _mousePositionWorld, 100f);
        //TODO: Apparently error here if you left-click while "dragging" a tile
        if (_raycastHit.transform.gameObject.TryGetComponent(out UnitTile unitTile))
        {
            if (_selectedCard != null && _selectedCard.GetCardType() == CardScriptableObject.Type.UNIT)
            {
                UnitStampScriptableObject unitSO = (UnitStampScriptableObject)_selectedCard.GetCardSO.StampObjectRef;
                unitSO.SpawnedUnit.SetActive(true);
                _selectedCard.HasBeenUsed = true;  
                unitTile.SetHeldStamp(unitSO.SpawnedUnit);
                ResetCardSelection();
            }
        }
        else if (_raycastHit.transform.gameObject.TryGetComponent(out BoardTile boardTile))
        {
            if (_selectedCard != null)
            {
                if (_selectedCard.GetCardType() == CardScriptableObject.Type.ITEM)
                {   
                    ItemStampScriptableObject itemSO = (ItemStampScriptableObject)_selectedCard.GetCardSO.StampObjectRef;
                    itemSO.SpawnedItem.SetActive(true);
                    _selectedCard.HasBeenUsed = true; 
                    boardTile.SetHeldStamp(itemSO.SpawnedItem);
                    ResetCardSelection();
                }
                else if (_selectedCard.GetCardType() == CardScriptableObject.Type.LAND)
                {
                    LandStampScriptableObject itemSO = (LandStampScriptableObject)_selectedCard.GetCardSO.StampObjectRef;
                    itemSO.SpawnedLand.SetActive(true);
                    _selectedCard.HasBeenUsed = true; 
                    boardTile.SetHeldStamp(itemSO.SpawnedLand);
                    ResetCardSelection();
                }      
            }
                
        }
        else if (_selectedCard != null && _raycastHit.transform.gameObject.TryGetComponent(out CardScriptLoader card)) 
        {       
            //Done to prevent initial a perpetual loop of LClick picking up card ==> LClickRelease dropping it
            Debug.Log("No interaction needed!");
            return;
        }
        else
        {
            ResetCardSelection();
        }

    }

    public void OnLeftClickHold(InputAction.CallbackContext context)
    {
        Debug.Log("Held Card!");
        _raycastHit = Physics2D.Raycast(_mousePositionWorld, _mousePositionWorld, 100f);
        if (_raycastHit.transform.gameObject.TryGetComponent(out CardScriptLoader card))
        {
            //Note: OnLeftClick is also being called if OnLeftClickHold is
            Sprite _cardSprite = _selectedCard.GetCardSO.StampObjectRef.StampSprite;
            Debug.Log(_cardSprite);
            _spriteDragScript.gameObject.SetActive(true);
            _spriteDragScript.SpriteReference = _cardSprite;
            _spriteDragScript.IsDragging = true;
        }
    }

    //TODO: Use MousePosition to drag the card instead of having a requirement of holding left-click down on a card
    //Not sure how to do this though? Once you click on a card, if you move your mouse, how would you know to drag vs just click on a tile?
    public void MousePosition(InputAction.CallbackContext context)
    {
        //Need to test to see if having this be checked only on "performed" is actually getting the value consistently
        _mousePositionScreen = context.ReadValue<Vector2>();
        _mousePositionWorld = Camera.main.ScreenToWorldPoint(_mousePositionScreen);
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {

    }

    public void ResetCardSelection()
    {
        _spriteDragScript.IsDragging = false;
        _spriteDragScript.gameObject.SetActive(false);
        if (_selectedCard != null)
        {
            //Undo effects to _selectedCard
            _selectedCard.transform.localScale = Vector3.one;
            _selectedCard = null;
        }  
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
