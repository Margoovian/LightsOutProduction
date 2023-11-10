using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private Canvas _menuGUI;
    [field: SerializeField] public GameObject GameGUI { get; set; }

    private void Start()
    {
        if (GameGUI != null) GameGUI.SetActive(false);
        _menuGUI = GetComponent<Canvas>();
    }

    public void onPlay()
    {
        GameManager.Instance.PlayerData.InMenu = false;
        if (GameGUI != null) GameGUI.SetActive(true);
        SceneController.Instance.LoadSpecific(0);

    }
    public void onOptions()
    {

    }
    public void onLeave()
    {

    }
}
