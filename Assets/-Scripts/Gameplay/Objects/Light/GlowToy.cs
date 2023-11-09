using System.Threading.Tasks;
using UnityEngine;

public class GlowToy : GenericLight
{
    #region Definitions
    
    private float FadeModifier { get => GameManager.Instance.GameSettings.GlowToyFadeModifier; }
    private float MaxFadeIn { get => GameManager.Instance.GameSettings.GlowToyFadeIn; }
    private float MaxBattery { get => GameManager.Instance.GameSettings.GlowToyMaxBattery; }
    private float BatteryTickRate { get => GameManager.Instance.GameSettings.GlowToyBatteryTickRate; }
    private float BatteryTickAmount { get => GameManager.Instance.GameSettings.GlowToyBatteryTickAmount; }
    private float DebounceModifier { get => GameManager.Instance.GameSettings.GlowToyDebounceModifier; }

    // These are shown in the inspector temporarily for debug reasons, they're not meant to be directly editable!
    public float CurrentFadeIn;
    public float CurrentBattery;
    public float CurrentDebounce;

    public bool HoldingInputDown = false;
    public bool WaitingForRelease = false;

    // public AudioManager audioManager;

    #endregion

    #region Glow-Toy Methods

    private void HandleInput(bool value)
    {
        HoldingInputDown = value;
    }

    private async void HandleBattery()
    {
        if (CurrentBattery <= 0)
        {
            // audioManager.Play("GlowToyBatteryLost", false);
            CurrentBattery = 0;
            Toggle();

            return;
        }

        await Task.Delay((int)BatteryTickRate * 1000);
        CurrentBattery -= BatteryTickAmount * Time.deltaTime;
    }

    #endregion

    #region Public Methods

    public async Task WaitForManagers()
    {
        while (InputManager.Instance == null || GameManager.Instance == null)
            await Task.Yield();
    }

    #endregion

    #region Unity Methods

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

    private void Start()
    {
        CurrentBattery = MaxBattery;
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        ChangeMaterial();
    }

    private void Update()
    {
        if (isOn)
        {
            if (!HoldingInputDown && WaitingForRelease)
                WaitingForRelease = false;

            if (HoldingInputDown && !WaitingForRelease)
            {
                CurrentDebounce = 1.0f;
                Toggle();

                return;
            }

            HandleBattery();
            return;
        }

        if (CurrentDebounce > 0.0f)
        {
            CurrentDebounce -= DebounceModifier * Time.deltaTime;
            
            if (HoldingInputDown)
                HoldingInputDown = false;

            if (CurrentDebounce < 0.0f)
                CurrentDebounce = 0.0f;
            
            return;
        }

        if (HoldingInputDown && CurrentBattery > 0)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            if (CurrentFadeIn < MaxFadeIn)
            {
                // audioName : string, looping : boolean
                //audioManager.Play("GlowToyRising", false);
                
                CurrentFadeIn += FadeModifier * Time.deltaTime;
                return;
            }

            CurrentFadeIn = 0.0f;
            WaitingForRelease = true;
            Toggle();

            return;
        }

        if (CurrentFadeIn > 0.0f)
        {
            CurrentFadeIn -= FadeModifier * Time.deltaTime;
            return;
        }

        CurrentFadeIn = 0.0f;
    }

    #endregion
}
