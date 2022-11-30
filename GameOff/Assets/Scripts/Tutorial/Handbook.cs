using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Handbook : MonoBehaviour
{
    [System.Serializable]
    public struct ButtonGuideSequence
    {
        [SerializeField] public Image image;
        [SerializeField] public TutorialSequence sequence;
    }
    [SerializeField] private ButtonGuideSequence[] _guideSequences;
    [SerializeField] private TutorialSequence _currentSequence;
    [SerializeField] private GameObject _leftArrow;
    [SerializeField] private GameObject _rightArrow;

    [SerializeField] private Image _previousSelectedButtonImage;
    [SerializeField] private Color _colorReg;
    [SerializeField] private Color _colorSelect;

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    public void OpenHandbook()
    {
        ChooseGuideHelper();
    }

    public void ChooseGuide(int guideNumber = 0)
    {
        AkSoundEngine.PostEvent("Play_UISelect", gameObject);
        ChooseGuideHelper(guideNumber);
    }

    private void ChooseGuideHelper(int guideNumber = 0)
    {
        _currentSequence = _guideSequences[guideNumber].sequence;
        
        if (_previousSelectedButtonImage != null)
        {
            _previousSelectedButtonImage.color = _colorReg;
        }
        
        _previousSelectedButtonImage = _guideSequences[guideNumber].image;
        _previousSelectedButtonImage.color = _colorSelect;
        _currentSequence.Reset();
        _leftArrow.SetActive(false);

        if (_currentSequence.Length <= 1)
        {    
            _rightArrow.SetActive(false);
        }
        else
        {
            _rightArrow.SetActive(true);
        }
        CaptionedImage captionedImage = _currentSequence.Continue();
        _text.text = captionedImage.Description;
        _image.sprite = captionedImage.Image;
    }

    public void ClickRightArrow()
    {
        AkSoundEngine.PostEvent("Play_UISelect", gameObject);
        CaptionedImage captionedImage = _currentSequence.Continue();
        _text.text = captionedImage.Description;
        _image.sprite = captionedImage.Image;

        _leftArrow.SetActive(true);
        if (!_currentSequence.CanContinue)
        {
            _rightArrow.SetActive(false);
        }
    }

    public void ClickLeftArrow()
    {
        AkSoundEngine.PostEvent("Play_UISelect", gameObject);
        Debug.Log("Clicked Left Arrow");
        CaptionedImage captionedImage = _currentSequence.GoBack();
        _text.text = captionedImage.Description;
        _image.sprite = captionedImage.Image;

        _rightArrow.SetActive(true);
        if (!_currentSequence.CanGoBack)
        {
            _leftArrow.SetActive(false);
        }
    }
}
