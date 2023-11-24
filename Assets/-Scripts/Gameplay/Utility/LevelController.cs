using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; internal set; }

    [field: SerializeField] public int MaxLights { get; set; }
    [field: SerializeField] public SceneTrigger SceneTrigger { get; set; }

    //Recompile!
    private int _lightCount = 0;

    public int GetMaxLights() => MaxLights;
    public void ResetValues() => _lightCount = 0;
    public void IncreaseLightCount() => _lightCount++;
    public (int, int) GetValues() => (_lightCount, GetMaxLights());
    public void ModifyLightCount(int amount) { _lightCount += amount; UpdateLightCount(); }

    public void UpdateLightCount()
    {
        bool result = _lightCount <= 0;
        SceneTrigger.enabled = result;

        if (result)
            AudioManager.Instance.Play("DoorOpened");

        if (!result && SceneTrigger.enabled)
        {
            SceneTrigger.enabled = false;
            AudioManager.Instance.Play("DoorOpened");
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

        if (_lightCount == 0)
        {
            Debug.LogWarning("No GenericLight Instances were added to array: 'TargetLights'", this);

            if (!SceneTrigger.enabled)
                SceneTrigger.enabled = true;

            return;
        }

        _lightCount = GetMaxLights();

        if (SceneTrigger.enabled)
            SceneTrigger.enabled = false;
    }
}
