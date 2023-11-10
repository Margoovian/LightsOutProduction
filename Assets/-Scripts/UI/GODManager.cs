using UnityEngine;
using UnityEngine.UI;

public class GODManager : MonoBehaviour
{
    public Button restartBtn;
    public Button backBtn;

    private void Start()
    {
        GameManager.Instance.PlayerData.InMenu = true;
        restartBtn.onClick.AddListener(delegate { SceneController.Instance.LoadSpecific(0); });
        backBtn.onClick.AddListener(delegate { SceneController.Instance.LoadSpecific(1); });
    }
}