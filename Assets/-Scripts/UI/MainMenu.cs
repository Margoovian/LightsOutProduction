using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[Serializable]
public class EventParams
{
    public string Title;
    public string Body;
    public string ApplyBtnText;
    public string DenyBtnText;
    public string EventName;
}

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; set; }

    [Header("Main")]
    public EventParams eventParams;

    [Header("Buttons")]
    public Button playButton;
    public Button optionsButton;
    public Button leaveButton;

    [field: Header("Uncategorized")]
    [field: SerializeField] public GameObject GameGUI { get; set; }
    [field: SerializeField] public SettingsManager SettingsUI { get; set; }
    [field: SerializeField] public OpeningManager OpeningUI { get; set; }

    private UnityEvent<bool, string> passthroughEvent;

    private void HandleQuitting(bool result, string eventName)
    {
        if (!result)
            return;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    private void Start()
    {
        if (!Instance)
            Instance = this;

        if (passthroughEvent == null)
        {
            passthroughEvent = new();
            passthroughEvent.AddListener(HandleQuitting);
        }

        if (GameGUI != null) 
            GameGUI.SetActive(false);

        playButton.onClick.AddListener(delegate { OpeningUI.gameObject.SetActive(true); });

        optionsButton.onClick.AddListener(OnOptions);
        leaveButton.onClick.AddListener(OnLeave);
    }

    public void OnPlay()
    {
        GameManager.Instance.PlayerData.InMenu = false;

        if (GameGUI != null)
            GameGUI.SetActive(true);

        AudioManager.Instance.Stop("TestMusic");
        SceneController.Instance.LoadSpecific(0);
    }

    public void OnOptions()
    {
        if (SettingsUI.gameObject.activeSelf)
            return;

        SettingsUI.gameObject.SetActive(true);
    }

    public void OnLeave() => PromptManager.Instance.StartPrompt(eventParams.Title, eventParams.Body, eventParams.ApplyBtnText, eventParams.DenyBtnText, eventParams.EventName, passthroughEvent);
}
