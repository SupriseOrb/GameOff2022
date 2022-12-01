using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bool", menuName = "SupriseOrb/Variables/BoolVar")]
public class BoolVariable : ScriptableObject
{
    [SerializeField] private bool _defaultValue;
    [SerializeField] private bool _currentValue;
    public bool Value
    {
        get {return _currentValue;} 
        set{_currentValue = value;}
    }
    private void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        _currentValue = _defaultValue;
    }
}
