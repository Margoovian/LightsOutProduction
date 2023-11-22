using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Manager<SceneController>
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

    [field: SerializeField] public SceneCombo[] Scenes { get; set; }
    public Dictionary<int, SceneCombo> SceneGlossary = new();

    internal int _startLevel = -1;
    internal int _currentLevel;
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    protected override void Initialize()
    {
        _currentLevel = _startLevel;

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

    public int GetStartIndex()
    {
        return _startLevel;
    }

    public int GetCurrentIndex()
    {
        return _currentLevel;
    }

    public async void NextLevel(Func<Task> beforeLoad = null, Func<Task> afterLoad = null, Action beforeAction = null, Action afterAction = null)
    {
        if (!SceneGlossary.ContainsKey(_currentLevel + 1))
        { 
            Debug.LogWarning("No More Levels!"); 
            return;
        }

        if (beforeLoad != null)
        {
            Task output = beforeLoad?.Invoke();
            if (output != null) await output;
            beforeAction?.Invoke();
        }
        _currentLevel += 1;

        TryLoadRandom(_currentLevel);
        if (afterLoad != null)
        {
            Task output = afterLoad?.Invoke();
            if (output != null) await output;
            afterAction?.Invoke();
        }

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

        _currentLevel = level;
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
