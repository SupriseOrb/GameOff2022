using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialSequence", menuName = "GameOff2022/Tutorial/TutorialSequence", order = 1)]
public class TutorialSequence : ScriptableObject
{
    [SerializeField] private CaptionedImage[] _pieces;
    [SerializeField] private int _defaultIndex = 0;
    [SerializeField] private int _currentIndex = 0;

    public CaptionedImage[] Sequence
    {
        get {return _pieces;}
    }

    public int Length
    {
        get {return Sequence.Length;}
    }

    public CaptionedImage GoBack()
    {
        CaptionedImage result = _pieces[_currentIndex-2];
        _currentIndex--;
        return result;
    }

    public bool CanGoBack
    {
        get {return _currentIndex > 1;}
    }
    public CaptionedImage Continue()
    {
        CaptionedImage result = _pieces[_currentIndex++];
        return result;
    }

    public bool CanContinue
    {
        get {return _currentIndex < _pieces.Length;}
    }

    private void OnEnable()
    {
        Reset();
    }
    public void Reset()
    {
        _currentIndex = _defaultIndex;
    }
}
