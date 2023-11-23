using UnityEngine;
using UnityEngine.UI;

public class GODManager : MonoBehaviour
{
    public Button restartBtn;
    public Button backBtn;

    public void ResetVariables()
    {
        GODController.Instance.triggered = false;
        PlayerData.Instance.FearLevel = 0.0f;
        PlayerData.Instance.BatteryLife = GameManager.Instance.GameSettings.GlowToyMaxBattery;
    }

    private void RestartSelected() => SceneController.Instance.LoadSpecific(SceneController.Instance.GetCurrentIndex(), ResetVariables);
    private void BackSelected() => SceneController.Instance.LoadSpecific(SceneController.Instance.GetStartIndex(), ResetVariables);

    private void Start()
    {
        GameManager.Instance.PlayerData.InMenu = true;
        backBtn.onClick.AddListener(BackSelected);
        restartBtn.onClick.AddListener(RestartSelected);
    }
}