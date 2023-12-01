using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using TMPro;

public class SettingsManager : MonoBehaviour
{
    #region All Definitions

    #region Classes

    [Serializable]
    public class Target
    {
        public GameObject Frame;
        public Button Action;
    }

    [Serializable]
    public class Events
    {
        public string eventName;

        public string promptTitle;
        public string promptBody;

        public UnityEvent acceptedEvent;
        public UnityEvent deniedEvent;
    }

    #endregion

    #region Unity Inspector

    [Header("Frames and Buttons")]
    public List<Target> targets = new();

    [Header("Confirm / Deny")]
    public Button applyBtn;
    public Button revertBtn;

    [Header("Setup")]
    public TMP_Text title;
    public SettingsData settings;
    public Button exitBtn;

    [Header("Values - Audio")]
    public SlideManager masterVolumeSlider;
    public SlideManager soundVolumeSlider;
    public SlideManager musicVolumeSlider;

    [Header("Values - Video")]
    public Toggle fullscreenCheckbox;
    public DropdownManager resolutionDropdown;
    public DropdownManager qualityDropdown;

    public SlideManager framesSlider;
    public Toggle displayFPSCheckbox;

    [Header("Audio Mixing")]
    public string[] mixerValues = new string[3];

    #endregion

    #region Miscellaneous Values

    private GameObject currentFrame;
    private GameObject previousFrame;

    // figure out wtf is happening with this before commiting all these changes!
    private UnityEvent<bool, string> passthroughEvent;

    [Header("Miscellaneous")]
    public List<Events> applicableEvents = new();

    #endregion

    #region CurrentVariables

    private float currentMasterVolume;
    private float currentSoundVolume;
    private float currentMusicVolume;

    private int currentResolution;
    private int currentFrames;
    private int currentQuality;
    
    private bool currentFullscreen;
    private bool currentDisplayFPS; 

    #endregion

    #endregion

    #region Base Buttons

    public void ApplySettings()
    {
        #region Video Adjustments

#if UNITY_EDITOR
        Debug.LogWarning("Current runtime is running under the Unity Editor, some video settings may be ignored!");
#endif
        Resolution targetResolution = settings.resolutions[currentResolution];
        Screen.SetResolution(targetResolution.width, targetResolution.height, currentFullscreen);
        
        QualitySettings.SetQualityLevel(currentQuality);
        Application.targetFrameRate = currentFrames;

        #endregion

        #region Settings Finalizing

        BaseSettings newerSettings = new()
        {
            masterVolume = currentMasterVolume,
            soundVolume = currentSoundVolume,
            musicVolume = currentMusicVolume,

            resolution = currentResolution,
            quality = currentQuality,
            fullscreen = currentFullscreen,

            frames = currentFrames,
            displayFPS = currentDisplayFPS
        };

        settings.ApplySettings(newerSettings);

        #endregion
    }

    public void RevertSettings()
    {
        Debug.LogWarning("Resetting Settings back to their defaults defined within the Settings Inspector!");

        #region Currents to Defaults

        currentMasterVolume = settings.defaultMasterVolume;
        currentSoundVolume = settings.defaultSoundVolume;
        currentMusicVolume = settings.defaultMusicVolume;

        currentResolution = settings.defaultResolution;
        currentQuality = settings.defaultQuality;
        currentFullscreen = settings.defaultFullscreen;

        currentFrames = settings.defaultFrames;
        currentDisplayFPS = settings.defaultDisplayFPS;

        #endregion

        #region UI Objects to Currents

        masterVolumeSlider.slider.value = currentMasterVolume;
        soundVolumeSlider.slider.value = currentSoundVolume;
        musicVolumeSlider.slider.value = currentMusicVolume;

        resolutionDropdown.dropdown.value = currentResolution;
        qualityDropdown.dropdown.value = currentQuality;
        fullscreenCheckbox.isOn = currentFullscreen;

        framesSlider.slider.value = currentFrames;
        displayFPSCheckbox.isOn = currentDisplayFPS;

        #endregion
    }

