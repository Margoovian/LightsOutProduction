using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EndSceneController : MonoBehaviour
{
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(GameManager.Instance.GameSettings.FearWallSpeed, 0, 0) * Time.deltaTime;
    }

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
