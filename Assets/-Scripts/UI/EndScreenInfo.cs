using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenInfo : MonoBehaviour
{

    [SerializeField] private TMPro.TMP_Text Rating;
    [SerializeField] private TMPro.TMP_Text Battery;
    [SerializeField] private TMPro.TMP_Text TimeLeft;
    [SerializeField] private TMPro.TMP_Text Levels;


    void Start()
    {
        EndRatingEnum rating = GameManager.Instance.CalcEndScore();

        switch(rating) {
            case EndRatingEnum.F: Rating.text = $"<color=red>{rating}";break;     // Change to use vertex colour instead of rich text
            case EndRatingEnum.E: Rating.text = $"<color=grey>{rating}";break;    // Change to use vertex colour instead of rich text
            case EndRatingEnum.D: Rating.text = $"<color=grey>{rating}";break;    // Change to use vertex colour instead of rich text
            case EndRatingEnum.C: Rating.text = $"<color=white>{rating}";break;   // Change to use vertex colour instead of rich text
            case EndRatingEnum.B: Rating.text = $"<color=blue>{rating}";break;    // Change to use vertex colour instead of rich text
            case EndRatingEnum.A: Rating.text = $"<color=green>{rating}";break;   // Change to use vertex colour instead of rich text
            case EndRatingEnum.S: Rating.text = $"<color=pink>{rating}";break;    // Change to use vertex colour instead of rich text
            case EndRatingEnum.SS: Rating.text = $"<color=pink>{rating}";break;   // Change to use vertex colour instead of rich text
            case EndRatingEnum.R: Rating.text = $"<color=orange>{rating}";break;  // Change to use vertex colour instead of rich text
            case EndRatingEnum.RR: Rating.text = $"<color=orange>{rating}";break; // Change to use vertex colour instead of rich text
            case EndRatingEnum.L: Rating.text = $"<color=yellow>{rating}";break;  // Change to use vertex colour instead of rich text
            default: Rating.text = $"<color=white>{rating}"; break;               // Change to use vertex colour instead of rich text
        }

        if (Levels) Levels.text = $"{GameManager.Instance.PuzzlesCompleted}";
        if (TimeLeft) TimeLeft.text = $"{System.MathF.Round(GameManager.Instance.PlayerData.ElapsedTime / 60,2)}m";
        if(Battery) Battery.text = $"{(int)(GameManager.Instance.PlayerData.BatteryLife / GameManager.Instance.GameSettings.GlowToyMaxBattery * 100)}%";

    }
}
