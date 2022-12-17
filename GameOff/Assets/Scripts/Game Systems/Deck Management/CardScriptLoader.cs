using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class CardScriptLoader : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    //NOTE: With this design, these cards shouldn't really have functionality beyond "building" themselves aesthetically (LoadCardValues)
    [SerializeField] private BoolVariable _isHoveringUI;
    [SerializeField] CardScriptableObject _cardSO = null;
    
    [SerializeField] public CardScriptableObject.Type _cardType {get; protected set;}
    // TODO : Change to _isExhausted
    [SerializeField] private bool _hasBeenUsed;
    [SerializeField] private GameObject _selectedBorder;
    [SerializeField] private Image _cardIcon;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _inkCostText;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animationHighlight;
    [SerializeField] private string _animationIdle;

    [Header("Ink")]
    [SerializeField] private Image _inkBG;
    [SerializeField] private Sprite[] _inkBGs;
    // TODO : Update these values
    [SerializeField] private int[] _inkCostCutoff = {0, 16, 18};
    private void Start()
    {
       LoadCardValues(); 
    }

    public void LoadCardValues()
    {
        //If values are not null, set values from SO to various parts of the card's UI to effectively "build" the card thru the SO
        //Is there a need to save the values from the SO here? I don't believe we ever interact with them once set, but unsure.
            //Might need to opt to do a dict which relates the SO to the Card GO
            //...if the cards can't be easily added to/abstract enough to support simply adding the values from the SO to the UI
        _cardType = _cardSO.CardType;
        _hasBeenUsed = _cardSO.HasBeenUsed;
        _nameText.text = _cardSO.CardName;

        SetInkCost(_cardSO.InkCost);

        _cardIcon.sprite = _cardSO.CardIcon;
    }

    private void SetInkCost(int inkCost)
    {
        _inkCostText.text = "" + inkCost;

        for (int i = _inkCostCutoff.Length - 1; i > 0; i--)
        {
            if (inkCost >= _inkCostCutoff[i])
            {
                _inkBG.sprite = _inkBGs[i];
                return;
            }
        }        
    }

    public CardScriptableObject.Type CardType
    {
        get {return _cardType;}
    }

    public CardScriptableObject CardSO
    {
        get {return _cardSO;}
    }

    public bool HasBeenUsed
    {
        get {return _hasBeenUsed;}
        set {_hasBeenUsed = value;}
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            DeckManager.Instance.SelectCard(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _animator.SetBool("Highlight", true);
        _isHoveringUI.Value = true;
        AkSoundEngine.PostEvent("Play_ClicheHover", gameObject);
        DeckManager.Instance.OpenCardInfoPanel(_cardSO.CardDescription, transform.parent.localPosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animator.SetBool("Highlight", false);
        _isHoveringUI.Value = false;
        DeckManager.Instance.CloseCardInfoPanel();
    }

    public void ToggleBorder(bool isOn)
    {
        _selectedBorder.SetActive(isOn);
    }
}
