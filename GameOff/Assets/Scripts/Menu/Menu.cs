using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] protected BoolVariable _isMusicPlaying;

    // [SerializeField] private GameObject _loadingScreen;
    // [SerializeField] private Slider _loadingBar;

    protected void LoadScene(int sceneIndex)
    {
        // Collin TODO: Play regular button sound
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
            // Collin TODO: Play regular button sound
            // AkSoundEngine.PostEvent("Play_UISelect", this.gameObject);
        }
        else
        {
            // Collin TODO: Play back sound
            // AkSoundEngine.PostEvent("Play_UIBack", this.gameObject);
        }
        
        canvas.enabled = canSee;
    }
    
    public void CloseSettings(Canvas canvas)
    {
        canvas.gameObject.GetComponent<SettingsManager>().CloseSettings();
        SetCanvasVisibility(canvas, false);
    }

    public void OpenCanvas(Canvas canvas)
    {
        SetCanvasVisibility(canvas, true);
    }

    public void CloseCanvas(Canvas canvas)
    {
        SetCanvasVisibility(canvas, false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
