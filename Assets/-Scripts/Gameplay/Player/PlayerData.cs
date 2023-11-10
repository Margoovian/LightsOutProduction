using System.Threading.Tasks;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private Task _timer = null;
    public float BatteryLife { get; set; }
    public float FearLevel { get; set; }
    public float ElapsedTime { get; set; }
    public bool InMenu { get; set; } = true;

    private void Awake()
    {
        if(_timer == null)
            _timer = Timer();

    }
    private async Task Timer()
    {
        while (true)
        {
            if(!InMenu)
                ElapsedTime += Time.deltaTime;
            await Task.Yield();
        }
    }
}
