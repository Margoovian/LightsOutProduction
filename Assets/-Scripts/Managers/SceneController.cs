using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Serializable] public class Level
    {
        public string Name;
        public string AltLevel;
    }
    [Serializable] public class SceneCombo 
    {
        public SceneCombo() { }
        public SceneCombo(Level Level, int LoadOrder) {
            this.Level = Level;
            this.LoadOrder = LoadOrder;
        }

        public void Init()
        {
            RandomLevel = RandomLevelFromPool();
        }

        private Level RandomLevelFromPool()
        {
            if (Pool.Length <= 0) 
                return null;

            var random = new System.Random();
            return Pool[random.Next(Pool.Length)];
        }

        public Level Level;
        public Level RandomLevel;
        public int LoadOrder;
        public Level[] Pool;
    } 

    public static SceneController Instance { get; internal set; }
    [field: SerializeField] public SceneCombo[] Scenes { get; set; }
    public Dictionary<int, SceneCombo> SceneGlossary = new();

    internal int _startLevel = -1;
    public int _currentLevel;
    internal bool _isAltLevel;

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

    public int GetStartIndex() => _startLevel;
    public int GetCurrentIndex() => _currentLevel;

    public async void NextLevel(Func<Task> beforeLoad = null, Func<Task> afterLoad = null, Action beforeAction = null, Action afterAction = null)
    {
        if (!SceneGlossary.ContainsKey(_isAltLevel == true ? _currentLevel - 1 : _currentLevel + 1))
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

        Instance._currentLevel = _isAltLevel == true ? Instance._currentLevel -= 1: Instance._currentLevel += 1;

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

        Instance._currentLevel = level;
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
            SceneManager.LoadScene(_isAltLevel == true ? SceneGlossary[level].Level.AltLevel : SceneGlossary[level].Level.Name); 
            return; 
        }

        if (GameManager.Instance.GameSettings.EnableRandomRooms)
        {
            if (SceneGlossary[_currentLevel].RandomLevel == null) 
            { 
                SceneManager.LoadScene(_isAltLevel == true ? SceneGlossary[level].Level.AltLevel: SceneGlossary[level].Level.Name); 
                return; 
            }

            SceneManager.LoadScene(_isAltLevel == true ? SceneGlossary[level].RandomLevel.AltLevel : SceneGlossary[level].RandomLevel.Name);
            return;
        }

        SceneManager.LoadScene(_isAltLevel == true ? SceneGlossary[level].Level.AltLevel : SceneGlossary[level].Level.Name);
    }

}
