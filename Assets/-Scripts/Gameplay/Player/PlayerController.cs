using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public bool isInLight { get; set; }
    public float CharacterSpeed { 
        get 
        {
            float s = _speed * GameManager.Instance.GameSettings.FearSpeedMultiplyer.Evaluate(Fear / GameManager.Instance.GameSettings.MaxFear);
            
            if (GameManager.Instance.GameSettings.EnableSpeedModifier) 
                return s * GameManager.Instance.GameSettings.SpeedModifier; 
                
            return s; 
        }
        
        set => _speed = value; 
    }

    private float _speed;
    public float Fear 
    { 
        get => GameManager.Instance.PlayerData.FearLevel; 
        
        set {
            if (!GameManager.Instance.GameSettings.EnableGodMode)
            {
                GameManager.Instance.PlayerData.FearLevel = value;
                return;
            }

            GameManager.Instance.PlayerData.FearLevel = 0;
        }}

    private float _fearTime;
    private Vector2 _moveDirection;
    private CharacterController _characterController;

    [field: SerializeField] public float Gravity { get; set; }

    private void OnEnable() {
        HelperFunctions.WaitForTask(WaitForManagers(), () =>
        {
            GameManager.Instance.Player = this;
            InputManager.Instance.Player_Move.AddListener(Move);

            GameManager.Instance.PlayerData.InMenu = false;

        });
    }

    public async Task WaitForManagers()
    {
        while (InputManager.Instance == null || GameManager.Instance == null)
            await Task.Yield();
    }

    private void OnDisable() { 
        if (GameManager.Instance)
            GameManager.Instance.Player = null;

        if (InputManager.Instance)
            InputManager.Instance.Player_Move.RemoveListener(Move);
    }
    private void Start()
    {
        name = "Player";
        _characterController = GetComponent<CharacterController>();
        CharacterSpeed = GameManager.Instance.GameSettings.PlayerBaseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        _characterController.Move(new Vector3(_moveDirection.x * CharacterSpeed, -Gravity, _moveDirection.y * CharacterSpeed) * Time.deltaTime);

        if (_fearTime >= GameManager.Instance.GameSettings.FearTickRate)
        {
            if (isInLight) 
                Fear = Mathf.Clamp(Fear - GameManager.Instance.GameSettings.FearTickAmount, 0, GameManager.Instance.GameSettings.MaxFear);
            else 
                Fear = Mathf.Clamp(Fear + GameManager.Instance.GameSettings.FearTickAmount, 0, GameManager.Instance.GameSettings.MaxFear);
            
            _fearTime = 0;
        }
        
        else 
            _fearTime += Time.deltaTime;
    }

    private void Move(Vector2 direction) => _moveDirection = direction;
}