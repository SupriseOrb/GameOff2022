using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Handbook : MonoBehaviour
{
    [SerializeField] private TutorialSequence[] _guideSequences;
    [SerializeField] private TutorialSequence _currentSequence;

    [SerializeField] private GameObject _leftArrow;
    [SerializeField] private GameObject _rightArrow;

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    public void OpenHandbook()
    {
        // TODO : Reset the Handbook
        ChooseGuide();
    }

    public void ChooseGuide(int guideNumber = 0)
    {
        // TODO : Show which button / topic I'm highlighting
        _currentSequence = _guideSequences[Mathf.Clamp(guideNumber, 0, _guideSequences.Length-1)];

        if (_currentSequence.Length < 1)
        {
            _leftArrow.SetActive(false);
            _rightArrow.SetActive(false);
        }
        else
        {
            _rightArrow.SetActive(true);
        }
    }

    public void ClickRightArrow()
    {
        CaptionedImage captionedImage = _currentSequence.Continue();
        _text.text = captionedImage.Description;
        _image.sprite = captionedImage.Image;

        // TODO : Figure out what you need to disable
    }

    public void ClickLeftArrow()
    {
        CaptionedImage captionedImage = _currentSequence.GoBack();
        _text.text = captionedImage.Description;
        _image.sprite = captionedImage.Image;

        // TODO : Figure out what you need to disable
    }
}
