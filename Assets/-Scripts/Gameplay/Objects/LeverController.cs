using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class LeverController : SwitchBox
{

    [field: SerializeField] public int TimeInMS { get; set; }
    private Task _timer = null;

    private async void StartTimer()
    {
        bool timerIsRunning;
        if( _timer != null) 
            timerIsRunning = !_timer.IsCompleted;
        else
        {
            _timer = HelperFunctions.Timer(TimeInMS);
            return;
        }
        if (timerIsRunning)
            

    }


    private void ResetSwitches()
    {

    }

}
