using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LightVolume : MonoBehaviour, IVolume
{
    public MeshRenderer Renderer { get; set; }
    public MeshCollider Mesh { get; set; }

    private void Start()
    {
        Renderer = GetComponent<MeshRenderer>();
        Mesh = GetComponent<MeshCollider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        PlayerController player;

        bool success = GameManager.Instance.TryGetPlayer(out player);
        if (!success) 
            return;

        if (other.gameObject == player.gameObject)
            player.isInLight = true;
    }

    public void OnTriggerExit(Collider other)
    {
        PlayerController player;

        bool success = GameManager.Instance.TryGetPlayer(out player);
        if (!success) 
            return;
        
        if (other.gameObject == player.gameObject)
            player.isInLight = false;
    }

    private void OnDisable()
    {
        PlayerController player;
        
        bool success = GameManager.Instance.TryGetPlayer(out player);
        if (!success) 
            return;

        player.isInLight = false;
    }
}
