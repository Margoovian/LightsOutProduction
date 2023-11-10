using UnityEngine;

public interface IVolume
{
    public MeshRenderer Renderer { get; set; }
    public MeshCollider Mesh { get; set; }
    public void OnTriggerEnter(Collider other);
    public void OnTriggerExit(Collider other);
}
