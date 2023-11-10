using System;
using System.Threading.Tasks;
using UnityEngine;

public class GlowToy : GenericLight
{
    #region Definitions

    [Serializable]
    public class BatteryUI
    {
        public Sprite sprite;
        public int percentage;
    }

    private float FadeModifier { get => GameManager.Instance.GameSettings.GlowToyFadeModifier; }
    private float MaxFadeIn { get => GameManager.Instance.GameSettings.GlowToyFadeIn; }
    private float MaxBattery { get => GameManager.Instance.GameSettings.GlowToyMaxBattery; }
    private float BatteryTickRate { get => GameManager.Instance.GameSettings.GlowToyBatteryTickRate; }
    private float BatteryTickAmount { get => GameManager.Instance.GameSettings.GlowToyBatteryTickAmount; }
    private float DebounceModifier { get => GameManager.Instance.GameSettings.GlowToyDebounceModifier; }
    public float CurrentBattery {
        get => PlayerData.Instance.BatteryLife;

        set { 
            PlayerData.Instance.BatteryLife = value; 
        } 
    }

    private float CurrentFadeIn;
    private float CurrentDebounce;

    private bool HoldingInputDown = false;
    private bool WaitingForRelease = false;

    public Light GlowToyLight;
    // public AudioManager audioManager;
    public SpriteRenderer BatteryUIRenderer;

    private const int BatteryUICap = 4;
    private int CurrentBatteryValue = 0;

    public BatteryUI[] BatteryUISprites = new BatteryUI[BatteryUICap];
    //public int[] BatteryUIStates = new int[BatteryUICap];

    #endregion

    #region Glow-Toy Methods

    private void HandleInput(bool value)
    {
        HoldingInputDown = value;
    }

    private void ToggleLight(bool value)
    {
        GlowToyLight.enabled = value;
    }

    private void UpdateBatteryUI()
    {
        BatteryUIRenderer.enabled = isOn;
        BatteryUIRenderer.transform.LookAt(GameManager.Instance.Camera.transform);

        // Figure out a way to condense this with math later down the road!
        for (int i = 0; i < BatteryUISprites.Length; i++)
        {
            if (BatteryUISprites[i].percentage == Math.Round(CurrentBattery))
            {
                CurrentBatteryValue = i;
                break;
            }
        }

        //int nearest = BatteryUIStates.OrderBy(x => Math.Abs(x - CurrentBattery)).First();
        //int index = Array.FindIndex(BatteryUIStates, row => row == nearest);

        //Debug.Log(nearest + " | " + index);
        BatteryUIRenderer.sprite = BatteryUISprites[CurrentBatteryValue].sprite;
    }

    private async void HandleBattery()
    {
        if (CurrentBattery <= 0)
        {
            // audioManager.Play("GlowToyBatteryLost", false);
            CurrentBattery = 0;
            
            Toggle();
            ToggleLight(false);

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
        // improve this at some point in the future asap!
        if (MaxFadeIn == 0 && FadeModifier == 0 
            && MaxBattery == 0 && BatteryTickRate == 0 
            && BatteryTickAmount == 0 && DebounceModifier == 0)
        {
            Debug.LogWarning("Cannot initalize values, please fix this by editing the difficulty scriptable objects via: Assets > ScriptableObjects!", this);
            return;
        }

        if (GlowToyLight == null)
        {
            Debug.LogWarning("GlowToyLight wasn't applied, please fix this!", this);
            return;
        }

        if (GlowToyLight.enabled)
            ToggleLight(false);

        if (BatteryUIRenderer == null)
        {
            Debug.LogWarning("No SpriteRenderer was attached to GlowToy, please fix!", this);
            return;
        }

        //if (BatteryUISprites.Length != BatteryUICap)
        //{
        //    Debug.LogWarning("You can only have " + BatteryUICap.ToString() + " amounts of sprites at a given time using the BatteryUISprites array.", this);
        //    return;
        //}

        //if (BatteryUIStates.Length != BatteryUICap)
        //{
        //    Debug.LogWarning("You can only have " + BatteryUICap.ToString() + " amounts of states at a given time using the BatteryUIStates array.", this);
        //    return;
        //}

        //if (BatteryUIRenderer.sprite == null)
        //    BatteryUIRenderer.sprite = BatteryUISprites[0];

        if (BatteryUIRenderer.enabled)
            BatteryUIRenderer.enabled = false;

        CurrentBattery = MaxBattery;
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        ChangeMaterial();
    }

    private void Update()
    {
        UpdateBatteryUI();

        if (isOn)
        {
            if (!HoldingInputDown && WaitingForRelease)
                WaitingForRelease = false;

            if (HoldingInputDown && !WaitingForRelease)
            {
                CurrentDebounce = 1.0f;

                Toggle();
                ToggleLight(false);

                return;
            }

            ToggleLight(true);
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
            ToggleLight(true);

            return;
        }

        ToggleLight(false);

        if (CurrentFadeIn > 0.0f)
        {
            CurrentFadeIn -= FadeModifier * Time.deltaTime;
            return;
        }

        CurrentFadeIn = 0.0f;
    }

    #endregion
}
