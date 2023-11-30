using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchBox : MonoBehaviour, IController
{
    [Serializable] public class Combo
    {
        [field: SerializeField] public GenericSwitch Switch { get; set; }
        [field: SerializeField] public bool RequiredState { get; set; }
    }
    public UnityEvent<bool> Event { get; set; } = new();
    public ILight[] Lights { get => _lights; set => _lights = (GenericLight[])value; }
    [SerializeField] private GenericLight[] _lights;
    [field: SerializeField] public Combo[] SwitchCombos { set; get; }
    private void Start()
    {
        foreach (ILight light in Lights)
        {
            light?.Controllers.Add(this);
        }
        foreach(Combo combo in SwitchCombos)
        {
            combo.Switch.Event?.AddListener(EvaluateWrapper);
        }
    }
    private void OnDestroy()
    {
        foreach (Combo combo in SwitchCombos)
        {
            combo.Switch.Event?.RemoveListener(EvaluateWrapper);
        }
    }
    private void EvaluateWrapper(bool var) => Evaluate();
    public virtual void Evaluate()
    {
        bool eval = true;
        foreach (Combo combo in SwitchCombos) {
            eval = combo.Switch.isOn == combo.RequiredState;
            if (!eval) break;
        }
        FlipLights(!eval);
    }
    private void FlipLights(bool state)
    {
        if (state)
            foreach (GenericLight light in _lights)
            {
                if (light.isOn == state) continue;
                light.isOn = true;
                light.ChangeMaterial();
                light.EditLightCount();
            }
        else
            foreach (GenericLight light in _lights)
            {
                if (light.isOn == state) continue;
                light.isOn = false;
                light.ChangeMaterial();
                light.EditLightCount();
            }
    }
}
