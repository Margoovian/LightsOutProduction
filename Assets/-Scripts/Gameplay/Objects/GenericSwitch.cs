using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericSwitch : MonoBehaviour, ISwitch
{
    public UnityEvent<bool> Event { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public ILight[] Lights { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Evaluate()
    {
        
    }

    public void Interact()
    {
        foreach (ILight light in Lights)
        {
            light.Toggle();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
