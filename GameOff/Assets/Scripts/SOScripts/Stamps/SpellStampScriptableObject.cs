using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellStampScriptableObject", menuName = "GameOff2022/Stamps/SpellStampSO", order = 3)]
public class SpellStampScriptableObject : StampScriptableObject
{
    [SerializeField] private float _spellStampValue;
    [SerializeField] private bool _isOffensive;

    public float SpellValue
    {
        get {return _spellStampValue;}
    }
    public bool IsOffensive
    {
        get {return _isOffensive;}
    }
}
