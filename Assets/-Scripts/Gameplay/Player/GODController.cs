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
            SceneManager.LoadScene("GameOverDeath");
        }
    }
}
