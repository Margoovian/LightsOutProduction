using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; internal set; }

    [field: SerializeField] public SceneTrigger SceneTrigger { get; set; }
    [field: SerializeField] protected GenericLight[] TargetLights { get; set; }

    [HideInInspector] public int CurrentLights;

    public int GetMaxLights() => TargetLights.Length;
    public void ResetValues() => CurrentLights = 0;

    public void UpdateLightCount()
    {
        CurrentLights = 0;
        foreach (GenericLight i in TargetLights)
        {
            if (i.isOn)
                CurrentLights++;
        }

        TrackLightCount.Instance.Modify(CurrentLights, GetMaxLights());

        bool result = CurrentLights == 0;
        SceneTrigger.enabled = result;

        if (result)
            result = true; // This is here temporarily until the DoorOpening sound is made!
            //AudioManager.Instance.Play("DoorOpening");

        if (!result && SceneTrigger.enabled)
        {
            SceneTrigger.enabled = false;
            //AudioManager.Instance.Play("DoorClosing");
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

        if (TargetLights.Length == 0)
        {
            Debug.LogWarning("No GenericLight Instances were added to array: 'TargetLights'", this);
            return;
        }

        CurrentLights = GetMaxLights();

        if (SceneTrigger.enabled)
            SceneTrigger.enabled = false;
    }
}
