using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class HelperFunctions
{
    public static async void WaitForTask(Task timeOutTask, Action onComplete) // Make Return
    {
        if (await Task.WhenAny(timeOutTask, Task.Delay(1000)) == timeOutTask)
            onComplete();
        else
            Debug.LogWarning("Instance of managers not found!");
    }
}
