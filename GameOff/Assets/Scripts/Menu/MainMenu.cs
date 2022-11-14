using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu
{
    public void Start()
    {
        // Rob TODO: Play Main Menu Music
        // Note: There may be a bug where music duplicates
    }
    public void StartGame()
    {
        LoadScene(1);
    }
}
