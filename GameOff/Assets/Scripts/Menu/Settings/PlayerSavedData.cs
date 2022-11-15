using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSavedData
{
    #region Volume
    [SerializeField] private float _allVolume;
    [SerializeField] private float _musicVolume;
    [SerializeField] private float _sfxVolume;
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
    #endregion

    #region Resolution
    [SerializeField] private bool _fullscreen;
    [SerializeField] private int _resolutionIndex;
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
    #endregion
}
