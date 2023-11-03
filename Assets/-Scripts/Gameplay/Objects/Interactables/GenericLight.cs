using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericLight : MonoBehaviour, ILight
{
    public bool isOn { get; set; }
    public List<IController> Controllers { get; set; } = new();
    [field: SerializeField] public Material OnMaterial { get; set; }
    [field: SerializeField] public Material OffMaterial { get; set; }
    [field: SerializeField] public bool DefaultState { get; set; } = true;
    protected MeshRenderer _meshRenderer;
    [field: SerializeField] private LightVolume LightVolume { get; set; }

    private void Start() { 
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        isOn = DefaultState;
        ChangeMaterial ();
    }
    
    public void ChangeMaterial()
    {
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
        
        if (isOn)
        {
            _meshRenderer.material = OnMaterial;
            if (LightVolume) { 
                LightVolume.enabled = true;
                if(LightVolume.Renderer) LightVolume.Renderer.enabled = true;
                if (LightVolume.Mesh) LightVolume.Mesh.enabled = true;
            }
            return;

        }

        _meshRenderer.material = OffMaterial;
        if (LightVolume)
        {
            _meshRenderer.material = OffMaterial;
            if (LightVolume) { 
                LightVolume.enabled = false;
                if (LightVolume.Renderer) LightVolume.Renderer.enabled = false;
                if (LightVolume.Mesh) LightVolume.Mesh.enabled = false; }
        }
    }

    public virtual void Toggle() { isOn = !isOn; ChangeMaterial();  }
}
