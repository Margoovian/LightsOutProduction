using UnityEngine;
using UnityEngine.UI;

public class EndScreenInfo : MonoBehaviour
{
    [System.Serializable]
    public class DeathVisual
    {
        public GameOverType Type;
        public string Text;
    }

    [System.Serializable]
    public class RankSprite
    {
        public EndRatingEnum Rank;
        public Sprite Sprite;
    }

    [Header("Backgrounds")]
    [SerializeField] private Sprite Win;
    [SerializeField] private Sprite Lose;

    [Header("Assets")]
    [SerializeField] private Image Rating;
    [SerializeField] private TMPro.TMP_Text Battery;
    [SerializeField] private TMPro.TMP_Text TimeLeft;
    [SerializeField] private TMPro.TMP_Text Levels;

    [Header("Control")]
    [SerializeField] private Button RestartBtn;
    [SerializeField] private Button ExitBtn;
    [SerializeField] private Button WinExitBtn;

    [Header("Miscellaneous")]
    [SerializeField] private GameObject MainGui;
    [SerializeField] private Image Background;
    [SerializeField] private GameObject Container;
    [SerializeField] private TMPro.TMP_Text DeathType;

    [Header("Arrays")]
    [SerializeField] private DeathVisual[] DeathVisuals;

    // Unused for the moment, will be used once all the Rank sprites have been made and imported!
    [SerializeField] private RankSprite[] RankSprites;

    private bool selected;

    private void ResetVariables()
    {
        selected = true;
        GODController.Instance.Triggered = false;

        PlayerData.Instance.FearLevel = 0.0f;
        PlayerData.Instance.BatteryLife = GameManager.Instance.GameSettings.GlowToyMaxBattery;

        GameManager.Instance.GameOverType = GameOverType.None;
    }

    public void Restart() 
    { 
        if (selected)
            return;

        GameManager.Instance.PlayerData.InMenu = false;
        SceneController.Instance.LoadSpecific(GameManager.Instance.PreviousGameScene, ResetVariables); 
    }

    public void Exit() 
    { 
        if (selected) 
            return;
        
        SceneController.Instance.LoadSpecific(SceneController.Instance.GetStartIndex(), ResetVariables); 
    }

    private void Start()
    {
        if (!GameManager.Instance.PlayerData.InMenu)
            GameManager.Instance.PlayerData.InMenu = true;

        RestartBtn.onClick.AddListener(Restart);
        ExitBtn.onClick.AddListener(Exit);

        MainGui.SetActive(false);

        RestartBtn.gameObject.SetActive(GameManager.Instance.GameOverType != GameOverType.Win);
        ExitBtn.gameObject.SetActive(GameManager.Instance.GameOverType != GameOverType.Win);

        WinExitBtn.gameObject.SetActive(GameManager.Instance.GameOverType == GameOverType.Win);
        Container.SetActive(GameManager.Instance.GameOverType == GameOverType.Win);

        if (GameManager.Instance.GameOverType != GameOverType.Win)
        {
            DeathType.enabled = true;
            Background.sprite = Lose;

            foreach (DeathVisual visual in DeathVisuals)
            {
                if (visual.Type == GameManager.Instance.GameOverType)
                {
                    DeathType.text = visual.Text;
                    break;
                }
            }

            return;
        }

        Background.sprite = Win;
        WinExitBtn.onClick.AddListener(Exit);

        EndRatingEnum rating = GameManager.Instance.CalcEndScore();

        RankSprite target = null;
        foreach (RankSprite i in RankSprites)
        {
            if (i.Rank == rating)
            {
                target = i;
                break;
            }
        }

        if (Levels)
            Levels.text = $"{GameManager.Instance.PuzzlesCompleted}";

        if (TimeLeft)
            TimeLeft.text = $"{System.MathF.Round(GameManager.Instance.PlayerData.ElapsedTime / 60, 2)}m";

        if (Battery)
            Battery.text = $"{(int)(GameManager.Instance.PlayerData.BatteryLife / GameManager.Instance.GameSettings.GlowToyMaxBattery * 100)}%";

        if (target == null)
        {
            Debug.Log("Couldn't find 'RankSprite' for Rank: " + rating.ToString(), this);
            Rating.color = new Color(0, 0, 0, 0);
            return;
        }

        Rating.sprite = target.Sprite;
    }
}
