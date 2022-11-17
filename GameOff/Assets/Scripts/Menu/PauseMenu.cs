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

        AkSoundEngine.SetState("Battle_Intensity", "Battle_Intensity_1");
    }
    public void PauseGame()
    {
        if (!_isPaused.Value && _pauseMenuAnimator.GetCurrentAnimatorStateInfo(0).IsName("PauseMenu_IdleClose"))
        {
            AkSoundEngine.PostEvent("Play_UIPause", this.gameObject);

            _isPaused.Value = true;
            _pauseMenuAnimator.Play("PauseMenu_Open");
        }
        
    }

    public void ResumeGame()
    {
        if (_isPaused.Value && _pauseMenuAnimator.GetCurrentAnimatorStateInfo(0).IsName("PauseMenu_IdleOpen"))
        {
            AkSoundEngine.PostEvent("Play_UIResume", this.gameObject);
            
            _isPaused.Value = false;
            _pauseMenuAnimator.Play("PauseMenu_Close");
        }    
    }

    public void MainMenu()
    {
        LoadScene(0);
    }
}
