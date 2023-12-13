using System.Threading.Tasks;

using TMPro;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class OpeningManager : MonoBehaviour
{
    // FIELDS //
    [field: Header("Settings")]
    [Tooltip("The Interval (In Seconds!) after Fading in and before fading out of the scene.")]
    [field: SerializeField] public float IntervalDuration { get; set; } = 3.0f;

    [Tooltip("Mouse to next scene Text Label.")]
    [field: SerializeField] public TMP_Text TextLabel { get; set; }

    [Tooltip("Hide the Mouse Cursor during the Splash Screen.")]
    [field: SerializeField] public bool HideCursor { get; set; } = true;

    [Tooltip("Determine whether or not the player should have to use their mouse to continue to the next scene.")]
    [field: SerializeField] public bool UseMouse { get; set; } = true;

    [Tooltip("(Only Checked if UseMouse is enabled!)")]
    [field: SerializeField] public KeyCode ContinueKey { get; set; } = KeyCode.Space;

    [field: Header("Assets")]
    [Tooltip("The Background that fades in and out behind the opening image.")]
    [field: SerializeField] public Image Background { get; set; }

    [Tooltip("The Image used as a foreground, fades in and out with the background at a different interval.")]
    [field: SerializeField] public Image OpeningImage { get; set; }

    [Tooltip("The Name of the Music to fade out before the gameplay begins. (Make sure this aligns with the SplashManager's 'MusicName' string!")]
    [field: SerializeField] public string MusicName { get; set; }

    private bool canClick = false;

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

    private async Task FadeText(TMP_Text text, bool fadeIn)
    {
        Color UpdateAlpha(float i)
        {
            return new()
            {
                r = text.color.r,
                g = text.color.g,
                b = text.color.b,
                a = i
            };
        }

        if (!fadeIn)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                text.color = UpdateAlpha(i);
                await Task.Yield();
            }

            return;
        }

        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            text.color = UpdateAlpha(i);
            await Task.Yield();
        }
    }

    private async void StartGame()
    {
        Task task = FadeImage(OpeningImage, false);
        
        if (!UseMouse)
            await task;
        else
        {
            Task subTask = FadeText(TextLabel, false);
            await subTask;
        }

        if (HideCursor && !Cursor.visible)
            Cursor.visible = true;

        MainMenu.Instance.OnPlay();
    }

    private async void Start()
    {
        IntervalDuration *= 1000.0f;
        TextLabel.text = TextLabel.text.Replace("{KEY}", ContinueKey.ToString());

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

        if (UseMouse && TextLabel.color.a > 0)
        {
            TextLabel.color = new()
            {
                r = TextLabel.color.r,
                g = TextLabel.color.g,
                b = TextLabel.color.b,
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

        await Task.Delay((int)IntervalDuration);

        if (!UseMouse)
        {
            task2 = FadeImage(OpeningImage, false);
            await task2;

            await Task.Delay((int)IntervalDuration / 4);

            StartGame();

            return;
        }

        if (HideCursor)
            Cursor.visible = true;

        Task task3 = FadeText(TextLabel, true);
        await task3;

        canClick = true;
    }

    private void Update()
    {
        if (!canClick)
            return;

        if (Mouse.current.leftButton.isPressed || Input.GetKeyDown(ContinueKey))
        {
            canClick = false;
            StartGame();
        }
    }
}
