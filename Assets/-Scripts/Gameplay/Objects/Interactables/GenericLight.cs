using System.Collections.Generic;
using UnityEngine;

public class GenericLight : MonoBehaviour, ILight
{
    public bool isOn { get; set; }
    public List<IController> Controllers { get; set; } = new();
    [field: SerializeField] public Material OnMaterial { get; set; }
    [field: SerializeField] public Material OffMaterial { get; set; }
    [field: SerializeField] public bool DefaultState { get; set; } = true;
    [field: SerializeField] private LightVolume LightVolume { get; set; }
    
    protected MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        isOn = DefaultState;
        ChangeMaterial();
    }
    
    public void ChangeMaterial()
    {
        //Debug.LogWarning(this);

        if (OnMaterial == null) 
        { 
            Debug.LogWarning("No OnMaterial", this); 
            return; 
        }

        if (OffMaterial == null) 
        { 
            Debug.LogWarning("No OffMaterial", this); 
            return; 
        }

        if (LightVolume == null)
            return;

        if (isOn)
            _meshRenderer.material = OnMaterial;
        else
            _meshRenderer.material = OffMaterial;

        if (LightVolume.Renderer == null)
        {
            Debug.LogWarning("No LightVolume 'MeshRenderer'", LightVolume);
            return;
        }

        if (LightVolume.Mesh == null)
        {
            Debug.LogWarning("No LightVolume 'MeshCollider'", LightVolume);
            return;
        }

        if (LightVolume)
        {
            LightVolume.enabled = isOn;
            LightVolume.Renderer.enabled = isOn;
            LightVolume.Mesh.enabled = isOn;
        }
    }

    public virtual void Toggle() { isOn = !isOn; ChangeMaterial();  }
}
