using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [field: SerializeField] public float CharacterSpeed { get; set; }
    [field: SerializeField] public float Gravity { get; set; }
    private Vector2 _moveDirection;

    private CharacterController _characterController;
    private void OnEnable() {
        HelperFunctions.WaitForTask(WaitForManagers(), () =>
        {
            GameManager.Instance.Player = this;
            InputManager.Instance.Player_Move.AddListener(Move);
        });
    }

    public async Task WaitForManagers()
    {
        while (InputManager.Instance == null || GameManager.Instance == null)
            await Task.Yield();
    }

    private void OnDisable() { 
        if(GameManager.Instance)
            GameManager.Instance.Player = null;
        if (InputManager.Instance)
            InputManager.Instance.Player_Move.RemoveListener(Move);
    }
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _characterController.Move(new Vector3(_moveDirection.x * CharacterSpeed, -Gravity, _moveDirection.y * CharacterSpeed) * Time.deltaTime);
    }

    private void Move(Vector2 direction) => _moveDirection = direction;
}
