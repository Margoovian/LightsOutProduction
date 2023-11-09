using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    public int sceneIndex;
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

        SceneController Inst = SceneController.Instance;
        Inst.LoadSpecificAndTransfer(sceneIndex);
    }
}
