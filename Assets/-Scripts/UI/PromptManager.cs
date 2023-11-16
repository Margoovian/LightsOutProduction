using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using TMPro;

public class PromptManager : MonoBehaviour
{
    [Header("Main")]
    public GameObject frame;
    
    [Header("Text")]
    public TMP_Text title;
    public TMP_Text body;

    [Header("Buttons")]
    public Button acceptBtn;
    public Button denyBtn;

    // Private Definitions
    private UnityEvent<bool, string> passthroughEvent;
    internal string currentEventName;

    private string defaultTitle;
    private string defaultBody;

    public static PromptManager Instance { get; internal set; }

    #region Public Functions

    public void HookButtons(string eventName)
    {
        currentEventName = eventName;

        acceptBtn.onClick.AddListener(() => { HandleInteraction(true, eventName); });
        denyBtn.onClick.AddListener(() => { HandleInteraction(false, eventName); });
    }

    public void UnhookButtons()
    {
        acceptBtn.onClick.RemoveAllListeners();
        denyBtn.onClick.RemoveAllListeners();

        currentEventName = string.Empty;
    }

    public void SetEvent(UnityEvent<bool, string> unityEvent)
    {
        passthroughEvent = unityEvent;
    }

    public void SetFrameVisibility(bool toggle)
    {
        frame.SetActive(toggle);
    }

    #region Easy Setup

    public void StartPrompt(string eventName, UnityEvent<bool, string> unityEvent)
    {
        HookButtons(eventName);
        SetEvent(unityEvent);
        SetFrameVisibility(true);
    }

    public void StartPrompt(string title, string body, string eventName, UnityEvent<bool, string> unityEvent)
    {
        SetTitle(title);
        SetBody(body);

        HookButtons(eventName);
        SetEvent(unityEvent);

        SetFrameVisibility(true);
    }

    public void StartPrompt(string title, string body, string yesBtnText, string noBtnText, string eventName, UnityEvent<bool, string> unityEvent)
    {
        SetTitle(title);
        SetBody(body);

        SetAcceptBtnText(yesBtnText);
        SetDenyBtnText(noBtnText);

        HookButtons(eventName);
        SetEvent(unityEvent);

        SetFrameVisibility(true);
    }

    #endregion

    #endregion

    #region Private Functions

    private void SetTitle(string title)
    {
        if (title == string.Empty)
            return;

        this.title.text = title;
    }

    private void SetBody(string body)
    {
        if (body == string.Empty)
            return;

        this.body.text = body;
    }

    private void SetAcceptBtnText(string text)
    {
        if (text == string.Empty)
            return;

        TMP_Text result = acceptBtn.GetComponentInChildren<TMP_Text>();

        if (result == null)
            return;

        result.text = text;
    }

    private void SetDenyBtnText(string text)
    {
        if (text == string.Empty)
            return;

        TMP_Text result = denyBtn.GetComponentInChildren<TMP_Text>();

        if (result == null)
            return;

        result.text = text;
    }

    private void RevertTitle()
    {
        title.text = defaultTitle;
    }

    private void RevertBody()
    {
        body.text = defaultBody;
    }

    private void RevertApplyBtnText()
    {
        SetAcceptBtnText("Yes");
    }

    private void RevertDenyBtnText()
    {
        SetDenyBtnText("No");
    }

    private void HandleInteraction(bool toggle, string eventName)
    {
        passthroughEvent.Invoke(toggle, eventName);

        RevertTitle();
        RevertBody();

        RevertApplyBtnText();
        RevertDenyBtnText();

        SetFrameVisibility(false);
        UnhookButtons();
    }

    private void Start()
    {
        if (!Instance)
            Instance = this;

        defaultTitle = title.text;
        defaultBody = body.text;
    }

    #endregion
}
