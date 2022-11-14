using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settings", menuName = "SupriseOrb/Settings")]
public class Settings : ScriptableObject
{
    [SerializeField] [Range(0f, 1f)] private float _allVolume = 1f;
    
    [SerializeField] [Range(0f, 1f)] private float _musicVolume = 1f;
    
    [SerializeField] [Range(0f, 1f)] private float _sfxVolume = 1f;
    // [SerializeField] [Range(0f, 1f)] private float _ambianceVolume = 1f;

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
    
    /* public float AmbianceVolume
    { 
        get{return _ambianceVolume;} 
        set{_ambianceVolume = value;} 
    } */
}
