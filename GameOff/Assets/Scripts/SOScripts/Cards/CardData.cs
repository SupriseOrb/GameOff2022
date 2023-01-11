using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "GameOff2022/CardData")]
public class CardData : ScriptableObject
{
    //Constructed from CardScriptableObject & StampScriptableObject
    #region Card Fields
    [Header("Card Values")]
    [SerializeField] private string _name = "";
    [SerializeField] [TextArea] private string _baseDescription = "";
    [SerializeField] [TextArea] private string _upgrade1Description = "";
    [SerializeField] [TextArea] private string _upgrade2Description = "";
    [SerializeField] private int _inkCost = 0;
    [SerializeField] private bool _hasBeenUsed = false;
    [SerializeField] private GameObject _stampReference;
    [SerializeField] private Type _type = Type.NONE;
    [SerializeField] private Sprite _icon;
    #endregion

    #region Card Properties
    public string CardName {get {return _name;}}
    // TODO : Figure out how we want to format the card description
        // For now, just return _cardDescription, but we might want to list stats and upgrades (if applicable)
    public string CardDescription {get {return _baseDescription;}}
    public int InkCost {get {return _inkCost;}}
    public bool HasBeenUsed {get {return _hasBeenUsed;}}
    public GameObject StampGO {get {return _stampReference;}}
    public Type CardType {get {return _type;}}
    public Sprite CardIcon {get {return _icon;}}
    #endregion
    public string CardDescriptionGivenInt(int i)
    {
        switch (i)
        {
            case 0:
                return _baseDescription;
            case 1:
                return _upgrade1Description;
            case 2:
                return _upgrade2Description;
            default:
                return "";
        }
    }
    public enum Type
    {
        NONE = 0,
        UNIT = 1,
        LAND = 2,
        ITEM = 3,
        SPELL = 4
    }

}
