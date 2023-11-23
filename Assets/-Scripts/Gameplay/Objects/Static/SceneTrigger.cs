using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    public int sceneIndex;
    public bool ignoreIndex;

    private BoxCollider trigger;

    private void Start()
    {
        trigger = GetComponent<BoxCollider>();
        trigger.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerController>(out var _))
            return;

        if (ignoreIndex)
        {
            SceneController.Instance.NextLevel( 
                beforeLoad: () => {
                    if (GameManager.Instance.SceneTransition == null) return null;
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
        SceneController.Instance.LoadSpecific(sceneIndex);
    }
}