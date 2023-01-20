using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] protected BoolVariable _hasSeenTutorial;
    [SerializeField] protected BoolVariable _isMusicPlaying;

    [SerializeField] private GameObject _loadingScreen;

    public void LoadScene(int sceneIndex)
    {
        AkSoundEngine.PostEvent("Play_UISelect", this.gameObject);
        
        StartCoroutine(LoadSceneAsynchronously(sceneIndex));
    }

    IEnumerator LoadSceneAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        _loadingScreen.SetActive(true);
        while(!operation.isDone)
        {
            yield return null;
        }
    }

    protected void SetCanvasVisibility(Canvas canvas, bool canSee)
    {
        if (canSee)
        {
            AkSoundEngine.PostEvent("Play_UISelect", this.gameObject);
        }
        else
        {
            AkSoundEngine.PostEvent("Play_UIBack", this.gameObject);
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
