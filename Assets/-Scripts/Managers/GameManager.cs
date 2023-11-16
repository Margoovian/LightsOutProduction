using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; internal set; }
    [field: SerializeField] public Cinemachine.CinemachineVirtualCamera Camera { get; set; }
    [field: SerializeField] public GameSettings GameSettings;
    [field: SerializeField] public PlayerData PlayerData;
    [field: SerializeField] public AudioMixer GameMixer;
    public PlayerController Player { get; set; }

    // The GODController is sorta hard-coded for the moment, sorry about that Devlyn!

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

}
