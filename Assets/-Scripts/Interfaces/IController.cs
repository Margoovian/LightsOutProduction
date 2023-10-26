using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IController
{
    /// <summary>
    /// Subscribable Event that triggers after Evaluate
    /// </summary>
    [SerializeField] public UnityEvent<bool> Event { get; set; }
    /// <summary>
    /// List of Lights
    /// </summary>
    [SerializeField] public ILight[] Lights { get; set; }

    /// <summary>
    /// Used for evaluating a logic
    /// </summary>
    public abstract void Evaluate();
}
