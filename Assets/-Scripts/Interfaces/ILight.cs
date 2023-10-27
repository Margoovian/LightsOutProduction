using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILight
{
    /// <summary>
    /// Is it on?
    /// </summary>
    public bool isOn { get; set; }
    public Material OnMaterial { get; set; }
    public Material OffMaterial { get; set; }
    public bool DefaultState { get; set; }

    /// <summary>
    /// The Controller that controls the light
    /// </summary>
    public List<IController> Controllers { get; set; }

    /// <summary>
    /// Toggle isOn
    /// </summary>
    public void Toggle();
    public void ChangeMaterial();
}
