using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverSwitch : BoxSwitch
{
    public void ResetSwitch()
    {
        isOn = true;
        foreach(GenericLight light in _lights)
        {
            light.isOn = isOn;
            light.ChangeMaterial();
        }
    }
}
