using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; internal set; }
    [field: SerializeField] public Cinemachine.CinemachineVirtualCamera Camera { get; set; }
    [field: SerializeField] public GameSettings GameSettings;
    [field: SerializeField] public PlayerData PlayerData;
    public PlayerController Player { get; set; }

    private void Awake()
    {
        if (!Instance)
            Instance = this;

        Initialize();     
    }

    private void Initialize()
    {
        //Debug.LogWarning("Add initalization code here!");
    }

    public bool TryGetPlayer(out PlayerController player)
    {
        player = Player;
        
        if (Player == null) 
            return false;    
        
        return true;
    } 

}
