using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public float InteractionRange { get; set; }
    public void Interact();
}
