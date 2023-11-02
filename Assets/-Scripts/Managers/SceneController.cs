using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    [Serializable] public class SceneCombo {
        public SceneCombo() { }
        public SceneCombo(string Name, int LoadOrder) {
            this.Name = Name;
            this.LoadOrder = LoadOrder;
        }
        public string Name;
        public int LoadOrder;
    } 

    public static SceneController Instance { get; internal set; }
    [field: SerializeField] public SceneCombo[] Scenes { get; set; }
    internal int _currentLevel = -1;
    public Dictionary<int, string> SceneGlossary = new();

    private void Awake() { 
        Instance = this;
        Initialize();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }


    internal void Initialize()
    {
        foreach(SceneCombo combo in Scenes)
        {
            if (SceneGlossary.ContainsKey(combo.LoadOrder)) continue;
            SceneGlossary.Add(combo.LoadOrder, combo.Name);
        }

        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.sceneUnloaded += OnSceneUnloaded;


    }

    public void NextLevel(Action beforeLoad = null)
    {
        Debug.Log(SceneGlossary[_currentLevel + 1]);
        if (!SceneGlossary.ContainsKey(_currentLevel + 1)) { Debug.LogWarning("No More Levels!"); return; }
        beforeLoad?.Invoke();
        Instance._currentLevel += 1;
        SceneManager.LoadScene(SceneGlossary[_currentLevel]);

    }

    public void LoadSpecific(int level, Action beforeLoad = null)
    {
        if (!SceneGlossary.ContainsKey(level)) { Debug.LogWarning("Level does not exist!"); return; }
        beforeLoad?.Invoke();
        SceneManager.LoadScene(SceneGlossary[level]);

        Instance._currentLevel = level;
    }

    public void LoadScene(string scene) => SceneManager.LoadScene(scene);

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {

    }

    private void OnSceneUnloaded(Scene scene)
    {

    }

}
