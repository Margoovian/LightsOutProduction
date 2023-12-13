using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; internal set; }

    [field: SerializeField] public int MaxLights { get; set; }
    [field: SerializeField] public SceneTrigger SceneTrigger { get; set; }

    //Recompile!
    private int _lightCount = 0;

    public void ResetValues() => _lightCount = MaxLights;
    public int GetCurrentValue() => (_lightCount);
    public void ModifyLightCount(int amount) { _lightCount += amount; UpdateLightCount(); }

    public void UpdateLightCount()
    {
        bool result = _lightCount <= 0;
        SceneTrigger.enabled = result;

        if (result)
            AudioManager.Instance.PlaySFX("DoorOpened");

        if (!result && SceneTrigger.enabled)
        {
            SceneTrigger.enabled = false;
            AudioManager.Instance.PlaySFX("DoorOpened");
        }

        //Debug.LogWarning("Door Opened Status: " + SceneTrigger.enabled.ToString());
    }

    private void Awake()
    {
        if (!Instance)
            Instance = this;

        if (SceneTrigger == null)
        {
            Debug.LogWarning("A SceneTrigger Component wasn't found!", this);
            return;
        }

        //_lightCount = GetMaxLights();

        if (MaxLights == 0)
        {
            Debug.LogWarning("'Max Lights' weren't adjusted or set to zero, skipping...", this);

            if (!SceneTrigger.enabled)
                SceneTrigger.enabled = true;

            return;
        }

        if (SceneTrigger.enabled)
            SceneTrigger.enabled = false;
    }
}
