using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; internal set; }

    //Recompile!
    private int _lightCount = 0;
    [field: SerializeField] public SceneTrigger SceneTrigger { get; set; }
    [field: SerializeField] protected GenericLight[] TargetLights { get; set; }

    [HideInInspector] public int CurrentLights;

    public int GetMaxLights() => _lightCount;
    public void ResetValues() => _lightCount = 0;
    public int GetValues() => _lightCount;

    public void UpdateLightCount()
    {
        //CurrentLights = 0;
        //foreach (GenericLight i in TargetLights)
        //{
        //    if (i.isOn)
        //        CurrentLights++;s
        //}
        
        //TODO: COREY FIX THIS, PS try and stop using singletons
        //TrackLightCount.Instance.Modify(CurrentLights, GetMaxLights());

        bool result = _lightCount <= 0;
        Debug.Log(_lightCount <= 0);
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

    public void IncreaseLightCount()
    {
        _lightCount++;
    }
    public void ModifyLightCount(int amount) { _lightCount += amount; UpdateLightCount(); }

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

        CurrentLights = GetMaxLights();

        if (SceneTrigger.enabled)
            SceneTrigger.enabled = false;
    }
}
