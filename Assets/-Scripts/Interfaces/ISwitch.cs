using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface ISwitch : IController
{
    public virtual void Interact() => Evaluate();
}
