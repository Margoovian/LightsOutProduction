using UnityEngine.SceneManagement;

public class GODController
{
    private static GODController instance;

    public static GODController Instance
    {
        get
        {
            instance ??= new GODController();
            return instance;
        }
    }

    public bool triggered;

    public void Initalize()
    {
        instance ??= new();
    }

    public void Update()
    {
        if (GameManager.Instance.PlayerData.FearLevel >= GameManager.Instance.GameSettings.MaxFear && !triggered)
        {
            triggered = true;

            if (PlayerData.Instance.InFearWall)  SceneManager.LoadScene("GameOverWall");
            else SceneManager.LoadScene("GameOverFear");

        } else if (SceneController.Instance.GetCurrentIndex() == 0 && GameManager.Instance.PlayerData.ElapsedTime > 300)
        {
            triggered = true;
            SceneManager.LoadScene("GameOverTime");
        }
    }
}
