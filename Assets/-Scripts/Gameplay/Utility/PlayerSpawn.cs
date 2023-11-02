using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [field: SerializeField] public GameObject PlayerPrefab { get; set; }
    [field: SerializeField] public Mesh DebugMesh { get; set; }

    void Start()
    {
        GameObject obj = Instantiate(PlayerPrefab);
        PlayerController player = obj.GetComponent<PlayerController>();
        if (!player)
        {
            Destroy(obj);
            return;
        }

        player.transform.position = new Vector3(0f,0.5f,0f) + transform.position;
        player.transform.rotation = transform.rotation;
        GameManager.Instance.Player = player;

    }

    private void OnDrawGizmosSelected()
    {
        if (DebugMesh == null) return;
        Gizmos.matrix = Matrix4x4.TRS(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, transform.lossyScale);
        Gizmos.color = Color.green;
        Gizmos.DrawWireMesh(DebugMesh);

    }
}
