using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager<T> : MonoBehaviour where T : Manager<T>
{
    public static T Instance { get => (T)_manager; private set => _manager = value; }
    private static Manager<T> _manager = null;
    private void Awake()
    {
        if (!_manager)
        {
            _manager = this;
            Initialize();
        }
    }
    protected abstract void Initialize();
}
