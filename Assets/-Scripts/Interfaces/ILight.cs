using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILight
{
    /// <summary>
    /// Is it on?
    /// </summary>
    [SerializeField] public bool isOn { get; set; }

    /// <summary>
    /// The Controller that controls the light
    /// </summary>
    [SerializeField] public IController Controller { get; set; }

    /// <summary>
    /// Toggle isOn
    /// </summary>
    [SerializeField] public void Toggle() => isOn = !isOn;
}
