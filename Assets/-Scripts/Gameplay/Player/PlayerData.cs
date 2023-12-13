using System.Threading.Tasks;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; internal set; }

    public float BatteryLife { get; set; }
    public float FearLevel { get; set; }
    public float ElapsedTime { get; set; }
    public bool InMenu { get; set; } = true;
    public bool ToyOn { get; set; } = false;
    public bool InFearWall { get; set; } = false;
    public string LevelNumber { get; set; }
    public int LevelID { get; set; }
    public bool BatteryLifeSet { get; set; }

    private void Awake()
    {
        Instance ??= this;
        Task _ = Timer();
    }

    private async Task Timer()
    {
        while (true)
        {
            if (!InMenu)
                ElapsedTime += Time.deltaTime;
            
            await Task.Yield();
        }
    }
}
