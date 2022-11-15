using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayHoverSound : MonoBehaviour
{
    public void OnMouseEnter()
    {
        AkSoundEngine.PostEvent("Play_UIHover", gameObject);
    }
}
