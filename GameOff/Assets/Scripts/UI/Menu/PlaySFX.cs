using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    public void playSFX(string sfxName)
    {
        AkSoundEngine.PostEvent(sfxName, gameObject);
    }
}
