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

    public bool triggered = false;

    public void Initalize()
    {
        instance ??= new();
    }

    public void Update()
    {
        if (GameManager.Instance.PlayerData.FearLevel >= 100 && !triggered)
        {
            triggered = true;
            SceneController.Instance.LoadSpecific(10);
        }
    }
}