    private void ToggleMenu(Target Data)
    {
        if (currentFrame == Data.Frame)
            return;

        Data.Frame.SetActive(!Data.Frame.activeSelf);

        previousFrame = currentFrame;
        currentFrame = Data.Frame;

        if (previousFrame.activeSelf)
            previousFrame.SetActive(false);

        title.text = "Settings - " + currentFrame.name;
    }

    #endregion

    #region Prompt Functions

    public void ApplyDenied() => PromptManager.Instance.SetFrameVisibility(false);
    public void RevertDenied() => PromptManager.Instance.SetFrameVisibility(false);

    public void OnPromptFrame(string title, string body, string eventName) => PromptManager.Instance.StartPrompt(title, body, eventName, passthroughEvent);

    public void PromptResultReceived(bool result, string eventName)
    {
        foreach (Events i in applicableEvents)
        {
            if (i.eventName == eventName)
            {
                UnityEvent unityEvent;

                if (result)
                    unityEvent = i.acceptedEvent;
                else
                    unityEvent = i.deniedEvent;

                unityEvent.Invoke();
            }
        }
    }

    #endregion

    #region Slider Controls

    private float AudioSliderCalculations(float value) => Mathf.Log10(value) * 20;

    private void MasterSliderChanged(float value)
    {
        currentMasterVolume = value;
        AudioManager.Instance.AudioMixer.SetFloat(mixerValues[0], AudioSliderCalculations(value));
    }

    private void SoundSliderChanged(float value)
    {
        currentSoundVolume = value;
        AudioManager.Instance.AudioMixer.SetFloat(mixerValues[1], AudioSliderCalculations(value));

        if (!AudioManager.Instance.IsPlaying("TestSound"))
            AudioManager.Instance.Play("TestSound");
    }

    private void MusicSliderChanged(float value)
    {
        currentMusicVolume = value;
        AudioManager.Instance.AudioMixer.SetFloat(mixerValues[2], AudioSliderCalculations(value));
    }

    private void FramesSliderChanged(float value) => currentFrames = (int)value;

    #endregion

    #region Private Functions

    private void ResolutionChanged(int value) => currentResolution = value;
    private void QualityChanged(int value) => currentQuality = value;
    private void FullscreenChanged(bool value) => currentFullscreen = value;
    private void DisplayFPSChanged(bool value) => currentDisplayFPS = value;

    private void SetupResolutions()
    {
        List<string> options = new();
        settings.resolutions.Reverse();

        for (int i = 0; i < settings.resolutions.Length; i++)
        {
            string option = settings.resolutions[i].width + "x" + settings.resolutions[i].height 
                + " - @" + settings.resolutions[i].refreshRate + "hz";
            options.Add(option);
        }

        if (resolutionDropdown.dropdown.options.Count > 0)
            resolutionDropdown.dropdown.ClearOptions();

        resolutionDropdown.dropdown.AddOptions(options);
        resolutionDropdown.dropdown.RefreshShownValue();

        resolutionDropdown.dropdown.value = settings.baseSettings.resolution;
    }

    private void ValidateData()
    {
        settings.baseSettings.masterVolume = Mathf.Clamp(
            settings.baseSettings.masterVolume, 
            masterVolumeSlider.slider.minValue, 
            masterVolumeSlider.slider.maxValue
            );

        settings.baseSettings.soundVolume = Mathf.Clamp(
            settings.baseSettings.soundVolume, 
            soundVolumeSlider.slider.minValue, 
            soundVolumeSlider.slider.maxValue
            );

        settings.baseSettings.musicVolume = Mathf.Clamp(
            settings.baseSettings.musicVolume, 
            musicVolumeSlider.slider.minValue, 
            musicVolumeSlider.slider.maxValue
            );

        settings.baseSettings.resolution = Mathf.Clamp(
            settings.baseSettings.resolution,
            0,
            settings.resolutions.Length - 1
            );

        settings.baseSettings.quality = Mathf.Clamp(
            settings.baseSettings.quality,
            0,
            qualityDropdown.dropdown.options.Count
            );

        settings.baseSettings.frames = Mathf.Clamp(
            settings.baseSettings.frames,
            (int)framesSlider.slider.minValue,
            (int)framesSlider.slider.maxValue
            );
    }

