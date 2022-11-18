using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScriptLoader : MonoBehaviour
{
    //NOTE: With this design, these cards shouldn't really have functionality beyond "building" themselves aesthetically (LoadCardValues)
    [SerializeField] CardScriptableObject _cardSO = null;
    
    [SerializeField] public CardScriptableObject.Type _cardType {get; protected set;}
    [SerializeField] public bool _hasBeenUsed;
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

    public void OpenCardInfoPanel()
    {
        DeckManager.Instance.OpenCardInfoPanel(_cardSO.CardDescription, transform.localPosition);
    }

    public void OnPointerClick()
    {
        DeckManager.Instance.SelectCard(this);
    }
}
