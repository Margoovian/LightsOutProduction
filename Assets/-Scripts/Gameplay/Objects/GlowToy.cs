using System.Threading.Tasks;
using UnityEngine;

public class GlowToy : MonoBehaviour
{
    private bool IsOn { get; set; }
    private float FadeModifier { get => GameManager.Instance.GameSettings.GlowToyFadeModifier; }
    private float MaxFadeIn { get => GameManager.Instance.GameSettings.GlowToyFadeIn; }
    private float MaxBattery { get => GameManager.Instance.GameSettings.GlowToyMaxBattery; }

    private float CurrentFadeIn;
    private float CurrentBattery;
    private bool HoldingInputDown = false;

    public async Task WaitForManagers()
    {
        while (InputManager.Instance == null || GameManager.Instance == null )
            await Task.Yield();
    }

    private void HandleInput(bool value)
    {
        HoldingInputDown = value;
    }

    private void OnEnable()
    {
        HelperFunctions.WaitForTask(WaitForManagers(), () =>
        {
            InputManager.Instance.Player_Glowtoy.AddListener(HandleInput);
        });
    }

    private void OnDisable()
    {
        if (InputManager.Instance)
            InputManager.Instance.Player_Glowtoy.RemoveListener(HandleInput);
    }

    private void Update()
    {
        if (IsOn)
        {
            // do code here!
            return;
        }

        Debug.Log(CurrentFadeIn);

        if (HoldingInputDown)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            if (CurrentFadeIn < MaxFadeIn)
            {
                CurrentFadeIn += FadeModifier * Time.deltaTime;
                return;
            }

            CurrentFadeIn = 0.0f;
            IsOn = true;
        }

        else
        {
            if (CurrentFadeIn > 0.0f)
            {
                CurrentFadeIn -= FadeModifier * Time.deltaTime;
                return;
            }

            CurrentFadeIn = 0.0f;
        }

        return;
    }
}
