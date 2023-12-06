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
    [SerializeField] private TMPro.TMP_Text Rating;
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

    public void Restart() { if (selected) return; SceneController.Instance.LoadSpecific(GameManager.Instance.PreviousGameScene, ResetVariables); }

    public void Exit() { if (selected) return; SceneController.Instance.LoadSpecific(SceneController.Instance.GetStartIndex(), ResetVariables); }

    private void Start()
    {
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
        EndRatingEnum rating = GameManager.Instance.CalcEndScore();
        WinExitBtn.onClick.AddListener(Exit);

        // Change to use vertex colour instead of rich text
        // This will be replaced with colours associated on the images themselves!
        switch (rating)
        {
            case EndRatingEnum.F: Rating.text = $"<color=red>{rating}"; break;
            case EndRatingEnum.E: Rating.text = $"<color=grey>{rating}"; break;
            case EndRatingEnum.D: Rating.text = $"<color=grey>{rating}"; break;
            case EndRatingEnum.C: Rating.text = $"<color=white>{rating}"; break;
            case EndRatingEnum.B: Rating.text = $"<color=blue>{rating}"; break;
            case EndRatingEnum.A: Rating.text = $"<color=green>{rating}"; break;
            case EndRatingEnum.S: Rating.text = $"<color=pink>{rating}"; break;
            case EndRatingEnum.SS: Rating.text = $"<color=pink>{rating}"; break;
            case EndRatingEnum.R: Rating.text = $"<color=orange>{rating}"; break;
            case EndRatingEnum.RR: Rating.text = $"<color=orange>{rating}"; break;
            case EndRatingEnum.L: Rating.text = $"<color=yellow>{rating}"; break;
            default: Rating.text = $"<color=white>{rating}"; break;
        }

        if (Levels)
            Levels.text = $"{GameManager.Instance.PuzzlesCompleted}";

        if (TimeLeft)
            TimeLeft.text = $"{System.MathF.Round(GameManager.Instance.PlayerData.ElapsedTime / 60, 2)}m";

        if (Battery)
            Battery.text = $"{(int)(GameManager.Instance.PlayerData.BatteryLife / GameManager.Instance.GameSettings.GlowToyMaxBattery * 100)}%";
    }
}
