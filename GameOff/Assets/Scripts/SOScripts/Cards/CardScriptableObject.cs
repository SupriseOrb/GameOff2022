using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardScriptableObject", menuName = "GameOff2022/Cards/CardSO", order = 0)]
public class CardScriptableObject : ScriptableObject
{
    #region SO Backing Fields
    [Header("Card Values")]
    [SerializeField] private string _cardName = "";
    [SerializeField] private string _cardDescription = "";
    [SerializeField] private int _inkCost = 0;
    [SerializeField] private bool _hasBeenUsed = false;
    [SerializeField] private StampScriptableObject _stampObjectRef = null;
    [SerializeField] private Type _cardType = Type.NONE;
    #endregion

    public enum Type
    {
        NONE = 0,
        UNIT = 1,
        LAND = 2,
        ITEM = 3
    }

    #region SO Getters
    public string CardName
    {
        get {return _cardName;}
    }

    public string CardDescription
    {
        get {return _cardDescription;}
    }

    public int InkCost
    {
        get {return _inkCost;}
    }

    public bool HasBeenUsed
    {
        get {return _hasBeenUsed;}
    }

    public StampScriptableObject StampObjectRef
    {
        get {return _stampObjectRef;}
    }

    public Type CardType
    {
        get {return _cardType;}
    }
    #endregion

}
