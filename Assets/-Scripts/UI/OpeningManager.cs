using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

public class OpeningManager : MonoBehaviour
{
    // FIELDS //
    [field: Header("Assets")]
    [Tooltip("The Interval (In Seconds!) after Fading in and before fading out of the scene.")]
    [field: SerializeField] public int IntervalDuration { get; set; }

    [Tooltip("Hide the Mouse Cursor during the Splash Screen.")]
    [field: SerializeField] public bool HideCursor { get; set; }

    [field: Header("Assets")]
    [Tooltip("The Background that fades in and out behind the opening image.")]
    [field: SerializeField] public Image Background { get; set; }

    [Tooltip("The Image used as a foreground, fades in and out with the background at a different interval.")]
    [field: SerializeField] public Image OpeningImage { get; set; }

    [Tooltip("The Name of the Music to fade out before the gameplay begins. (Make sure this aligns with the SplashManager's 'MusicName' string!")]
    [field: SerializeField] public string MusicName { get; set; }

    private async Task FadeImage(Image image, bool fadeIn)
    {
        Color UpdateAlpha(float i)
        {
            return new() { 
                r = image.color.r,
                g = image.color.g,
                b = image.color.b,
                a = i 
            };
        }

        if (!fadeIn)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                image.color = UpdateAlpha(i);
                await Task.Yield();
            }

            return;
        }

        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            image.color = UpdateAlpha(i);
            await Task.Yield();
        }
    }

    private async void Start()
    {
        IntervalDuration *= 1000;

        if (Background.color.a > 0)
            Background.color = new() { a = 0 };

        if (OpeningImage.color.a > 0)
        {
            OpeningImage.color = new()
            {
                r = OpeningImage.color.r,
                g = OpeningImage.color.g,
                b = OpeningImage.color.b,
                a = 0
            };
        }

        if (HideCursor)
            Cursor.visible = false;

        AudioManager.Instance.FadeOut("TestMusic");

        Task task1 = FadeImage(Background, true);
        await task1;

        Task task2 = FadeImage(OpeningImage, true);
        await task2;

        await Task.Delay(IntervalDuration);

        task2 = FadeImage(OpeningImage, false);
        await task2;

        await Task.Delay(IntervalDuration / 4);
        MainMenu.Instance.OnPlay();

        if (HideCursor)
            Cursor.visible = true;
    }
}
