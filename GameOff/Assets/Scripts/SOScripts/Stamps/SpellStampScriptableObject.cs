using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellStampScriptableObject", menuName = "GameOff2022/Stamps/SpellStampSO", order = 3)]
public class SpellStampScriptableObject : StampScriptableObject
{
    [SerializeField] private float _spellStampAbilityValue;

    public float SpellAbilityValue
    {
        get {return _spellStampAbilityValue;}
    }
}
