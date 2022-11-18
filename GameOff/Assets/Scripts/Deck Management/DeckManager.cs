using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckManager : MonoBehaviour
{
    [Header("Cards")]
    [SerializeField] private List<GameObject> _activeDeck; //Holds all active cards
    [SerializeField] private Transform _activeDeckTransform; //Location of the deck
    [SerializeField] private List<GameObject> _discardDeck; //Holds all discarded cards
    [SerializeField] private Transform[] _cardHolders; //Holds positions for each card in _cardHand
    [SerializeField] private GameObject[] _cardHand; //Holds all cards in hand (filling the card "slots")

    [SerializeField] private CardScriptLoader _selectedCard;
    [SerializeField] private BoolVariable _isLeftClick;

    [Header("Card Info Panel")]
    [SerializeField] private GameObject _cardInfoPanel;
    [SerializeField] private TextMeshProUGUI _cardInfoDescription;
    [SerializeField] private Vector3 _cardInfoPanelOffset;

    [Header("Deck Vars")]
    [SerializeField] private float _firstDrawDelay;
    [SerializeField] private int _drawNumber; //How many cards to draw (how many cards we want in hand at a time)
    [SerializeField] private float _baseReshuffleTimer; //Base time till hand is reset
    [SerializeField] private float _currentReshuffleTimer; //Current time till hand is reset
    [SerializeField] private bool _isInWave = false; //Is there currently a wave happening? //Does this belong here? Should this be a SO bool?
    [SerializeField] private bool _isFirstDraw = false; //Is this the first draw of the wave?

    [Header("Timer")]
    [SerializeField] private Slider _timerSlider;
    [SerializeField] private TextMeshProUGUI _timerText;
    
    [Header("Animation")]
    [SerializeField] private Animator _cardFrameAnimator;
    [SerializeField] private string _cardFrameOpenString = "CardFrame_Open";
    [SerializeField] private string _cardFrameCloseString = "CardFrame_Close";
    [SerializeField] private string _cardFrameIdleOpenString = "CardFrame_IdleOpen";
    [SerializeField] private string _cardFrameIdleCloseString = "CardFrame_IdleClose";

    private static System.Random r = new System.Random();
    private static DeckManager _instance;

    public static DeckManager Instance
    {
        get{return _instance;}
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        // TODO: Might want to initialize the decks here, like adding cards and setting up the hand for the tutorial and such
        // Note: Need slight delay in order to get the correct _cardHolders transforms 
        StartCoroutine(DelayDrawCards());
    }

    private IEnumerator DelayDrawCards()
    {
        yield return new WaitForSeconds(_firstDrawDelay);
        DrawCards();
    }
    
    private void FixedUpdate() 
    {
        if (!_isFirstDraw)
        {
            if (_currentReshuffleTimer <= 0)
            {
                CycleHand();
                DrawCards();
                _currentReshuffleTimer = _baseReshuffleTimer;
                AkSoundEngine.PostEvent("Cliche_HandCycle", gameObject);
            }
            _currentReshuffleTimer -= Time.deltaTime;    
            _timerText.text = ((int)_currentReshuffleTimer).ToString() + "s";
            _timerSlider.value = _currentReshuffleTimer/_baseReshuffleTimer;
            /* TODO: Figure out how to trigger the timer sound when the timer reads 3 seconds
            if (_currentReshuffletimer == 3f);
            {
                AkSoundEngine.PostEvent("Timer", gameObject);
            }*/
        }
        
    }

    private void CycleHand()
    {
        if(!_isInWave)
        {
            _isInWave = true;
            foreach(GameObject card in _discardDeck)
            {
                CardScriptLoader loader = card.GetComponent<CardScriptLoader>();
                loader._hasBeenUsed = false;
                _activeDeck.Add(card);
                card.transform.position = _activeDeckTransform.position;
            }
            _discardDeck.Clear();
        }

        foreach(GameObject card in _cardHand)
        {
            CardScriptLoader loader = card.GetComponent<CardScriptLoader>();
            if(loader.CardType == CardScriptableObject.Type.LAND && loader._hasBeenUsed == true)
            {
                _discardDeck.Add(card);
                _activeDeck.Remove(card);
            }
            //If card is a unit, put it back in the top 3 cards in the deck list
            else if(loader.CardType == CardScriptableObject.Type.UNIT)
            {
                _activeDeck.Insert(0, card);
            }
            //Otherwise, put card at the bottom of the deck
            else
            {
                _activeDeck.Add(card);
            }
            card.transform.position = _activeDeckTransform.position;
        }
    }

    private void DrawCards()
    {
        if(_isFirstDraw)
        {
            _isFirstDraw = false;
            //Draw 3 units and then a random card from the deck
            for(int i = 0; i < _cardHand.Length - 1; i++)
            {
                int randomNum = Random.Range(0, 2 - i);
                _cardHand[i] = _activeDeck[randomNum];
                _activeDeck.RemoveAt(randomNum);
                //replace w/ animation later
                _cardHand[i].transform.position = _cardHolders[i].transform.position;
            }
            //Debug.Log("Deck Length - 1: " + (_activeDeck.Count -1));
            int j = Random.Range(0, _activeDeck.Count);
            //Debug.Log("j: " + j);
            _cardHand[3] = _activeDeck[j];
            _activeDeck.RemoveAt(j);
            _cardHand[3].transform.position = _cardHolders[3].transform.position;
        }
        else
        {
            for(int i = 0; i < _cardHand.Length; i++)
            {
                int randomNum = Random.Range(0, _activeDeck.Count);
                _cardHand[i] = _activeDeck[randomNum];
                _activeDeck.RemoveAt(randomNum);
                _cardHand[i].transform.position = _cardHolders[i].position;

            }
        }
    }

    public void ToggleInWave()
    {
        _isInWave = !_isInWave;
    }

    public void IsFirstDraw()
    {
        _isFirstDraw = true;
    }

    public void ToggleCardFrameVisibility()
    {
        if (_cardFrameAnimator.GetCurrentAnimatorStateInfo(0).IsName(_cardFrameCloseString) ||
            _cardFrameAnimator.GetCurrentAnimatorStateInfo(0).IsName(_cardFrameIdleCloseString))
        {
            _cardFrameAnimator.Play(_cardFrameOpenString);
            AkSoundEngine.PostEvent("Play_UISelect", this.gameObject);
        }
        else if (_cardFrameAnimator.GetCurrentAnimatorStateInfo(0).IsName(_cardFrameOpenString) ||
                _cardFrameAnimator.GetCurrentAnimatorStateInfo(0).IsName(_cardFrameIdleOpenString))
        {
            _cardFrameAnimator.Play(_cardFrameCloseString);
            AkSoundEngine.PostEvent("Play_UIBack", this.gameObject);
        }
    }
    public void OpenCardInfoPanel(string description, Vector2 localPosition)
    {
        _cardInfoDescription.text = description;
        _cardInfoPanel.transform.localPosition = localPosition;
        _cardInfoPanel.transform.localPosition += _cardInfoPanelOffset;
        _cardInfoPanel.SetActive(true);
    }
    public void CloseCardInfoPanel()
    {
        _cardInfoPanel.SetActive(false);
    }

    /*
        There are 3 scenarios:
        [1] No card is selected, selecting for the first time
        [2] A card is selected, but we're selecting a different card
        [3] A card is selected, and we're selecting the same card. Do nothing
    */
    public void SelectCard(CardScriptLoader cardToSelect)
    {   
        if (!_isLeftClick.Value)
        {
            return;
        }   

        //Scenario 1
        if (_selectedCard == null)
        {
            AkSoundEngine.PostEvent("Play_UISelect", this.gameObject);
            _selectedCard = cardToSelect;
            Debug.Log("Select Card: Scenario 1; Select " + cardToSelect.name);
        }
        else
        {
            // Scenario 2
            if (_selectedCard != cardToSelect)
            {
                AkSoundEngine.PostEvent("Play_UISelect", this.gameObject);
                ResetCardSelection();
                _selectedCard = cardToSelect;
                Debug.Log("Select Card: Scenario 2; Select " + cardToSelect.name);
            }
        }
    }

    public void DeselectCard()
    {
        if (_selectedCard != null)
        {
            AkSoundEngine.PostEvent("Play_UIBack", this.gameObject);
            ResetCardSelection();
            Debug.Log("Deselect card");
        }
    }

    private void ResetCardSelection()
    {
        _selectedCard = null;
    }
}
