/* 
using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;
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
        public ActionButtonManager Action;
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
    public PromptManager promptFrame;
    public List<Target> targets = new();

    [Header("Confirm / Deny")]
    public ActionButtonManager applyBtn;
    public ActionButtonManager revertBtn;

    [Header("Setup")]
    public TMP_Text title;
    public Settings settings;
    public RectTransform safeZone;

    [Header("Values - Audio")]
    public SliderManager masterVolumeSlider;
    public SliderManager soundVolumeSlider;
    public SliderManager musicVolumeSlider;
    public AudioSource testSound;

    [Header("Values - Video")]
    public CheckboxManager fullscreenCheckbox;
    public DropdownManager resolutionDropdown;
    public DropdownManager qualityDropdown;
    public SliderManager hudScaleSlider;

    [Header("Audio Mixer Groups")]
    public AudioMixer gameMixer;
    public string[] mixerValues = new string[3];

    #endregion

    #region Miscellaneous Values

    private GameObject currentFrame;
    private GameObject previousFrame;

    private UnityEvent<bool, string> passthroughEvent;

    [Header("Miscellaneous")]
    public List<Events> applicableEvents = new();

    #endregion

    #region CurrentVariables

    private float currentMasterVolume;
    private float currentSoundVolume;
    private float currentMusicVolume;

    private int currentResolution;
    private int currentQuality;
    private bool currentFullscreen;
    private float currentHudScale;

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
            hudScale = currentHudScale
        };

        settings.ApplyChanges(newerSettings);

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
        currentHudScale = settings.defaultHudScale;

        #endregion

        #region UI Objects to Currents

        masterVolumeSlider.slider.value = currentMasterVolume;
        soundVolumeSlider.slider.value = currentSoundVolume;
        musicVolumeSlider.slider.value = currentMusicVolume;

        resolutionDropdown.dropdown.value = currentResolution;
        qualityDropdown.dropdown.value = currentQuality;
        fullscreenCheckbox.toggle.isOn = currentFullscreen;
        hudScaleSlider.slider.value = currentHudScale;

        #endregion
    }

    public void ApplyDenied()
    {
        promptFrame.SetFrameVisibility(false);
    }

    public void RevertDenied()
    {
        promptFrame.SetFrameVisibility(false);
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

    public void OnPromptFrame(string title, string body, string eventName)
    {
        promptFrame.StartPrompt(title, body, eventName, passthroughEvent);
    }

    public void PromptResultReceived(bool result, string eventName)
    {
        // visibility and unhooking of buttons happen automatically as of now!

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

    private float AudioSliderCalculations(float value)
    {
        // original math equation: (float)Math.Log10(Math.Pow(10, value / 10)) * 10 - 100;
        return Mathf.Log10(value) * 20;
    }

    private void MasterSliderChanged(float value)
    {
        currentMasterVolume = value;
        gameMixer.SetFloat(mixerValues[0], AudioSliderCalculations(value));
    }

    private void SoundSliderChanged(float value)
    {
        currentSoundVolume = value;
        gameMixer.SetFloat(mixerValues[1], AudioSliderCalculations(value));

        // This may need to be adjusted!
        if (!testSound.isPlaying)
            testSound.Play();
    }

    private void MusicSliderChanged(float value)
    {
        currentMusicVolume = value;
        gameMixer.SetFloat(mixerValues[2], AudioSliderCalculations(value));
    }

    private void HudScaleSliderChanged(float value)
    {
        currentHudScale = (float)Math.Round(value, 2);
        float result = Mathf.Lerp(0f, 100f, currentHudScale);
        safeZone.sizeDelta = new Vector2(-result, -result);
    }

    #endregion

    #region Private Functions

    private void QualityChanged(int value)
    {
        currentQuality = value;
        QualitySettings.SetQualityLevel(value);
    }

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

        settings.baseSettings.hudScale = Mathf.Clamp(
            settings.baseSettings.hudScale, 
            hudScaleSlider.slider.minValue, 
            hudScaleSlider.slider.maxValue
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
    }

    private void PostInitSetup()
    {
        masterVolumeSlider.slider.value = settings.baseSettings.masterVolume;
        soundVolumeSlider.slider.value = settings.baseSettings.soundVolume;
        musicVolumeSlider.slider.value = settings.baseSettings.musicVolume;

        masterVolumeSlider.inputField.text = settings.baseSettings.masterVolume.ToString();
        soundVolumeSlider.inputField.text = settings.baseSettings.soundVolume.ToString();
        musicVolumeSlider.inputField.text = settings.baseSettings.musicVolume.ToString();

        qualityDropdown.dropdown.value = settings.baseSettings.quality;
        QualitySettings.SetQualityLevel(settings.baseSettings.quality);

        fullscreenCheckbox.toggle.isOn = settings.baseSettings.fullscreen;

        hudScaleSlider.slider.value = settings.baseSettings.hudScale;
        hudScaleSlider.inputField.text = settings.baseSettings.hudScale.ToString();
        HudScaleSliderChanged(hudScaleSlider.slider.value);

        currentMasterVolume = settings.baseSettings.masterVolume;
        currentSoundVolume = settings.baseSettings.soundVolume;
        currentMusicVolume = settings.baseSettings.musicVolume;
        
        gameMixer.SetFloat(mixerValues[0], AudioSliderCalculations(currentMasterVolume));
        gameMixer.SetFloat(mixerValues[1], AudioSliderCalculations(currentSoundVolume));
        gameMixer.SetFloat(mixerValues[2], AudioSliderCalculations(currentMusicVolume));

        currentResolution = settings.baseSettings.resolution;
        currentFullscreen = settings.baseSettings.fullscreen;
        currentHudScale = settings.baseSettings.hudScale;

        masterVolumeSlider.slider.onValueChanged.AddListener(MasterSliderChanged);
        soundVolumeSlider.slider.onValueChanged.AddListener(SoundSliderChanged);
        musicVolumeSlider.slider.onValueChanged.AddListener(MusicSliderChanged);

        fullscreenCheckbox.toggle.onValueChanged.AddListener((bool value) => { currentFullscreen = value; });
        resolutionDropdown.dropdown.onValueChanged.AddListener((int value) => { currentResolution = value; });
        qualityDropdown.dropdown.onValueChanged.AddListener(QualityChanged);
        hudScaleSlider.slider.onValueChanged.AddListener(HudScaleSliderChanged);

        applyBtn.button.onClick.AddListener(() => { OnPromptFrame(applicableEvents[0].promptTitle, applicableEvents[0].promptBody, applicableEvents[0].eventName); });
        revertBtn.button.onClick.AddListener(() => { OnPromptFrame(applicableEvents[1].promptTitle, applicableEvents[1].promptBody, applicableEvents[1].eventName); });
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
            i.Action.button.onClick.AddListener(delegate { ToggleMenu(i); });

        SetupResolutions();
        ValidateData();
        PostInitSetup();
    }

    #endregion
}
*/