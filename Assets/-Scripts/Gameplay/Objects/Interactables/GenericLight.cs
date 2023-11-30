using System.Collections.Generic;
using UnityEngine;

public class GenericLight : MonoBehaviour, ILight
{
    public bool isOn { get; set; }
    public List<IController> Controllers { get; set; } = new();
    [field: SerializeField] public bool IgnoreMaterials { get; set; } = true;
    [field: SerializeField] public Material OnMaterial { get; set; }
    [field: SerializeField] public Material OffMaterial { get; set; }
    [field: SerializeField] public bool DefaultState { get; set; } = true;
    [field: SerializeField] private LightVolume[] LightVolumes { get; set; }
    [field: SerializeField] private Light[] Lights { get; set; }
    protected bool isIndication = false;
    protected MeshRenderer _meshRenderer;

    protected void Start()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        

        isOn = DefaultState;
        ChangeMaterial();
        EditLightCount();
    }
    
    internal virtual void EditLightCount()
    {
        if (isIndication) return;
        if (isOn)
            LevelController.Instance.ModifyLightCount(1);
        else
            LevelController.Instance.ModifyLightCount(-1);
    }

    public void ChangeMaterial()
    {
        if (!IgnoreMaterials)
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
        }

        Material material = isOn ? OnMaterial : OffMaterial;

        if (material != null)
            _meshRenderer.material = material;

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

    public virtual void Toggle() { isOn = !isOn; ChangeMaterial(); EditLightCount(); }
}