    private void PostInitSetup()
    {
        masterVolumeSlider.slider.value = settings.baseSettings.masterVolume;
        soundVolumeSlider.slider.value = settings.baseSettings.soundVolume;
        musicVolumeSlider.slider.value = settings.baseSettings.musicVolume;
        framesSlider.slider.value = settings.baseSettings.frames;

        masterVolumeSlider.inputField.text = settings.baseSettings.masterVolume.ToString();
        soundVolumeSlider.inputField.text = settings.baseSettings.soundVolume.ToString();
        musicVolumeSlider.inputField.text = settings.baseSettings.musicVolume.ToString();
        framesSlider.inputField.text = settings.baseSettings.frames.ToString();

        qualityDropdown.dropdown.value = settings.baseSettings.quality;
        QualitySettings.SetQualityLevel(settings.baseSettings.quality);

        fullscreenCheckbox.isOn = settings.baseSettings.fullscreen;
        displayFPSCheckbox.isOn = settings.baseSettings.displayFPS; 

        currentMasterVolume = settings.baseSettings.masterVolume;
        currentSoundVolume = settings.baseSettings.soundVolume;
        currentMusicVolume = settings.baseSettings.musicVolume;

        AudioManager.Instance.AudioMixer.SetFloat(mixerValues[0], AudioSliderCalculations(currentMasterVolume));
        AudioManager.Instance.AudioMixer.SetFloat(mixerValues[1], AudioSliderCalculations(currentSoundVolume));
        AudioManager.Instance.AudioMixer.SetFloat(mixerValues[2], AudioSliderCalculations(currentMusicVolume));

        currentResolution = settings.baseSettings.resolution;
        currentFullscreen = settings.baseSettings.fullscreen;

        currentFrames = settings.baseSettings.frames;
        currentDisplayFPS = settings.baseSettings.displayFPS;

        masterVolumeSlider.slider.onValueChanged.AddListener(MasterSliderChanged);
        soundVolumeSlider.slider.onValueChanged.AddListener(SoundSliderChanged);
        musicVolumeSlider.slider.onValueChanged.AddListener(MusicSliderChanged);
        framesSlider.slider.onValueChanged.AddListener(FramesSliderChanged);

        fullscreenCheckbox.onValueChanged.AddListener(FullscreenChanged);
        resolutionDropdown.dropdown.onValueChanged.AddListener(ResolutionChanged);
        qualityDropdown.dropdown.onValueChanged.AddListener(QualityChanged);
        displayFPSCheckbox.onValueChanged.AddListener(DisplayFPSChanged);

        applyBtn.onClick.AddListener(() => { OnPromptFrame(applicableEvents[0].promptTitle, applicableEvents[0].promptBody, applicableEvents[0].eventName); });
        revertBtn.onClick.AddListener(() => { OnPromptFrame(applicableEvents[1].promptTitle, applicableEvents[1].promptBody, applicableEvents[1].eventName); });

        exitBtn.onClick.AddListener(delegate { gameObject.SetActive(false); });

        settings.settingsChanged.Invoke(settings.baseSettings.displayFPS);
    }

    #endregion

    #region Invoke Controlled

    public void Initilize()
    {
        if (passthroughEvent == null)
        {
            passthroughEvent = new();
            passthroughEvent.AddListener(PromptResultReceived);
        }

        currentFrame = targets[0].Frame;
        title.text = "Settings - " + currentFrame.name;

        foreach (Target target in targets)
        {
            if (target.Frame == currentFrame)
            {
                if (!target.Frame.activeSelf)
                    target.Frame.SetActive(true);

                continue;
            }

            target.Frame.SetActive(false);
        }

        foreach (Target i in targets)
            i.Action.onClick.AddListener(delegate { ToggleMenu(i); });

        SetupResolutions();
        ValidateData();
        PostInitSetup();
    }

    #endregion
}