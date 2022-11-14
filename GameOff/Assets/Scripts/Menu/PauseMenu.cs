using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : Menu
{
    [SerializeField] private Canvas _pauseMenuCanvas;
    [SerializeField] private BoolVariable _isPaused;

    private EventSystem _currentEventSystem;
    [SerializeField] private GameObject _resumeButton;

    public void Start()
    {
        _currentEventSystem = EventSystem.current;
        _isPaused.Value = false;

        // Rob TODO: Play Gameplay Music
        // Note: There may be a bug where music duplicates
    }
    public void PauseGame()
    {
        if(!_isPaused.Value)
        {
            // Collin TODO: Play pause sfx
            // AkSoundEngine.PostEvent("Play_UIPause", this.gameObject);

            _pauseMenuCanvas.enabled = true;
            _isPaused.Value = true;
        }
        
    }

    public void ResumeGame()
    {
        if(_isPaused.Value)
        {
            // Collin TODO: Play resume sfx
            // AkSoundEngine.PostEvent("Play_UIResume", this.gameObject);
            
            _pauseMenuCanvas.enabled = false;
            _isPaused.Value = false;
        }    
    }

    public void MainMenu()
    {
        LoadScene(0);
    }
}
