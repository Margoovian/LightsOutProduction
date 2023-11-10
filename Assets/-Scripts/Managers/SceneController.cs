using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Serializable] public class SceneCombo 
    {
        public SceneCombo() { }
        public SceneCombo(string Name, int LoadOrder) {
            this.Name = Name;
            this.LoadOrder = LoadOrder;
        }

        public void Init()
        {
            RandomLevel = RandomLevelFromPool();
        }

        private string RandomLevelFromPool()
        {
            if (Pool.Length <= 0) 
                return "";

            var random = new System.Random();
            return Pool[random.Next(Pool.Length)];
        }

        public string Name;
        public string RandomLevel;
        public int LoadOrder;
        public string[] Pool;
    } 

    public static SceneController Instance { get; internal set; }
    [field: SerializeField] public SceneCombo[] Scenes { get; set; }
    internal int _currentLevel = -1;
    public Dictionary<int, SceneCombo> SceneGlossary = new();

    private void Awake() {
        if (!Instance)
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
            if (SceneGlossary.ContainsKey(combo.LoadOrder)) 
                continue;

            SceneGlossary.Add(combo.LoadOrder, combo);
            combo.Init();
        }

        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public void NextLevel(Action beforeLoad = null)
    {
        if (!SceneGlossary.ContainsKey(_currentLevel + 1))
        { 
            Debug.LogWarning("No More Levels!"); 
            return; 
        }

        beforeLoad?.Invoke();
        Instance._currentLevel += 1;

        TryLoadRandom(_currentLevel);
    }

    public void LoadSpecific(int level, Action beforeLoad = null)
    {
        if (!SceneGlossary.ContainsKey(level))
        { 
            Debug.LogWarning("Level does not exist!"); 
            return; 
        }

        beforeLoad?.Invoke();
        TryLoadRandom(level);

        Instance._currentLevel = level;
    }

    public void LoadSpecificAndTransfer(int level)
    {
        LoadSpecific(level);

        // this gets the scene loaded previously, not the currently active scene, please fix asap!
        Scene currentScene = SceneManager.GetActiveScene();

        Debug.Log("Reading From Scene: " + currentScene.name);
        foreach (GameObject i in currentScene.GetRootGameObjects())
        {
            Debug.Log(i.name);
        }
    }

    public void LoadScene(string scene) => SceneManager.LoadScene(scene);

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {

    }

    private void OnSceneUnloaded(Scene scene)
    {

    }
    public void NextLevelClickable() => NextLevel();
    public void LoadSpecificClickable(int level) => LoadSpecific(level);

    private void TryLoadRandom(int level)
    {
        if (!GameManager.Instance) 
        { 
            SceneManager.LoadScene(SceneGlossary[level].Name); 
            return; 
        }

        if (GameManager.Instance.GameSettings.EnableRandomRooms)
        {
            if (SceneGlossary[_currentLevel].RandomLevel == "") 
            { 
                SceneManager.LoadScene(SceneGlossary[level].Name); 
                return; 
            }

            SceneManager.LoadScene(SceneGlossary[level].RandomLevel);
            return;
        }

        SceneManager.LoadScene(SceneGlossary[level].Name);
    }

}
