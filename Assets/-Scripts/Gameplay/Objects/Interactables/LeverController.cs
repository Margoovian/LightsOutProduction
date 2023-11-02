using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LeverController : MonoBehaviour, IController
{

    [field: SerializeField] public LeverSwitch[] Switches {get;set;}
    [field: SerializeField] public int TimeInMS { get; set; }
    public UnityEvent<bool> Event { get; set; }
    public ILight[] Lights { get => _lights; set => _lights = (GenericLight[])value; }
    [SerializeField] private GenericLight[] _lights;

    private Task _timer = null;
    private bool _complete = false;

    private void Start()
    {
        foreach (ILight light in Lights)
        {
            light?.Controllers.Add(this);
        }
        foreach (LeverSwitch lever in Switches)
        {
            lever.Event?.AddListener(EvaluateWrapper);
        }
    }
    private void OnDestroy()
    {
        foreach (LeverSwitch lever in Switches)
        {
            lever.Event?.RemoveListener(EvaluateWrapper);
        }
    }

    private void StartTimer()
    {
        bool timerIsRunning;
        if( _timer != null) 
            timerIsRunning = !_timer.IsCompleted;
        else
        {
            _timer = HelperFunctions.Timer(TimeInMS);
            return;
        }
        if (!timerIsRunning && !_complete) ResetSwitches();
        return;

    }

    private void Update()
    {
        if (_timer == null) return;
        if(_timer.IsCompleted && !_complete) ResetSwitches();
    }


    private void ResetSwitches()
    {
        foreach(LeverSwitch lever in Switches)
        {
            lever?.ResetSwitch();
        }
        _timer = null;
    }

    private void EvaluateWrapper(bool var) => Evaluate();
    public void Evaluate()
    {
        if (_complete) return;
        StartTimer();
        bool eval = true;
        foreach (LeverSwitch lever in Switches)
        {
            eval = lever.isOn;
            if (eval)
                return;
        }
        _complete = !eval;
        FlipLights(!eval);
    }

    private void FlipLights(bool state)
    {
        foreach(GenericLight light in _lights)
        {
            light.Toggle();
        }
    }
}
