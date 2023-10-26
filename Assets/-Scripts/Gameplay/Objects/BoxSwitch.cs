using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSwitch : GenericSwitch
{
    public bool isOn { get; set; }
    public override void  Interact()
    {
        isOn = !isOn;
        base.Interact();
    }
}
