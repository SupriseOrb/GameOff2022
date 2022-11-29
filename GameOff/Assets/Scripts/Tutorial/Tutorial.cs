using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TutorialSequence _tutorialSequence;
    [SerializeField] private UnityEvent _endEvent;

    [Header("Text Box")]
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _caption;
    [SerializeField] private GameObject _canContinueIndicator;
    [SerializeField] private bool _playerCanContinue;
    [SerializeField] private bool _inTutorial = false;
    [SerializeField] private IEnumerator _coroutine;
    [SerializeField] private bool _displayWholeSentence;

    public void StartText(UnityEvent endEvent = null)
    {
        StartTextHelper(endEvent);
    }

    public void StartTextButton()
    {
        StartTextHelper();
    }

    private void StartTextHelper(UnityEvent endEvent = null)
    {
        if (_inTutorial)
        {
            return;
        }
        _tutorialSequence.Reset();
        _inTutorial = true;
        _endEvent = endEvent;
        _caption.text = "";
        _playerCanContinue = true;
        _canContinueIndicator.SetActive(false);
        ContinueText();
    }

    public void ContinueText()
    {
        if (_tutorialSequence==null)
        {
            return;
        }
        if (_playerCanContinue)
        {
            if (_tutorialSequence.CanContinue)
            { 
                AkSoundEngine.PostEvent("Play_UISelect", gameObject);
                _canContinueIndicator.SetActive(false);
                _playerCanContinue = false;
                if (_coroutine!=null)
                {
                    StopCoroutine(_coroutine);
                }
                _displayWholeSentence = false;
                _coroutine = TypeSentence(_tutorialSequence.Continue());
                StartCoroutine(_coroutine);
            }
            else
            {
                FinishText();
            }
        }
        else
        {
            _displayWholeSentence = true;
        }  
    }

    private IEnumerator TypeSentence(CaptionedImage captionedImage)
    {
        string sentence = captionedImage.Description;

        if (captionedImage.Image != null)
        {
            _image.sprite = captionedImage.Image;
        }
        
        _caption.text = "";
        AkSoundEngine.PostEvent("Play_Text", gameObject);

        for(int charIndex = 1; charIndex < sentence.Length; charIndex++)
        {
            if (_displayWholeSentence)
            {
                AkSoundEngine.PostEvent("Stop_Text", gameObject);
                _caption.text = sentence;
                _canContinueIndicator.SetActive(true);
                _playerCanContinue = true;
                yield break;
            }

            _caption.text = sentence.Substring(0,charIndex) 
                                + "<color=#ffffff00>" 
                                + sentence.Substring(charIndex)
                                + "</color>";
            yield return null;            
        }

        AkSoundEngine.PostEvent("Stop_Text", gameObject);
        _canContinueIndicator.SetActive(true);
        _playerCanContinue = true;
    }

    private void FinishText()
    {
        _playerCanContinue = true;
        _inTutorial = false;
        if(_endEvent != null)
        {
            _endEvent.Invoke();
        }

        Debug.Log("Finish Text");
    }

    public void SkipText()
    {
        FinishText();
    }
}
