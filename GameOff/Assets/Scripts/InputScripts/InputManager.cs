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
    [SerializeField] private SpriteDrag _spriteDrag;
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
    [SerializeField] private string _mousePosString;
    #endregion

    #region Input Actions
    private InputAction _lClickAction;
    private InputAction _lClickHoldAction;
    private InputAction _rClickAction;
    private InputAction _mousePosAction;
    #endregion

    private void Awake() 
    {
        _lClickAction = _playerInput.actions[_lClickString];
        _lClickHoldAction = _playerInput.actions[_lClickHoldString];
        _rClickAction = _playerInput.actions[_rClickString];
        _mousePosAction = _playerInput.actions[_mousePosString];
    }

    private void OnEnable() 
    {
        _lClickAction.performed += OnLeftClick;
        _lClickAction.canceled += OnLeftClick;

        _rClickAction.performed += OnRightClick;
        _mousePosAction.performed += MousePosition;
    }

    private void OnDisable() 
    {
        _lClickAction.performed -= OnLeftClick;
        _lClickAction.canceled -= OnLeftClick;

        _rClickAction.performed -= OnRightClick;
        _mousePosAction.performed -= MousePosition;
    }

    public void OnRightClick(InputAction.CallbackContext context)
    { 
        DeckManager.Instance.DeselectCard();
        //ResetCardSelection();
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        _selectedCard = DeckManager.Instance.SelectedCard;
        if (_spriteDrag.IsDragging && _selectedCard != null) //Need to figure out how we're getting _selectedCard
        {
            _raycastHit = Physics2D.Raycast(_mousePositionWorld, _mousePositionWorld, 100f);
            if (_raycastHit.collider == null) //If no collider on click
            {
                return;
            }

            if (_raycastHit.transform.gameObject.TryGetComponent(out UnitTile unitTile)) //If drag is released on UnitTile / UnitTile clicked
            {
                if (_selectedCard.CardSO.CardType == CardScriptableObject.Type.UNIT)
                {
                    unitTile.SetHeldStamp(_selectedCard.CardSO.StampGO);
                    Debug.Log("Placed Unit " + _selectedCard + " onto UnitTile!");
                    DeckManager.Instance.ResetCardSelection();
                }
                
            }
            else if (_raycastHit.transform.gameObject.TryGetComponent(out BoardTile boardTile)) //If drag is released on BoardTile / BoardTile clicked
            {
                if (_selectedCard.CardSO.CardType == CardScriptableObject.Type.ITEM)
                {
                    boardTile.SetHeldStamp(_selectedCard.CardSO.StampGO);
                    Debug.Log("Placed Item " + _selectedCard + " onto BoardTile!");
                    DeckManager.Instance.ResetCardSelection();
                }
                else if (_selectedCard.CardSO.CardType == CardScriptableObject.Type.LAND)
                {
                    BoardLane boardLane = BoardManager.Instance.GetLane(boardTile._laneNumber);
                    boardLane.ApplyLandStamp(_selectedCard.CardSO.StampGO);
                    Debug.Log("Placed Land " + _selectedCard + " onto BoardLane!");
                    DeckManager.Instance.ResetCardSelection();
                }
            }
        }
    }

    public void MousePosition(InputAction.CallbackContext context)
    {
        //Need to test to see if having this be checked only on "performed" is actually getting the value consistently
        _mousePositionScreen = context.ReadValue<Vector2>();
        _mousePositionWorld = Camera.main.ScreenToWorldPoint(_mousePositionScreen);
        _spriteDrag.Move(_mousePositionWorld);
    }

    /*
    TODO:
    Set up all the inputs to work for the tiles exclusively, with the cards working thru inspector calls + DeckManager
    [1] Left Click (OnClick)
    - If you click a tile, if _selectedCard isn't null, setStamp(_selectedCard)
    - setStamp should take the GO referenced in the SO in _selectedCard and activate it and move its position/instantiate it on the tile
    - Note: this will need different functionality for the land stamps
    [2] Drag
    - HahahahahahaHAhahahahaHAHAHahHAhah no
    */

    //TODO: Need to check before clicking if _selectedCard is not null. If so, deselect the other card and update 
    //..._selectedCard to current card
    /*public void OnLeftClick(InputAction.CallbackContext context)
    {
        _raycastHit = Physics2D.Raycast(_mousePositionWorld, _mousePositionWorld, 100f);
        if (_raycastHit.collider == null)
        {
            return;
        }

        if (_raycastHit.transform.gameObject.TryGetComponent(out UnitTile unitTile))
        {
            if (_selectedCard != null && _selectedCard.CardType == CardScriptableObject.Type.UNIT)
            {
                UnitStampScriptableObject unitSO = (UnitStampScriptableObject)_selectedCard.CardSO.StampObjectRef;
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
                if (_selectedCard.CardType == CardScriptableObject.Type.ITEM)
                {   
                    ItemStampScriptableObject itemSO = (ItemStampScriptableObject)_selectedCard.CardSO.StampObjectRef;
                    itemSO.SpawnedItem.SetActive(true);
                    _selectedCard.HasBeenUsed = true; 
                    boardTile.SetHeldStamp(itemSO.SpawnedItem);
                    ResetCardSelection();
                }
                else if (_selectedCard.CardType == CardScriptableObject.Type.LAND)
                {
                    LandStampScriptableObject itemSO = (LandStampScriptableObject)_selectedCard.CardSO.StampObjectRef;
                    itemSO.SpawnedLand.SetActive(true);
                    _selectedCard.HasBeenUsed = true; 
                    boardTile.SetHeldStamp(itemSO.SpawnedLand);
                    ResetCardSelection();
                }
            }
                
        }

    }*/

    

  
        /*//move isdraggingcard to bottom of function because we want to use this as a check
        _isDraggingCard = false;
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
        }*/

    

    public void OnLeftClickHold(InputAction.CallbackContext context)
    {
        /*
        _isDraggingCard = true;
        //Note: OnLeftClick is also being called if OnLeftClickHold is
        Sprite _cardSprite = _selectedCard.GetCardSO.StampObjectRef.StampSprite;
        Debug.Log(_cardSprite);
        _spriteDragScript.gameObject.SetActive(true);
        _spriteDragScript.SpriteReference = _cardSprite;
        _spriteDragScript.IsDragging = true;
        */
    }

}
