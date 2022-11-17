using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

using Ink.Runtime;
using TMPro;

public class SystemTextManager : MonoBehaviour
{
    public static SystemTextManager Instance
    {
        get{return _instance;}
    }

    public void StartStoryViaButton(TextAsset inkFile)
    {
        StartStoryHelper(inkFile, playButtonSFX: true);
    }

    public void StartStory(TextAsset inkFile, UnityEvent endEvent = null)
    {
        StartStoryHelper(inkFile, endEvent);
    }

    public void OnAdvanceDialogue()
    {
        AdvanceDialogue(true);
    }

    private static SystemTextManager _instance;

    #region Story
    private static Story _story;
    [SerializeField] private UnityEvent _endEvent;
    [SerializeField] private float _animationTime = 0.5f;
    [SerializeField] private float _waitTime = 0.1f;
    private string _currentSentence;
    #endregion
    	
    #region Dialogue Box
    [SerializeField] private GameObject _systemTextBox;
    [SerializeField] private TextMeshProUGUI _textHolder;
    [SerializeField] private GameObject _canContinueIndicator;
    [SerializeField] private Animator animator;

    #endregion

    #region Checks
    [SerializeField] private bool _playerCanContinue;
    [SerializeField] private BoolVariable _systemTextOn;
    [SerializeField] private BoolVariable _isPaused;
    #endregion

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        SceneManager.activeSceneChanged += OnChangeScene;
    }

    private void OnChangeScene(Scene current, Scene next)
    {
        _systemTextOn.Value = false;
        SceneManager.activeSceneChanged -= OnChangeScene;
    }

    private void StartStoryHelper(TextAsset inkFile, UnityEvent endEvent = null, bool playButtonSFX = false)
    {
        if(!_systemTextOn.Value && !_isPaused.Value)
        {
            if (playButtonSFX)
            {
                AkSoundEngine.PostEvent("Play_UISelect", gameObject);
            }
            _story = new Story(inkFile.text);
            _endEvent = endEvent;
            _textHolder.text = "";
            _canContinueIndicator.SetActive(false);

            animator.Play("SystemText_Open");
            _systemTextOn.Value = true;
            
            StartCoroutine(WaitForOpenAnimation());
        }
    }

    private IEnumerator WaitForOpenAnimation()
    {
        yield return new WaitForSeconds(_animationTime);
        _playerCanContinue = true;
        AdvanceDialogue();
    }

    private void AdvanceDialogue(bool playButtonSfx = false)
    {
        if(_playerCanContinue && _systemTextOn.Value && !_isPaused.Value && _story!=null)
        {
            if (_story.canContinue)
            {
                if (playButtonSfx)
                {
                    AkSoundEngine.PostEvent("Play_UISelect", gameObject);
                }
                
                _canContinueIndicator.SetActive(false);
                _playerCanContinue = false;

                StartCoroutine(TypeSentence(_story.Continue()));
            }
            else
            {
                FinishDialogue();
            }
        }          
    }

    private IEnumerator TypeSentence(string sentence)
    {
        _textHolder.text = "";
        AkSoundEngine.PostEvent("Play_Text", gameObject);

        for(int charIndex = 1; charIndex < sentence.Length; charIndex++)
        {

            _textHolder.text = sentence.Substring(0,charIndex) 
                                + "<color=#ffffff00>" 
                                + sentence.Substring(charIndex)
                                + "</color>";
            yield return null;            
        }

        AkSoundEngine.PostEvent("Stop_Text", gameObject);
        _canContinueIndicator.SetActive(true);
        _playerCanContinue = true;
    }

    private void FinishDialogue()
    {
        animator.Play("SystemText_Close");
        _systemTextOn.Value = false;
        _playerCanContinue = false;
        _story = null;
        StartCoroutine(WaitForCloseAnimation());      
    }
    private IEnumerator WaitForCloseAnimation()
    {
        yield return new WaitForSeconds(_animationTime);
        _playerCanContinue = true;
        TriggerEndBehavior();
    }
    private void TriggerEndBehavior()
    {
        if(_endEvent != null)
        {
            _endEvent.Invoke();
        }
    }
}
