using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public class InteractionProperties
    {
        [field: SerializeField] public Sprite Sprite { get; set; }
        [field: SerializeField] public float UniformScale { get; set; }
        [field: SerializeField] public Vector3 Offset { get; set; }
        [field: SerializeField] public Color Color { get; set; }
    }

    [field: Header("Generic")]
    public static GameManager Instance { get; internal set; }
    [field: SerializeField] public Cinemachine.CinemachineVirtualCamera Camera { get; set; }
    [field: SerializeField] public GameSettings GameSettings { get; set; }
    [field: SerializeField] public PlayerData PlayerData { get; set; }
    [field: SerializeField] public Animator SceneTransition { get; set; }
    [field: SerializeField] public GameObject MainGUI { get; set; }
    [field: SerializeField] public InteractionProperties InteractProperties { get; set; }
    public PlayerController Player { get; set; }
    public int PuzzlesCompleted { get; set; }
    public GameOverType GameOverType { get; set; }
    public int PreviousGameScene { get; set; }

    private void Awake()
    {
        Instance ??= this;
        GODController.Instance.Initalize();
    }

    private void Update() => GODController.Instance.Update();

    public bool TryGetPlayer(out PlayerController player)
    {
        player = Player;
        
        if (Player == null) 
            return false;    
        
        return true;
    }

    public void GameOver(GameOverType type) 
    {
        GameOverType = type;
        PreviousGameScene = SceneController.Instance.GetCurrentIndex();
        SceneController.Instance.LoadSpecific("GameEnd");
    }

    public EndRatingEnum CalcEndScore()
    {
        int P = PuzzlesCompleted;
        float B = PlayerData.BatteryLife / 10;
        float T = PlayerData.ElapsedTime;
        int R = (int)((P + B) / T);

        switch(R) // Turn this info else if for more control
        {
            case 0: return EndRatingEnum.F;
            case 1: return EndRatingEnum.E;
            case 2: return EndRatingEnum.D;
            case 3: return EndRatingEnum.C;
            case 4: return EndRatingEnum.B;
            case 5: return EndRatingEnum.A;
            case 6: return EndRatingEnum.S;
            case 7: return EndRatingEnum.SS;
            case 8: return EndRatingEnum.R;
            case 9: return EndRatingEnum.RR;
            case 10: return EndRatingEnum.L;
            default: return EndRatingEnum.X;
        }
        
    }
}
