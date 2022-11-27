using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    private Vector3 _mousePositionScreen;
    [SerializeField] private Vector3 _mousePositionWorld;
    [SerializeField] private CardScriptLoader _selectedCard;
    [SerializeField] private SpriteDrag _spriteDrag;
    [SerializeField] private LayerMask _layerMask;
    private Ray2D _raycast;
    private RaycastHit2D _raycastHit;

    public Vector3 MousePositionWorld
    {
        get {return _mousePositionWorld;}
    }

    #region Input Action Strings
    [Header("Input Strings")]
    [SerializeField] private string _lClickString;
    [SerializeField] private string _rClickString;
    [SerializeField] private string _mousePosString;
    #endregion

    #region Input Actions
    private InputAction _lClickAction;
    private InputAction _rClickAction;
    private InputAction _mousePosAction;
    #endregion

    [Header("Error Message")]
    [SerializeField] private Animator _errorMessagePanelAnimator;
    [SerializeField] private string _openErrorMessagePanelAnim = "ErrorMessage_Appear";
    [SerializeField] private TextMeshProUGUI _errorMessageTextBox;
    [SerializeField] [TextArea] private string _errorNotEnoughInk = "Cannot Stamp, not enough ink";
    [SerializeField] [TextArea] private string _stampOutsideGrid = "Cannot stamp outside of the grid";
    [SerializeField] [TextArea] private string _stampUnknownOnUnit = "Cannot stamp items / lands on allied unit tiles";
    [SerializeField] [TextArea] private string _stampUnitOnBoardTile = "Cannot stamp allied unit outside of unit tiles";
    private void Awake() 
    {
        _lClickAction = _playerInput.actions[_lClickString];
        _rClickAction = _playerInput.actions[_rClickString];
        _mousePosAction = _playerInput.actions[_mousePosString];
    }

    private void OnEnable() 
    {
        _lClickAction.performed += OnLeftClick;
        _lClickAction.canceled += OnLeftClick; // To implement card dragging

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
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        _selectedCard = DeckManager.Instance.SelectedCard;
        _raycastHit = Physics2D.Raycast(_mousePositionWorld, _mousePositionWorld, 100f, _layerMask);

        if (_raycastHit.collider == null) //If no collider on click
        {
            if (_selectedCard != null && context.performed)
            {
                AnnounceError(_stampOutsideGrid, context.performed);
            }
            // Deselect when clicking outside of the tiles
            BoardManager.Instance.HideTileInfo();
            return;
        }

        if (_spriteDrag.IsDragging)
        {            
            if (_selectedCard != null)
            {
                if (_raycastHit.transform.gameObject.TryGetComponent(out UnitTile unitTile)) //If drag is released on UnitTile / UnitTile clicked
                {
                    if (_selectedCard.CardSO.CardType == CardScriptableObject.Type.UNIT)
                    {
                        if(DeckManager.Instance.RemoveInk(_selectedCard.CardSO.InkCost))
                        {
                            unitTile.SetHeldStamp(_selectedCard.CardSO.StampGO);
                            Debug.Log("Placed Unit " + _selectedCard + " onto UnitTile!");
                        }
                        else
                        {
                            AnnounceError(_errorNotEnoughInk, context.performed);
                        }
                        DeckManager.Instance.ResetCardSelection();
                    }
                    else if (_selectedCard.CardSO.CardType == CardScriptableObject.Type.SPELL)
                    {
                        unitTile.PlaySpell(_selectedCard.CardSO.StampGO);
                        Debug.Log("Placed Spell " + _selectedCard + " onto the item!");
                        DeckManager.Instance.ResetCardSelection(); 
                    }
                    else
                    {
                        AnnounceError(_stampUnknownOnUnit, context.performed);
                    }
                    
                }
                else if (_raycastHit.transform.gameObject.TryGetComponent(out BoardTile boardTile)) //If drag is released on BoardTile / BoardTile clicked
                {
                    if (_selectedCard.CardSO.CardType == CardScriptableObject.Type.ITEM)
                    {
                        if(DeckManager.Instance.RemoveInk(_selectedCard.CardSO.InkCost))
                        {
                            boardTile.SetHeldStamp(_selectedCard.CardSO.StampGO);
                            Debug.Log("Placed Item " + _selectedCard + " onto BoardTile!");
                        }
                        else
                        {
                            AnnounceError(_errorNotEnoughInk, context.performed);
                        }
                        DeckManager.Instance.ResetCardSelection();
                    }
                    else if (_selectedCard.CardSO.CardType == CardScriptableObject.Type.LAND)
                    {
                        if(DeckManager.Instance.RemoveInk(_selectedCard.CardSO.InkCost))
                        {
                            BoardLane boardLane = BoardManager.Instance.GetLane(boardTile.LaneNumber);
                            boardLane.ApplyLandStamp(_selectedCard.CardSO.StampGO);
                            _selectedCard.HasBeenUsed = true;
                            Debug.Log("Placed Land " + _selectedCard + " onto BoardLane!");
                        }
                        else
                        {
                            AnnounceError(_errorNotEnoughInk, context.performed);
                        }
                        DeckManager.Instance.ResetCardSelection();
                    }
                    else if (_selectedCard.CardSO.CardType == CardScriptableObject.Type.SPELL)
                    {
                        boardTile.PlaySpell(_selectedCard.CardSO.StampGO);
                        Debug.Log("Placed Spell " + _selectedCard + " onto the item!");
                        DeckManager.Instance.ResetCardSelection(); 
                    }
                    else
                    {
                        AnnounceError(_stampUnitOnBoardTile, context.performed);
                    }
                }
            }          
        }
        else if (context.performed) // Only need to do this check once when performed (canceled doesn't matter)
        {
            if (_selectedCard == null && _raycastHit.transform.gameObject.TryGetComponent(out BoardTile boardTile))
            {
                if (_raycastHit.transform.gameObject.TryGetComponent(out UnitTile unitTile))
                {
                    unitTile.Clicked();
                }
                else
                {
                    boardTile.Clicked();
                }
            }     
        }
    }

    public void MousePosition(InputAction.CallbackContext context)
    {
        _mousePositionScreen = context.ReadValue<Vector2>();
        _mousePositionWorld = Camera.main.ScreenToWorldPoint(_mousePositionScreen);
        _spriteDrag.Move(new Vector3(_mousePositionWorld.x, _mousePositionWorld.y, 0f));
    }

    private void AnnounceError(string errorMessage, bool isOnPerformed)
    {
        if (!isOnPerformed)
        {
            return;
        }

        // COLLIN TODO: Play error message sfx
        AkSoundEngine.PostEvent("Play_Placeholder", gameObject);
        _errorMessageTextBox.text = errorMessage;
        _errorMessagePanelAnimator.Play(_openErrorMessagePanelAnim);
        if (errorMessage == _errorNotEnoughInk)
        {
            // TODO: Make the ink bar flash w/ a shader
        }
    }
}
