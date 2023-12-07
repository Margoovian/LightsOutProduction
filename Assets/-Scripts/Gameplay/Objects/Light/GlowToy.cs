using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class GlowToy : MonoBehaviour
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
    private bool isOn { get => PlayerData.Instance.ToyOn; set => PlayerData.Instance.ToyOn = value; }
    public float CurrentBattery
    {
        get => PlayerData.Instance.BatteryLife;

        set
        {
            PlayerData.Instance.BatteryLife = value;
        }
    }

    [field: Header("Materials")]
    [field: SerializeField] public MeshRenderer ToyRender { get; set; }
    [field: SerializeField] public Material DefaultMaterial { get; set; }
    [field: SerializeField] public Material GlowMaterial { get; set; }

    [field: Header("Miscellaneous")]
    public Light GlowToyLight;
    public GameObject glowToyVolume;

    public SpriteRenderer BatteryUIRenderer;
    [field: SerializeField] public Animator Animator { get; set; }

    LightVolume lightVolume;

    private const int BatteryUICap = 4;
    public BatteryUI[] BatteryUISprites = new BatteryUI[BatteryUICap];

    private float CurrentFadeIn;
    private float CurrentDebounce;

    private bool HoldingInputDown = false;
    private bool WaitingForRelease = false;
    private bool CanTurnOn = false;

    #endregion

    #region Glow-Toy Methods

    private void HandleInput(bool value) => HoldingInputDown = value;
    private void Toggle() => isOn = !isOn;
    
    private void ToggleLight(bool value)
    {
        GlowToyLight.enabled = value;

        // Update lightVolume
        if (value)
        {
            lightVolume.enabled = value;
        }
        lightVolume.Renderer.enabled = value;
        lightVolume.Mesh.enabled = value;
        lightVolume.enabled = value;
    }

    private void UpdateBatteryUI()
    {
        BatteryUIRenderer.enabled = isOn;

        if (!isOn)
            return;

        BatteryUIRenderer.transform.LookAt(GameManager.Instance.Camera.transform);

        int batteryIndex = (int)Mathf.Lerp(BatteryUICap - 1, 0, CurrentBattery / MaxBattery);
        BatteryUIRenderer.sprite = BatteryUISprites[batteryIndex].sprite;
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

    private void Awake()
    {
        lightVolume = glowToyVolume.GetComponent<LightVolume>();
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

        if (BatteryUISprites.Length != BatteryUICap)
        {
            Debug.LogWarning("You can only have " + BatteryUICap.ToString() + " amounts of sprites at a given time using the BatteryUISprites array.", this);
            return;
        }

        if (BatteryUIRenderer.sprite == null)
            BatteryUIRenderer.sprite = BatteryUISprites[0].sprite;

        if (BatteryUIRenderer.enabled)
            BatteryUIRenderer.enabled = false;

        CurrentBattery = MaxBattery;
    }

    private void Update()
    {
        UpdateBatteryUI();

        CanTurnOn = HoldingInputDown && CurrentBattery > 0 && !GameManager.Instance.Player.isInLight;
        Animator.SetBool("IsShaking", CanTurnOn && !isOn && CurrentDebounce == 0.0f);
        ToyRender.material = isOn ? GlowMaterial : DefaultMaterial;

        if (isOn)
        {
            if (!HoldingInputDown && WaitingForRelease)
                WaitingForRelease = false;

            if ((HoldingInputDown && !WaitingForRelease))
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

        if (CanTurnOn)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            if (CurrentFadeIn < MaxFadeIn)
            {
                if (!AudioManager.Instance.IsPlaying("ShakeLoop"))
                    AudioManager.Instance.Play("ShakeLoop");

                CurrentFadeIn += FadeModifier * Time.deltaTime;
                
                return;
            }

            CurrentFadeIn = 0.0f;
            WaitingForRelease = true;

            Toggle();
            ToggleLight(true);

            AudioManager.Instance.Stop("ShakeLoop");
            AudioManager.Instance.Play("ShakeDone");

            return;
        }

        ToggleLight(false);

        if (CurrentFadeIn > 0.0f)
        {
            if (AudioManager.Instance.IsPlaying("ShakeLoop"))
                AudioManager.Instance.Stop("ShakeLoop");

            CurrentFadeIn -= FadeModifier * Time.deltaTime;
            return;
        }

        CurrentFadeIn = 0.0f;
    }

    #endregion
}
