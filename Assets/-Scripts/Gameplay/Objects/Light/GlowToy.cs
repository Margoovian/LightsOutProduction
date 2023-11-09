using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GlowToy : GenericLight
{
    #region Definitions
    
    private float FadeModifier { get => GameManager.Instance.GameSettings.GlowToyFadeModifier; }
    private float MaxFadeIn { get => GameManager.Instance.GameSettings.GlowToyFadeIn; }
    private float MaxBattery { get => GameManager.Instance.GameSettings.GlowToyMaxBattery; }
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

    private void ToggleLight()
    {

    }

    #endregion

    #region Public Methods

    public async Task WaitForManagers()
    {
        while (InputManager.Instance == null || GameManager.Instance == null )
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
        Debug.Log(isOn);

        if (isOn)
        {
            if (!HoldingInputDown && WaitingForRelease)
                WaitingForRelease = false;

            if (HoldingInputDown && !WaitingForRelease)
            {
                CurrentDebounce = 1.0f;
                
                isOn = false;
                Toggle();

                return;
            }

            // do battery code here!

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

        if (HoldingInputDown)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            if (CurrentFadeIn < MaxFadeIn)
            {
                // audioName : string, playOnce : boolean
                //audioManager.Play("GlowToyRising", true);
                
                CurrentFadeIn += FadeModifier * Time.deltaTime;
                return;
            }

            CurrentFadeIn = 0.0f;
            WaitingForRelease = true;
            
            isOn = true;
            Toggle();
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

    #endregion
}
