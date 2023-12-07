using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public bool isInLight { get; set; }
    [field: SerializeField] public GameObject Model { get; set; }    
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
            if (PlayerData.Instance.InMenu) return;
            if (!GameManager.Instance.GameSettings.EnableGodMode )
            {
                GameManager.Instance.PlayerData.FearLevel = value;
                return;
            }

            GameManager.Instance.PlayerData.FearLevel = 0;
        }}

    private float _fearTime;
    private Vector2 _moveDirection;
    private CharacterController _characterController;
    private Animator _animator;

    [field: SerializeField] public float Gravity { get; set; }

    public void Footstep(int index) => AudioManager.Instance.PlaySFX("FootStep" + index.ToString());

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
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool("IsWalking", _moveDirection.x != 0 || _moveDirection.y != 0);
        _characterController.Move(new Vector3(_moveDirection.x * CharacterSpeed, -Gravity, _moveDirection.y * CharacterSpeed) * Time.deltaTime);

        if (_fearTime >= GameManager.Instance.GameSettings.FearTickRate)
        {

            if (isInLight || PlayerData.Instance.ToyOn)
                Fear = Mathf.Clamp(Fear - GameManager.Instance.GameSettings.FearDropAmount, 0, GameManager.Instance.GameSettings.MaxFear);
            else
            {
                if(PlayerData.Instance.InFearWall)
                    Fear = Mathf.Clamp(Fear + GameManager.Instance.GameSettings.FearWallTick, 0, GameManager.Instance.GameSettings.MaxFear);
                else
                    Fear = Mathf.Clamp(Fear + GameManager.Instance.GameSettings.FearTickAmount, 0, GameManager.Instance.GameSettings.MaxFear);
            }
            _fearTime = 0;
            
        }
        else
            _fearTime += Time.deltaTime;

        if (_moveDirection.magnitude > 0)
        {
            float radian = Mathf.Atan2(_moveDirection.y, _moveDirection.x * -1);
            float degree = 180 * radian / Mathf.PI;
            float rotation = (360 + Mathf.Round(degree)) % 360;

            Model.transform.rotation = Quaternion.Euler(-20,rotation-90, 0);
        }
    }

    private void Move(Vector2 direction) => _moveDirection = direction;
}