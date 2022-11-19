using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settings", menuName = "SupriseOrb/Settings")]
public class Settings : ScriptableObject
{
    [Header("Volume")]
    [SerializeField] [Range(0f, 1f)] private float _allVolume = 1f;
    
    [SerializeField] [Range(0f, 1f)] private float _musicVolume = 1f;
    
    [SerializeField] [Range(0f, 1f)] private float _sfxVolume = 1f;

    [Header("Resolution")]
    [SerializeField] private bool _fullscreen = true;
    [SerializeField] [Range(0,1)] private int _resolutionIndex = 0;

    public float AllVolume
    { 
        get{return _allVolume;} 
        set{_allVolume = value;}
    }
    public float MusicVolume
    { 
        get{return _musicVolume;} 
        set{_musicVolume = value;}
    }
    public float SFXVolume
    { 
        get{return _sfxVolume;} 
        set{_sfxVolume = value;} 
    }

    public bool FullScreen
    {
        get{return _fullscreen;}
        set{_fullscreen = value;}
    }

    public int ResolutionIndex
    {
        get{return _resolutionIndex;}
        set{_resolutionIndex = value;}
    }
}
