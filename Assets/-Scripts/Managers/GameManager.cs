using System;
using UnityEngine;

public class GameManager : Manager<GameManager>
{
    [Serializable]
    public class InteractionProperties
    {
        [field: SerializeField] public Sprite Sprite { get; set; }
        [field: SerializeField] public Vector3 Offset { get; set; }
        [field: SerializeField] public float UniformScale { get; set; }
    }

    [field: Header("Generic")]
    [field: SerializeField] public Cinemachine.CinemachineVirtualCamera Camera { get; set; }
    [field: SerializeField] public GameSettings GameSettings { get; set; }
    [field: SerializeField] public PlayerData PlayerData { get; set; }
    [field: SerializeField] public Animator SceneTransition { get; set; }
    [field: SerializeField] public InteractionProperties InteractProperties { get; set; }
    public PlayerController Player { get; set; }


    // The GODController is sorta hard-coded for the moment, sorry about that Devlyn!
    // ^ That should be fine


    private void Update()
    {
        GODController.Instance.Update();
    }

    protected override void Initialize()
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
            case GameOverType.Timeout: break;   // This could be an easter egg, if you stay on the start level for a long time?
            default: break;
            
        }
    }

    public void ScoreGame()
    {
        
    }
}
