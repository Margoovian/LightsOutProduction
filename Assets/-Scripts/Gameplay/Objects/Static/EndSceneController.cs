using System.Threading.Tasks;
using UnityEngine;

public class EndSceneController : MonoBehaviour
{
    void Awake()
    {
        HelperFunctions.WaitForTask(WaitForManagers(), () =>
        {});
    }

    public async Task WaitForManagers()
    {
        while (GameManager.Instance == null)
            await Task.Yield();
    }

    void Update() => transform.position -= new Vector3(GameManager.Instance.GameSettings.FearWallSpeed, 0, 0) * Time.deltaTime;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerController>(out var _))
            return;

        PlayerData.Instance.InFearWall = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<PlayerController>(out var _))
            return;

        PlayerData.Instance.InFearWall = false;
    }
}
