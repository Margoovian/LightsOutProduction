using System.Threading.Tasks;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; internal set; }

    private Task _timer = null;
    public float BatteryLife { get; set; }
    public float FearLevel { get; set; }
    public float ElapsedTime { get; set; }
    public bool InMenu { get; set; } = true;
    public bool ToyOn { get; set; } = false;
    public string LevelNumber { get; set; }
    public int LevelID { get; set; }

    private void Awake()
    {
        Instance ??= this;
        _timer ??= Timer();
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
