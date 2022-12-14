using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardScriptableObject", menuName = "GameOff2022/Cards/CardSO", order = 0)]
public class CardScriptableObject : ScriptableObject
{
    #region SO Backing Fields
    [Header("Card Values")]
    [SerializeField] private string _cardName = "";
    [SerializeField] [TextArea] private string _cardDescription = "";
    [SerializeField] [TextArea] private string _upgrade1Description = "";
    [SerializeField] [TextArea] private string _upgrade2Description = "";
    [SerializeField] private int _inkCost = 0;
    [SerializeField] private bool _hasBeenUsed = false;
    [SerializeField] private GameObject _stampGORef;
    [SerializeField] private Type _cardType = Type.NONE;
    [SerializeField] private Sprite _cardIcon;
    #endregion

    public enum Type
    {
        NONE = 0,
        UNIT = 1,
        LAND = 2,
        ITEM = 3,
        SPELL = 4
    }

    #region SO Getters
    public string CardName
    {
        get {return _cardName;}
    }

    public string CardDescription
    {
        get
        {
            // TODO : Figure out how we want to format the card description
            // For now, just return _cardDescription, but we might want to list stats and upgrades (if applicable)
            return _cardDescription;
        }
    }

    public string CardDescriptionGivenInt(int i)
    {
        switch (i)
        {
            case 0:
                return _cardDescription;
            case 1:
                return _upgrade1Description;
            case 2:
                return _upgrade2Description;
            default:
                return "";
        }
    }

    public int InkCost
    {
        get {return _inkCost;}
    }

    public bool HasBeenUsed
    {
        get {return _hasBeenUsed;}
    }

    public GameObject StampGO
    {
        get {return _stampGORef;}
    }

    public Type CardType
    {
        get {return _cardType;}
    }

    public Sprite CardIcon
    {
        get {return _cardIcon;}
    }
    #endregion

}
