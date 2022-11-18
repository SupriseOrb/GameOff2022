using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardScriptLoader : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //NOTE: With this design, these cards shouldn't really have functionality beyond "building" themselves aesthetically (LoadCardValues)
    [SerializeField] CardScriptableObject _cardSO = null;
    
    [SerializeField] public CardScriptableObject.Type _cardType {get; protected set;}
    [SerializeField] public bool _hasBeenUsed;
    [SerializeField] private GameObject _selectedBorder;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animationHighlight;
    [SerializeField] private string _animationIdle;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            DeckManager.Instance.SelectCard(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _animator.Play(_animationHighlight);
        AkSoundEngine.PostEvent("Play_ClicheHover", gameObject);
        DeckManager.Instance.OpenCardInfoPanel(_cardSO.CardDescription, transform.localPosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animator.Play(_animationIdle);
        DeckManager.Instance.CloseCardInfoPanel();
    }

    public void ToggleBorder(bool isOn)
    {
        _selectedBorder.SetActive(isOn);
    }
}
