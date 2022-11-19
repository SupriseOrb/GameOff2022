using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu
{
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
}
