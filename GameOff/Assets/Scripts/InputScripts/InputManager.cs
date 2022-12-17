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
    [SerializeField] private SpriteDrag _spriteDrag;
    // Note : this is a Tile Layer Mask
    [SerializeField] private LayerMask _layerMask;

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
    [SerializeField] private BoolVariable _isHoveringUI;
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
        CardScriptLoader selectedCard = DeckManager.Instance.SelectedCard;
        RaycastHit2D raycastHit = Physics2D.Raycast(_mousePositionWorld, _mousePositionWorld, 100f, _layerMask);

        if (raycastHit.collider == null) //If no collider on click
        {
            if (selectedCard != null && !_isHoveringUI.Value)
            {
                AnnounceError(_stampOutsideGrid);
                DeckManager.Instance.ResetCardSelection();
            }
            // Deselect when clicking outside of the tiles
            BoardManager.Instance.HideTileInfo();
            return;
        }

        if (selectedCard != null)
        {            
            if (raycastHit.transform.gameObject.TryGetComponent(out UnitTile unitTile)) //If drag is released on UnitTile / UnitTile clicked
            {
                if (selectedCard.CardSO.CardType == CardScriptableObject.Type.UNIT)
                {
                    if(DeckManager.Instance.RemoveInk(selectedCard.CardSO.InkCost))
                    {
                        unitTile.SetHeldStamp(selectedCard.CardSO.StampGO);
                    }
                    else
                    {
                        AnnounceError(_errorNotEnoughInk);
                    }
                }
                else if (selectedCard.CardSO.CardType == CardScriptableObject.Type.SPELL)
                {
                    unitTile.PlaySpell(selectedCard.CardSO.StampGO);
                }
                else
                {
                    AnnounceError(_stampUnknownOnUnit);
                }
                
            }
            else if (raycastHit.transform.gameObject.TryGetComponent(out BoardTile boardTile)) //If drag is released on BoardTile / BoardTile clicked
            {
                if (selectedCard.CardSO.CardType == CardScriptableObject.Type.ITEM)
                {
                    if(DeckManager.Instance.RemoveInk(selectedCard.CardSO.InkCost))
                    {
                        boardTile.SetHeldStamp(selectedCard.CardSO.StampGO);
                    }
                    else
                    {
                        AnnounceError(_errorNotEnoughInk);
                    }
                }
                else if (selectedCard.CardSO.CardType == CardScriptableObject.Type.LAND)
                {
                    if(DeckManager.Instance.RemoveInk(selectedCard.CardSO.InkCost))
                    {
                        BoardLane boardLane = BoardManager.Instance.GetLane(boardTile.LaneNumber);
                        boardLane.ApplyLandStamp(selectedCard.CardSO.StampGO);
                        selectedCard.HasBeenUsed = true;
                    }
                    else
                    {
                        AnnounceError(_errorNotEnoughInk);
                    }
                }
                else if (selectedCard.CardSO.CardType == CardScriptableObject.Type.SPELL)
                {
                    boardTile.PlaySpell(selectedCard.CardSO.StampGO);
                }
                else
                {
                    AnnounceError(_stampUnitOnBoardTile);
                }
            }
            DeckManager.Instance.ResetCardSelection();
        }
        else if (context.performed) // Only need to do this check once when performed (canceled doesn't matter)
        {
            if (selectedCard == null && raycastHit.transform.gameObject.TryGetComponent(out BoardTile boardTile))
            {
                if (raycastHit.transform.gameObject.TryGetComponent(out UnitTile unitTile))
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

    private void AnnounceError(string errorMessage)
    {
        if (_isHoveringUI.Value)
        {
            return;
        }

        AkSoundEngine.PostEvent("Play_ErrorMessage", gameObject);
        _errorMessageTextBox.text = errorMessage;
        _errorMessagePanelAnimator.Play(_openErrorMessagePanelAnim, -1, 0f);
        if (errorMessage == _errorNotEnoughInk)
        {
            // TODO : Make the ink bar flash w/ a shader
        }
    }
}
