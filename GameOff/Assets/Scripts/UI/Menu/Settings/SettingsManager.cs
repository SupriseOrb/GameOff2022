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
        _allSlider.SetValueWithoutNotify(_defaultSettings.AllVolume);
        AkSoundEngine.SetRTPCValue(_all, GetVolume(_allSlider.value));
        _musicSlider.SetValueWithoutNotify(_defaultSettings.MusicVolume);
        AkSoundEngine.SetRTPCValue(_music, GetVolume(_musicSlider.value));
        _sfxSlider.SetValueWithoutNotify(_defaultSettings.SFXVolume);
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
            _allSlider.SetValueWithoutNotify(loadedSettings.AllVolume);
            AkSoundEngine.SetRTPCValue(_all, GetVolume(_allSlider.value));
            _musicSlider.SetValueWithoutNotify(loadedSettings.MusicVolume);
            AkSoundEngine.SetRTPCValue(_music, GetVolume(_musicSlider.value));
            _sfxSlider.SetValueWithoutNotify(loadedSettings.SFXVolume);
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
        AkSoundEngine.PostEvent("Play_UISelect", gameObject);
        UseDefaultSettings();
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

    public void SetFullScreen(bool isFullScreen)
    {
        AkSoundEngine.PostEvent("Play_UISelect", gameObject);
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        AkSoundEngine.PostEvent("Play_UISelect", gameObject);

        // TODO : When full screen is enabled, any resolution that is not the default resolution looks wonky
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, _fullScreenToggle.isOn);
    }
}