using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public PlayerController Player { get; set; }

    private void Awake()
    {
        Instance = this;
        Initialize();     
    }

    private void Initialize()
    {

    }

    public bool TryGetPlayer(out PlayerController player)
    {
        player = Player;
        if (Player == null) return false;    
        return true;
    } 

}
