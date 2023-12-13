using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [field: Header("Generic")]
    [field: SerializeField] public GameObject Menu { get; set; }

    [field: Header("Buttons")]
    [field: SerializeField] public Button ResumeBtn { get; set; }
    [field: SerializeField] public Button SettingsBtn { get; set; }
    [field: SerializeField] public Button MainMenuBtn { get; set; }

    private bool IsPaused = true;

    public void HandleInput(bool input)
    {
        IsPaused = !IsPaused;
        GameManager.Instance.PlayerData.InMenu = IsPaused;
    }

    public void ResumeGame()
    {
        Menu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        Menu.SetActive(true);
        Time.timeScale = 0f;
    }

    private async Task WaitForManagers()
    {
        while (InputManager.Instance == null || GameManager.Instance == null)
            await Task.Yield();
    }
    
    private void OnEnable()
    {
        HelperFunctions.WaitForTask(WaitForManagers(), () =>
        {
            InputManager.Instance.Player_Pause.AddListener(HandleInput);
        });
    }

    private void OnDisable()
    {
        if (InputManager.Instance)
            InputManager.Instance.Player_Pause.RemoveListener(HandleInput);
    }

    private void Update()
    {
        if (IsPaused)
        {
            ResumeGame();
            return;
        }
        
        PauseGame();
    }

    private void Awake()
    {
        if (Menu == null)
        {
            Debug.Log("No Menu GameObject was attached to PauseMenuManager.cs", this);
            return;
        }

        if (Menu.activeSelf)
            Menu.SetActive(false);

        ResumeBtn.onClick.AddListener(delegate { HandleInput(false); });
        SettingsBtn.onClick.AddListener(delegate { Debug.LogWarning("Hasn't been implemented yet!"); });
        MainMenuBtn.onClick.AddListener(delegate { Debug.LogWarning("Hasn't been implemented yet!"); });
    }
}
