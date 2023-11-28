using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [field: SerializeField] public int SceneIndex { get; set; }
    [field: SerializeField] public bool IgnoreIndex { get; set; }
    [field: SerializeField] public bool ReverseIndex { get; set; }
    
    private BoxCollider _trigger;

    private void OnEnable()
    {
        _trigger = GetComponent<BoxCollider>();
        _trigger.isTrigger = true;
        _trigger.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerController>(out var _))
            return;

        SceneController.Instance._isAltLevel = ReverseIndex;

        if (IgnoreIndex)
        {
            SceneController.Instance.NextLevel( 
                beforeLoad: () => {
                    if (GameManager.Instance.SceneTransition == null) 
                        return null;
                    
                    GameManager.Instance.SceneTransition.Play("Base Layer.Transition", 0);
                    PlayerData.Instance.InMenu = true;
                    
                    return HelperFunctions.Timer(1000);
                },

                afterLoad: () => HelperFunctions.Timer(1000),
                afterAction: () => { PlayerData.Instance.InMenu = false; }
            );

            return;
        }

        InputManager.Instance.DisableControls();
        SceneController.Instance.LoadSpecific(SceneIndex);
    }
}