using UnityEngine;
using UnityEngine.UI;

public class GODManager : MonoBehaviour
{
    public Button RestartBtn;
    public Button BackBtn;

    public void ResetVariables()
    {
        GODController.Instance.Triggered = false;
        
        PlayerData.Instance.FearLevel = 0.0f;
        PlayerData.Instance.BatteryLife = GameManager.Instance.GameSettings.GlowToyMaxBattery;

        GameManager.Instance.GameOverType = GameOverType.None;
    }

    private void RestartSelected()
    {
        Debug.Log(SceneController.Instance.GetCurrentIndex(), this);
        Debug.Log(SceneController.Instance.GetStartIndex(), this);

        SceneController.Instance.LoadSpecific(SceneController.Instance.GetCurrentIndex(), ResetVariables);
    }

    private void BackSelected()
    {
        SceneController.Instance.LoadSpecific(SceneController.Instance.GetStartIndex(), ResetVariables);
    }

    private void Start()
    {
        GameManager.Instance.PlayerData.InMenu = true;
        RestartBtn.onClick.AddListener(RestartSelected);
        BackBtn.onClick.AddListener(BackSelected);
    }
}