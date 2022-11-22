using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : Menu
{
    [SerializeField] private Canvas _pauseMenuCanvas;
    [SerializeField] private BoolVariable _isPaused;
    [SerializeField] private Animator _pauseMenuAnimator;

    private EventSystem _currentEventSystem;
    public void Start()
    {
        _currentEventSystem = EventSystem.current;
        _isPaused.Value = false;

        if (!_isMusicPlaying.Value)
        {
            AkSoundEngine.PostEvent("Music", gameObject);
            _isMusicPlaying.Value = true;
        }
        AkSoundEngine.SetState("Music_State", "Battle");
        AkSoundEngine.SetState("Battle_Intensity", "Battle_Intensity_1");
        AkSoundEngine.SetState("Ambience_states", "Ambience_1");
        AkSoundEngine.PostEvent("Play_Ambience", this.gameObject);
    }
    public void PauseGame()
    {
        if (!_isPaused.Value && _pauseMenuAnimator.GetCurrentAnimatorStateInfo(0).IsName("Panel_IdleClose"))
        {
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
        ResumeAudio();
        AkSoundEngine.PostEvent("Stop_Ambience", this.gameObject);
        LoadScene(0);
    }
}
