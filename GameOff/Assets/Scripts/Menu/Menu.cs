using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _settingsCanvas;

    [SerializeField] protected BoolVariable _isMusicPlaying;

    // [SerializeField] private GameObject _loadingScreen;
    // [SerializeField] private Slider _loadingBar;

    protected void LoadScene(int sceneIndex)
    {
        // Play regular button sound
        // AkSoundEngine.PostEvent("Play_UISelect", this.gameObject);
        
        SceneManager.LoadScene(sceneIndex);
        // StartCoroutine(LoadSceneAsynchronously(sceneIndex));
    }

    /* IEnumerator LoadSceneAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        _loadingBar.interactable = false;
        _loadingScreen.SetActive(true);
        while(!operation.isDone)
        {
            _loadingBar.value = operation.progress;
            yield return null;
        }
    } */

    protected void SetCanvasVisibility(Canvas canvas, bool canSee)
    {
        if (canSee)
        {
            // Play regular button sound
            // AkSoundEngine.PostEvent("Play_UISelect", this.gameObject);
        }
        else
        {
            // Play back sound
            // AkSoundEngine.PostEvent("Play_UIBack", this.gameObject);
        }
        
        canvas.enabled = canSee;
    }

    public void OpenSettings()
    {
        SetCanvasVisibility(_settingsCanvas.GetComponent<Canvas>(), true);
    }
    
    public void CloseSettings()
    {
        _settingsCanvas.GetComponent<SettingsManager>().CloseSettings();
        SetCanvasVisibility(_settingsCanvas.GetComponent<Canvas>(), false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
