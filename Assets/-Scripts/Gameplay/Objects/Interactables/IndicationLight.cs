using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicationLight : GenericLight
{
    private void Start() { isIndication = true; base.Start(); } 
    
    internal override void EditLightCount(){}
}
