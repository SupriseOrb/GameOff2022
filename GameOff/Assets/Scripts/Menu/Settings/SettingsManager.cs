using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance
    {
        get{return instance;}
    }
    private static SettingsManager instance;

    [SerializeField] private Settings _savedSettings;
    [SerializeField] private Settings _defaultSettings; 
    [SerializeField] private Slider _allSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    // [SerializeField] private Slider _ambianceSlider;
    private IEnumerator _musicCoroutine;
    private static string _all = "AllVolume";
    private static string _music = "MusicVolume";
    private static string _sfx = "SFXVolume";
    // private static string _ambiance =  "AmbianceVolume";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        LoadPrefs();
    }

    public void CloseSettings()
    {
        SavePrefs();
    }

    private float GetVolume(float percentage)
    {
        return 100 * percentage;
    }

    public void LoadPrefs()
    {
        _allSlider.value = _savedSettings.AllVolume;
        AkSoundEngine.SetRTPCValue(_all, GetVolume(_savedSettings.AllVolume));
        _musicSlider.value = _savedSettings.MusicVolume;
        AkSoundEngine.SetRTPCValue(_music, GetVolume(_savedSettings.MusicVolume));
        _sfxSlider.value = _savedSettings.SFXVolume;
        AkSoundEngine.SetRTPCValue(_sfx, GetVolume(_savedSettings.SFXVolume));
        // _ambianceSlider.value = _savedSettings.SFXVolume;
        // AkSoundEngine.SetRTPCValue(_ambiance, GetVolume(_savedSettings.AmbianceVolume));
    }

    public void SavePrefs()
    {
        _savedSettings.AllVolume = _allSlider.value;
        _savedSettings.MusicVolume = _musicSlider.value;
        _savedSettings.SFXVolume = _sfxSlider.value;
        // _savedSettings.AmbianceVolume = _ambianceSlider.value;
    }

    public void ResetPrefs()
    {
        _allSlider.value = _savedSettings.AllVolume = _defaultSettings.AllVolume;
        _musicSlider.value = _savedSettings.MusicVolume = _defaultSettings.MusicVolume;
        _sfxSlider.value = _savedSettings.SFXVolume = _defaultSettings.SFXVolume;
        // _ambianceSlider.value = _savedSettings.AmbianceVolume = _defaultSettings.AmbianceVolume;
    }

    public void SetAllVolume(float volume)
    {
        AkSoundEngine.SetRTPCValue(_all, GetVolume(volume));
    }

    public void SetMusicVolume(float volume)
    {
        AkSoundEngine.SetRTPCValue(_music, GetVolume(volume));
    }

    public void SetSFXVolume(float volume)
    {
        AkSoundEngine.SetRTPCValue(_sfx, GetVolume(volume));
    }

    /* public void SetAmbianceVolume(float volume)
    {
        AkSoundEngine.SetRTPCValue(_ambiance, GetVolume(volume));
    } */

    // TODO: Implement resolution dropdown
    // TODO: Implement full screen toggle
}