using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; internal set; }
    [field: SerializeField] public Cinemachine.CinemachineVirtualCamera Camera { get; set; }
    [field: SerializeField] public GameSettings GameSettings { get; set; }
    [field: SerializeField] public PlayerData PlayerData { get; set; }
    [field: SerializeField] public Animator SceneTransition { get; set; }
    public PlayerController Player { get; set; }

    // The GODController is sorta hard-coded for the moment, sorry about that Devlyn!
    // ^ That should be fine

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        Initialize();     
    }

    private void Update()
    {
        GODController.Instance.Update();
    }

    private void Initialize()
    {
        GODController.Instance.Initalize();
    }

    public bool TryGetPlayer(out PlayerController player)
    {
        player = Player;
        
        if (Player == null) 
            return false;    
        
        return true;
    } 
    public void GameOver(GameOverType type)
    {
        switch (type)
        {
            case GameOverType.FearWander: break;
            case GameOverType.FearWall: break;
            case GameOverType.Timeout: break;
            default: break;
            
        }
    }
}
