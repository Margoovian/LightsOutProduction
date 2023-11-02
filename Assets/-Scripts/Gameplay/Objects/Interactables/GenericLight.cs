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
<<<<<<< HEAD:Assets/-Scripts/Gameplay/Objects/GenericLight.cs
            if (LightVolume) { 
                LightVolume.enabled = true;
                if(LightVolume.Renderer) LightVolume.Renderer.enabled = true;
                if (LightVolume.Mesh) LightVolume.Mesh.enabled = true;
            }
=======
            if (LightVolume) 
            { 
                LightVolume.enabled = true; 
                LightVolume.Renderer.enabled = true; 
                LightVolume.Mesh.enabled = true; 
            }

            return;
>>>>>>> ac537964c33da6399d6ea73eac99a80583023dbf:Assets/-Scripts/Gameplay/Objects/Interactables/GenericLight.cs
        }

        _meshRenderer.material = OffMaterial;
        if (LightVolume)
        {
<<<<<<< HEAD:Assets/-Scripts/Gameplay/Objects/GenericLight.cs
            _meshRenderer.material = OffMaterial;
            if (LightVolume) { 
                LightVolume.enabled = false;
                if (LightVolume.Renderer) LightVolume.Renderer.enabled = false;
                if (LightVolume.Mesh) LightVolume.Mesh.enabled = false; }
=======
            LightVolume.enabled = false;
            LightVolume.Renderer.enabled = false;
            LightVolume.Mesh.enabled = false;
>>>>>>> ac537964c33da6399d6ea73eac99a80583023dbf:Assets/-Scripts/Gameplay/Objects/Interactables/GenericLight.cs
        }
    }

    public virtual void Toggle() { isOn = !isOn; ChangeMaterial();  }
}
