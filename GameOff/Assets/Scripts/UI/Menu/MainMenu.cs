using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenu : Menu
{
    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private UnityEvent _afterTutorial;
    public void Start()
    {
        if (!_isMusicPlaying.Value)
        {
            AkSoundEngine.PostEvent("Music", gameObject);
            _isMusicPlaying.Value = true;
        }
        AkSoundEngine.SetState("Music_State", "Title");
    }

    public void StartGame()
    {
        LoadScene(1);
    }

    public void StartTutorial()
    {
        if (_hasSeenTutorial.Value)
        {
            StartGame();
        }
        else
        {
            _hasSeenTutorial.Value = true;
            _tutorial.gameObject.SetActive(true);
            _tutorial.StartText(_afterTutorial);
        }
    }
}
