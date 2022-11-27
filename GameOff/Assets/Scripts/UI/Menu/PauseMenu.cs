using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : Menu
{
    [SerializeField] private Canvas _pauseMenuCanvas;
    [SerializeField] private BoolVariable _isPaused;
    [SerializeField] private Animator _pauseMenuAnimator;
    [SerializeField] private BoolVariable _inUpgradeMenu;

    private EventSystem _currentEventSystem;
    void Start()
    {
        _currentEventSystem = EventSystem.current;
        _isPaused.Value = false;

        if (!_isMusicPlaying.Value)
        {
            AkSoundEngine.PostEvent("Music", gameObject);
            _isMusicPlaying.Value = true;
        }
        AkSoundEngine.PostEvent("Play_Ambience", this.gameObject);
        AkSoundEngine.SetState("Music_State", "Battle");
    }
    public void PauseGame()
    {
        if (!_isPaused.Value && _pauseMenuAnimator.GetCurrentAnimatorStateInfo(0).IsName("Panel_IdleClose"))
        {
            Time.timeScale = 0f;
            AkSoundEngine.PostEvent("Play_UIPause", this.gameObject);
            AkSoundEngine.SetRTPCValue("Is_Paused", 100f);
            AkSoundEngine.PostEvent("Mute_Ambience", this.gameObject);

            _isPaused.Value = true;
            _pauseMenuAnimator.Play("Panel_Open");
        }
        
    }

    public void ResumeGame()
    {
        if (_isPaused.Value && _pauseMenuAnimator.GetCurrentAnimatorStateInfo(0).IsName("Panel_IdleOpen"))
        {
            if (!_inUpgradeMenu.Value)
            {
                Time.timeScale = 1f;
            }

            AkSoundEngine.PostEvent("Play_UIResume", this.gameObject);
            ResumeAudio();
            _isPaused.Value = false;
            _pauseMenuAnimator.Play("Panel_Close");
        }    
    }

    private void ResumeAudio()
    {
        AkSoundEngine.SetRTPCValue("Is_Paused", 0f);
        AkSoundEngine.PostEvent("Unmute_Ambience", this.gameObject);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        ResumeAudio();
        AkSoundEngine.PostEvent("Stop_Ambience", this.gameObject);
        LoadScene(0);
    }
}
