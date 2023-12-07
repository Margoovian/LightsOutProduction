using UnityEngine;

[CreateAssetMenu(fileName = "NewGameSettings", menuName = "Lights Out/GameSettings")]
public class GameSettings : ScriptableObject
{
    [field: Header("Game Settings")]
    [field: SerializeField] public Difficulty Difficulty { get; set; }
    [field: SerializeField] public string DifficultyName { get; set; }
    [field: SerializeField] public float PlayerBaseSpeed { get; set; }
    [field: SerializeField] public AnimationCurve FearSpeedMultiplyer { get; set; }
    [field: SerializeField] public float MaxFear { get; set; }
    [field: SerializeField] public float FearTickRate { get; set; }
    [field: SerializeField] public float FearTickAmount { get; set; }
    [field: SerializeField] public float FearDropAmount { get; set; }
    [field: SerializeField] public float FearWallSpeed { get; set; }
    [field: SerializeField] public float FearWallTick { get; set; }
    [field: SerializeField] public float GlowToyFadeIn { get; set; }
    [field: SerializeField] public float GlowToyFadeModifier { get; set; }
    [field: SerializeField] public float GlowToyDebounceModifier { get; set; }
    [field: SerializeField] public float GlowToyMaxBattery { get; set; }
    [field: SerializeField] public float GlowToyBatteryTickRate { get; set; }
    [field: SerializeField] public float GlowToyBatteryTickAmount { get; set; }

    [field: Header("Miscellaneous")]
    [field: SerializeField] public bool EnableTimer { get; set; }
    [field: SerializeField] public bool EnableRandomRooms { get; set; }

    [field: Header("Debug / Cheats")]
    [field: SerializeField] public bool EnableGodMode { get; set; }
    [field: SerializeField] public bool EnableSpeedModifier { get; set; }
    [field: SerializeField] public float SpeedModifier { get; set; } = 1f;

    private void OnEnable()
    {
        SetCheatsFalse();
    }

    private void OnDisable()
    {

    }

    private void SetCheatsFalse()
    {
#if DEVELOPMENT_BUILD
        EnableGodMode = false;
        EnableSpeedModifier = false;
#endif
    }

    private void SetCheatsTrue()
    {
        #if DEVELOPMENT_BUILD
            EnableGodMode = true;
            EnableSpeedModifier = true;
#endif
    }
}
