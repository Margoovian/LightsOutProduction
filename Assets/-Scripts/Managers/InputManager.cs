using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; set; }
    private GameInput Inputs;

    #region Events

    [HideInInspector] public UnityEvent<bool> Player_Interact;
    [HideInInspector] public UnityEvent<bool> Player_Fire;
    [HideInInspector] public UnityEvent<bool> Player_Glowtoy;
    [HideInInspector] public UnityEvent<Vector2> Player_Move;
    [HideInInspector] public UnityEvent<Vector2> Player_Look;
    
    #endregion
    private void Awake() {
        if (!Instance)
            Instance = this;
        Inputs = new();
        Initialize();     
    }

    private void OnEnable()
    {
        Inputs.Player.Move.performed += PlayerMove;
        Inputs.Player.Fire.performed += PlayerFire;
        Inputs.Player.GlowToy.performed += PlayerGlowtoy;
        Inputs.Player.Interact.performed += PlayerInteract;
        Inputs.Player.Look.performed += PlayerLook;

        Inputs.Player.Enable();
    }
    private void OnDisable()
    {
        Inputs.Player.Move.performed -= PlayerMove;
        Inputs.Player.Fire.performed -= PlayerFire;
        Inputs.Player.GlowToy.performed -= PlayerGlowtoy;
        Inputs.Player.Interact.performed -= PlayerInteract;
        Inputs.Player.Look.performed -= PlayerLook;

        Inputs.Player.Disable();
    }
    private void Initialize()
    {

        Player_Interact ??= new();
        Player_Fire ??= new();
        Player_Glowtoy ??= new();
        Player_Move ??= new();
        Player_Look ??= new();
    }

    public void PlayerInteract(InputAction.CallbackContext ctx) => Player_Interact?.Invoke(ctx.ReadValueAsButton());
    public void PlayerFire(InputAction.CallbackContext ctx) => Player_Fire?.Invoke(ctx.ReadValueAsButton());
    public void PlayerGlowtoy(InputAction.CallbackContext ctx) => Player_Glowtoy?.Invoke(ctx.ReadValueAsButton());
    public void PlayerMove(InputAction.CallbackContext ctx) => Player_Move?.Invoke(ctx.ReadValue<Vector2>());
    public void PlayerLook(InputAction.CallbackContext ctx) => Player_Look?.Invoke(ctx.ReadValue<Vector2>());
}
