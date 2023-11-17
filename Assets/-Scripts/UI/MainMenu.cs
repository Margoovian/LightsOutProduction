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
    [Header("Main")]
    public EventParams[] eventParams = new EventParams[2];

    [Header("Buttons")]
    public Button playButton;
    public Button optionsButton;
    public Button leaveButton;

    [field: Header("Uncategorized")]
    [field: SerializeField] public GameObject GameGUI { get; set; }
    [field: SerializeField] public GameObject SettingsUI { get; set; }

    private Canvas _menuGUI;

    private UnityEvent<bool, string> passthroughEvent1;
    private UnityEvent<bool, string> passthroughEvent2;

    private void HandleQuitting(bool result, string eventName)
    {
        if (!result)
            return;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    private void HandlePlaying(bool result, string eventName)
    {
        if (!result)
            return;

        GameManager.Instance.PlayerData.InMenu = false;
        
        if (GameGUI != null) 
            GameGUI.SetActive(true);

        AudioManager.Instance.Stop("TestMusic");
        SceneController.Instance.LoadSpecific(0);
    }

    private void Start()
    {
        if (passthroughEvent1 == null)
        {
            passthroughEvent1 = new();
            passthroughEvent1.AddListener(HandleQuitting);
        }

        if (passthroughEvent2 == null)
        {
            passthroughEvent2 = new();
            passthroughEvent2.AddListener(HandlePlaying);
        }

        if (GameGUI != null) 
            GameGUI.SetActive(false);
        
        _menuGUI = GetComponent<Canvas>();

        playButton.onClick.AddListener(OnPlay);
        optionsButton.onClick.AddListener(OnOptions);
        leaveButton.onClick.AddListener(OnLeave);
    }

    public void OnPlay()
    {
        PromptManager.Instance.StartPrompt(eventParams[0].Title, eventParams[0].Body, eventParams[0].ApplyBtnText, eventParams[0].DenyBtnText, eventParams[0].EventName, passthroughEvent2);
    }

    public void OnOptions()
    {
        if (SettingsUI.activeSelf)
            return;

        SettingsUI.SetActive(true);
    }

    public void OnLeave()
    {
        PromptManager.Instance.StartPrompt(eventParams[1].Title, eventParams[1].Body, eventParams[1].ApplyBtnText, eventParams[1].DenyBtnText, eventParams[1].EventName, passthroughEvent1);
    }
}
