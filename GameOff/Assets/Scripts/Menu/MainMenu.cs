using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu
{
    public void Start()
    {
        // Rob TODO: Play Main Menu Music
        AkSoundEngine.SetState("Music_State", "Title");
        AkSoundEngine.SetState("Battle_Intensity", "Battle_Intensity_1");
        AkSoundEngine.PostEvent("Music", gameObject);
        // Note: There may be a bug where music duplicates
    }
    public void StartGame()
    {
        LoadScene(1);
    }
}
