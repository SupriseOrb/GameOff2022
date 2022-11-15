using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance
    {
        get{return instance;}
    }
    private static SettingsManager instance;

    [Header("Settings")]
    [SerializeField] private Settings _defaultSettings;
    static private string SAVED_DATA_FILE_NAME = "PlayerSaveData";
    [Header("Volume")]
    [SerializeField] private Slider _allSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    private static string _all = "AllVolume";
    private static string _music = "MusicVolume";
    private static string _sfx = "SFXVolume";
    [Header("Resolution")]

    [SerializeField] private Toggle _fullScreenToggle;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private Resolution[] _resolutions;

    [System.Serializable]
    public struct Resolution
    {
        public int width;
        public int height;
    }
    
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
    private void UseDefaultSettings()
    {
        _allSlider.value = _defaultSettings.AllVolume;
        AkSoundEngine.SetRTPCValue(_all, GetVolume(_allSlider.value));
        _musicSlider.value = _defaultSettings.MusicVolume;
        AkSoundEngine.SetRTPCValue(_music, GetVolume(_musicSlider.value));
        _sfxSlider.value = _defaultSettings.SFXVolume;
        AkSoundEngine.SetRTPCValue(_sfx, GetVolume(_sfxSlider.value));

        _fullScreenToggle.isOn = _defaultSettings.FullScreen;
        _resolutionDropdown.value = _defaultSettings.ResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
        Resolution resolution = _resolutions[_defaultSettings.ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, _fullScreenToggle.isOn);
    }

    public void LoadPrefs()
    {
        PlayerSavedData loadedSettings = DataSaver.LoadData<PlayerSavedData>(SAVED_DATA_FILE_NAME);
        if (loadedSettings == null)
        {
            UseDefaultSettings();
        }
        else
        {
            _allSlider.value = loadedSettings.AllVolume;
            AkSoundEngine.SetRTPCValue(_all, GetVolume(_allSlider.value));
            _musicSlider.value = loadedSettings.MusicVolume;
            AkSoundEngine.SetRTPCValue(_music, GetVolume(_musicSlider.value));
            _sfxSlider.value = loadedSettings.SFXVolume;
            AkSoundEngine.SetRTPCValue(_sfx, GetVolume(_sfxSlider.value));

            _fullScreenToggle.isOn = loadedSettings.FullScreen;
            _resolutionDropdown.value = loadedSettings.ResolutionIndex;
            _resolutionDropdown.RefreshShownValue();
            Resolution resolution = _resolutions[loadedSettings.ResolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, _fullScreenToggle.isOn);
        }
    }

    public void SavePrefs()
    {
        PlayerSavedData saveFile = new PlayerSavedData();

        saveFile.AllVolume = _allSlider.value;
        saveFile.MusicVolume = _musicSlider.value;
        saveFile.SFXVolume = _sfxSlider.value;

        saveFile.FullScreen = _fullScreenToggle.isOn;
        saveFile.ResolutionIndex = _resolutionDropdown.value;
        DataSaver.SaveData(saveFile, SAVED_DATA_FILE_NAME);
    }

    public void ResetPrefs()
    {
        // Collin TODO: Play sfx
        UseDefaultSettings();
    }

    public void SetAllVolume(float volume)
    {
        // COLLIN TODO: Play sfx sound
        AkSoundEngine.SetRTPCValue(_all, GetVolume(volume));
    }

    public void SetMusicVolume(float volume)
    {
        // COLLIN TODO: Play sfx sound
        AkSoundEngine.SetRTPCValue(_music, GetVolume(volume));
    }

    public void SetSFXVolume(float volume)
    {
        // COLLIN TODO: Play sfx sound
        AkSoundEngine.SetRTPCValue(_sfx, GetVolume(volume));
    }

    public void SetFullScreen(bool isFullScreen)
    {
        // COLLIN TODO: Play sfx sound
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        // COLLIN TODO: Play sfx sound

        // TODO: When full screen is enabled, any resolution that is not the default resolution looks wonky
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, _fullScreenToggle.isOn);
    }
}