using System.Collections.Generic;
using UnityEngine;

public class GenericLight : MonoBehaviour, ILight
{
    public bool isOn { get; set; }
    public List<IController> Controllers { get; set; } = new();
    [field: SerializeField] public Material OnMaterial { get; set; }
    [field: SerializeField] public Material OffMaterial { get; set; }
    [field: SerializeField] public bool DefaultState { get; set; } = true;
    [field: SerializeField] private LightVolume[] LightVolumes { get; set; }
    [field: SerializeField] private Light[] Lights { get; set; }
    
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

        if (isOn)
            _meshRenderer.material = OnMaterial;
        else
            _meshRenderer.material = OffMaterial;

        LevelController.Instance.UpdateLightCount();

        if (LightVolumes.Length == 0)
            return;

        foreach (LightVolume i in LightVolumes)
        {
            i.enabled = isOn;
            
            if (i.Renderer) 
                i.Renderer.enabled = isOn;

            if (i.Mesh) 
                i.Mesh.enabled = isOn;
        }

        foreach (Light i in Lights)
            i.enabled = isOn;
    }

    public virtual void Toggle() { isOn = !isOn; ChangeMaterial();  }
}
