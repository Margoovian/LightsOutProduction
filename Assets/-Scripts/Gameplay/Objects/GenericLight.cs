using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericLight : MonoBehaviour, ILight
{
    public bool isOn { get; set; } = true;
    public List<IController> Controllers { get; set; } = new();
    [field: SerializeField]public Material OnMaterial { get; set; }
    [field: SerializeField]public Material OffMaterial { get; set; }
    private MeshRenderer _meshRenderer;

    private void Start() => _meshRenderer = GetComponent<MeshRenderer>();
    
    public void ChangeMaterial()
    {
        if (OnMaterial == null) { Debug.LogWarning("No OnMaterial", this); return; }
        if (OffMaterial == null) { Debug.LogWarning("No OffMaterial", this); return; }
        if (isOn)
            _meshRenderer.material = OnMaterial;
        else
            _meshRenderer.material = OffMaterial;
    }
}
