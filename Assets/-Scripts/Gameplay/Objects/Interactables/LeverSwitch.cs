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
            if (light.isOn == isOn) continue;
            light.isOn = isOn;
            light.ChangeMaterial();
            light.EditLightCount();
        }
    }
}
