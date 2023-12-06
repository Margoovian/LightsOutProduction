using UnityEngine.SceneManagement;

public class GODController
{
    public bool Triggered { get; set; }

    private static GODController instance;
    public static GODController Instance
    {
        get
        {
            instance ??= new GODController();
            return instance;
        }
    }

    public void Initalize() => instance ??= new();

    public void Update()
    {
        if (Triggered)
            return;

        if (GameManager.Instance.PlayerData.FearLevel >= GameManager.Instance.GameSettings.MaxFear)
        {
            Triggered = true;

            if (PlayerData.Instance.InFearWall)
                GameManager.Instance.GameOver(GameOverType.FearWall);
            else
                GameManager.Instance.GameOver(GameOverType.FearWander);
        }

        else if (SceneController.Instance.GetCurrentIndex() == 0 && GameManager.Instance.PlayerData.ElapsedTime > 300)
        {
            Triggered = true;
            GameManager.Instance.GameOver(GameOverType.Timeout);
        }
    }
}
