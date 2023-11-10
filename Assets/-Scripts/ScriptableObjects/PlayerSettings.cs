using System;
using System.IO;

using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class BaseSettings
{
    [Header("Audio")] // 0.0001 - 1
    public float masterVolume;
    public float soundVolume;
    public float musicVolume;

    [Header("Video")]
    public int resolution; // 0+
    public int quality; // 0+
    public bool fullscreen;
    public float hudScale; // 0.00 - 1.00
}

[CreateAssetMenu(fileName = "NewPlayerSettings", menuName = "Lights Out/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public BaseSettings baseSettings;

    [Header("Miscellaneous")]
    public string fileName = "SettingsData.Json";
    private string filePath;

    [Header("Events")]
    public UnityEvent initalizeEvent;

    [Header("Debugging")]
    public bool deleteFileOnStart;

    [HideInInspector] public Resolution[] resolutions;

    // DEFAULTS //
    [HideInInspector] public float defaultMasterVolume;
    [HideInInspector] public float defaultSoundVolume;
    [HideInInspector] public float defaultMusicVolume;

    [HideInInspector] public int defaultResolution;
    [HideInInspector] public int defaultQuality;
    [HideInInspector] public bool defaultFullscreen;
    [HideInInspector] public float defaultHudScale;

    public void ApplySettings(BaseSettings newerSettings)
    {
        if (newerSettings == baseSettings)
        {
            Debug.LogWarning("No additional changes have been made, skipping...");
            return;
        }

        baseSettings = newerSettings;
        DataManager dataInst = new();

        dataInst.SaveAllData(baseSettings, filePath);
    }

    private void SetDefaults()
    {
        defaultMasterVolume = baseSettings.masterVolume;
        defaultSoundVolume = baseSettings.soundVolume;
        defaultMusicVolume = baseSettings.musicVolume;

        defaultResolution = baseSettings.resolution;
        defaultQuality = baseSettings.quality;
        defaultFullscreen = baseSettings.fullscreen;
        defaultHudScale = baseSettings.hudScale;
    }

    private void ManageData()
    {
        SetDefaults();

        #region Setup

        filePath = Application.persistentDataPath + "\\" + fileName;
        DataManager dataInst = new();

        #endregion

        #region Debugging

        if (deleteFileOnStart && File.Exists(filePath))
        {
            Debug.LogWarning("Debug Deletion Enabled, Deleting: " + filePath);
            File.Delete(filePath);
        }

        #endregion

        #region Read Data

        if (!File.Exists(filePath))
        {
            Debug.Log("SettingsData.json file has been created in Directory: " + Application.persistentDataPath + "\\");
            dataInst.SaveAllData(baseSettings, filePath);
        }

        else
        {
            string jsonFileContents = File.ReadAllText(filePath);
            baseSettings = dataInst.GetData(jsonFileContents);
        }

        #endregion
    }

    public void BeginDataInitalization()
    {
        resolutions = Screen.resolutions;
        ManageData();

        if (initalizeEvent != null)
            initalizeEvent.Invoke();
        else
            Debug.LogWarning("Initalize Event Empty, cannot invoke an empty event!");
    }
}
