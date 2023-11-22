using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager<T> : MonoBehaviour where T : Manager<T>
{
    public static Manager<T> Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            Initialize();
        }
    }
    protected abstract void Initialize();
}
