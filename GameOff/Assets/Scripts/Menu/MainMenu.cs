using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu
{
    [SerializeField] private Canvas _creditsCanvas;
    public void Start()
    {
        // Play Main Menu Music
    }
    public void StartGame()
    {
        LoadScene(1);
    }
    public void OpenCredits()
    {
        SetCanvasVisibility(_creditsCanvas, true);
    }

    public void CloseCredits()
    {
        SetCanvasVisibility(_creditsCanvas, false);
    }
}
