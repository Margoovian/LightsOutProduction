using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Delay before Splash Screen starts formatted in Seconds.")]
    public int startupDelay;

    [Tooltip("The Background of the Splash Logo, used as Transition")]
    public Image background;

    [Tooltip("Hide the Mouse Cursor during the Splash Screen")]
    public bool hideCursor;

    [Header("Assets")]
    [Tooltip("The Logo used within the Splash Screen.")]
    public Image logo;

    [Tooltip("The Title Screen loaded before the Splash Logo Transition occurs")]
    public GameObject titleUI;

    private async void Start()
    {
        if (titleUI.activeSelf)
            titleUI.SetActive(false);

        if (startupDelay < 0)
            startupDelay = 0;

        if (hideCursor)
            Cursor.visible = false;

        if (logo.color.a > 0)
        {
            Color newColor = new()
            {
                a = 0
            };  

            logo.color = newColor;
        }

        await Task.Delay(startupDelay * 1000);

        Task task = FadeImage(logo, true);
        await task;

        await Task.Delay(1500);

        if (!titleUI.activeSelf)
            titleUI.SetActive(true);

        Task _ = FadeImage(logo, false);
        
        await Task.Delay(500);

        task = SizeImageY(background, Screen.currentResolution.height);
        await task;

        if (!Cursor.visible)
            Cursor.visible = true;

        gameObject.SetActive(false);
    }

    private async Task FadeImage(Image image, bool fadeIn)
    {
        Color UpdateAlpha(float i)
        {
            return new Color(1, 1, 1, i);
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

    private async Task SizeImageY(Image image, float yRes)
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            image.rectTransform.offsetMax = new Vector2(
                image.rectTransform.offsetMin.x, 
                -Mathf.Lerp(
                    0, 
                    yRes, 
                    i
                    )
                );

            await Task.Yield();
        }
    }
}
