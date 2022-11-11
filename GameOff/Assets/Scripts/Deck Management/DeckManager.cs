using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private List<int> _test;
    [SerializeField] private List<GameObject> _activeDeck; //Holds all active cards
    [SerializeField] private List<GameObject> _discardDeck; //Holds all discarded cards
    [SerializeField] private List<GameObject> _cardHolders; //Holds card "slots" in UI (?) //Could hold transforms instead as well
    [SerializeField] private List<GameObject> _cardHand; //Holds all cards in hand (filling the card "slots")
    [SerializeField] private int _drawNumber; //How many cards to draw (how many cards we want in hand at a time)
    [SerializeField] private float _baseReshuffleTimer; //Base time till hand is reset
    [SerializeField] private float _currentReshuffleTimer; //Current time till hand is reset
    [SerializeField] private bool _isInWave = false; //Is there currently a wave happening? //Does this belong here? Should this be a SO bool?

    private static System.Random r = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        //Might want to initialize the decks here, like adding cards and setting up the hand for the tutorial and such
    }

    void Update()
    {
        //if (_currentReshuffleTimer <= 0)
        //{
        //ShuffleDeck();
        //DrawCards();
        //}
        //_currentReshuffleTimer = _baseReshuffleTimer
        //_currentReshuffleTimer -= Time.DeltaTime;
    }

    private void ShuffleDeck()
    {
        //If (_isInWave):
            //For each card in _cardHand, if it is Land type, if _hasBeenUsed is true, add it to _discardDeck and remove it from _activeDeck
            //Remove all items in _cardHand and [set all bools in _cardHolders to false]
            //Shuffle _activeDeck ==> ShuffleHelper(_activeDeck);
        //If (!_isInWave):
            //If _discardDeck >= 1
                //For each card in _discardDeck, add it to _activeDeck and remove it from _discardDeck
            //Remove all items in _cardHand and [set all bools in _cardHolders to false]
            //Shuffle _activeDeck ==> ShuffleHelper(_activeDeck); 
    }

    private void DrawCards()
    {
        //Choose [4] random cards from _activeDeck, add them to _cardHand (and _cardHolders), then activate and move them to the correct pos
            //GameObject[] randomCards = new Gameobject[4];
            //for (i = 0; i < _drawNumber; i++)
                //randomCards[i] = _activeDeck[Random.Range(0, _activeDeck.Count)];
                //_cardHand[i] = randomCards[i];
                //_cardHand[i].SetActive(true);
                //_cardHand[i].transform.position = _cardHolders[i].position;

                //Might want to have a bool check for each _cardHolders slot to know if it is being used or not. This SHOULDN'T matter,
                //...as we always draw _drawNumber amt of cards, and thus should never be worried that there aren't enough slots open
                //...but if we ever decide we discard cards between cycles, or draw cards, or get an extra card, it might matter.
                //The way to go about this is likely a dict between two lists (GOs, Bools), since tuples don't serialize in Inspector.

                //Make the above a helper function?
                //Personally don't like how the last 3 lines are part of this function, as they almost relate purely to the visuals. Should we 
                //...make a separate function to handle them?

        //Might? Need limitations on what types of cards this can draw. Needs playtesting.
    }

    private void ShuffleHelper<T>(List<T> deck)
    {
        for (int y = deck.Count - 1; y > 0; --y)
        {
            int x = r.Next(y+1);
            var temp = deck[y];
            deck[y] = deck[x];
            deck[x] = temp;
        }
    }
}
