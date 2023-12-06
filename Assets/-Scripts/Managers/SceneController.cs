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

        public void Init() => RandomLevel = RandomLevelFromPool();

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
    internal int _currentLevel;
    internal bool _isAltLevel;

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void Awake()
    {
        Instance ??= this;
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
        if (!SceneGlossary.ContainsKey(_isAltLevel? _currentLevel - 1 : _currentLevel + 1))
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

        Instance._currentLevel += 1;

        TryLoadRandom(_currentLevel);
        if (afterLoad != null)
        {
            Task output = afterLoad?.Invoke();
            if (output != null) await output;
            afterAction?.Invoke();
        }

    }

    public void LoadSpecific(int level, Action beforeLoad = null, Action afterLoad = null)
    {
        if (!SceneGlossary.ContainsKey(level))
        { 
            Debug.LogWarning("Level does not exist!"); 
            return; 
        }

        beforeLoad?.Invoke();
        TryLoadRandom(level);
        afterLoad?.Invoke();

        Instance._currentLevel = level;
    }

    public void LoadSpecific(string level, Action beforeLoad = null,  Action afterLoad = null)
    {
        SceneCombo found = null;

        foreach (SceneCombo combo in Scenes)
        {
            if (combo.Level.Name == level)
            {
                found = combo;
                break;
            }
        }

        if (found == null)
        {
            Debug.LogWarning("Level does not exist!");
            return;
        }

        beforeLoad?.Invoke();
        TryLoadRandom(found.LoadOrder);
        afterLoad?.Invoke();

        Instance._currentLevel = found.LoadOrder;
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
            SceneManager.LoadScene(_isAltLevel? SceneGlossary[level].Level.AltLevel : SceneGlossary[level].Level.Name); 
            return; 
        }

        if (GameManager.Instance.GameSettings.EnableRandomRooms)
        {
            if (SceneGlossary[_currentLevel].RandomLevel == null) 
            { 
                SceneManager.LoadScene(SceneGlossary[level].Level.Name); 
                return; 
            }

            SceneManager.LoadScene(SceneGlossary[level].RandomLevel.Name);
            return;
        }

        SceneManager.LoadScene(SceneGlossary[level].Level.Name);
    }
}
