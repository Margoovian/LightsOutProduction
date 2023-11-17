using System;
using System.Threading.Tasks;
using UnityEditor;
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

    public static async Task Timer(int timeMS) => await Task.Delay(timeMS);

}
