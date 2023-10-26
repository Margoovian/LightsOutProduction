using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; set; }

    #region Events

    [HideInInspector] public UnityEvent<bool> Player_Interact;
    [HideInInspector] public UnityEvent<bool> Player_Fire;
    [HideInInspector] public UnityEvent<Vector2> Player_Move;
    [HideInInspector] public UnityEvent<Vector2> Player_Look;
    
    #endregion
    private void Awake() {
        Instance = this;
        Initialize();     
    }

    private void Initialize()
    {
        if (Player_Interact == null) Player_Interact = new();
        if (Player_Fire == null) Player_Fire = new();
        if (Player_Move == null) Player_Move = new();
        if (Player_Look == null) Player_Look = new();
        
    }

    public void PlayerInteract(InputAction.CallbackContext ctx) => Player_Interact?.Invoke(ctx.ReadValueAsButton());
    public void PlayerFire(InputAction.CallbackContext ctx) => Player_Fire?.Invoke(ctx.ReadValueAsButton());
    public void PlayerMove(InputAction.CallbackContext ctx) => Player_Move?.Invoke(ctx.ReadValue<Vector2>());
    public void PlayerLook(InputAction.CallbackContext ctx) => Player_Look?.Invoke(ctx.ReadValue<Vector2>());


}
