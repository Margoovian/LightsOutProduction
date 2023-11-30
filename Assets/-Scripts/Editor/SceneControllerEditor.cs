using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneController))]
public class SceneControllerEditor : Editor
{
    private static int selectedLevel;
    private static int selectedDropDown;
    private static int selectedDropDownStartup;
    private static List<string> levelNames;
    private static Dictionary<string,int> levels;
    private static string specificScene;
    private static int setCurrentLevel;
    
    private void OnEnable()
    {
    }
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        SceneController sc = (SceneController)target;

        levelNames = new(); // This is horrible
        levels = new(); // Equally as horrible

        foreach (var pair in sc.Scenes)
        {
            levelNames.Add(pair.Level.Name);
            levels.Add(pair.Level.Name, pair.LoadOrder);
        }

        EditorHelperFunctions.GroupWrapper("Functions", () => {

            GUILayout.BeginHorizontal();

            selectedDropDown = EditorGUILayout.Popup("Selected Level", selectedDropDown, levelNames.ToArray());
            selectedLevel = levels[levelNames.ToArray()[selectedDropDown]];

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Next Level"))
                SceneController.Instance.NextLevel();

            if (GUILayout.Button("To Selected"))
                SceneController.Instance.LoadSpecific(selectedLevel);

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Next Level (Destructive)"))
                SceneController.Instance.NextLevel(() => { Destroy(GameManager.Instance.Player); return null; });

            if (GUILayout.Button("To Selected (Destructive)"))
                SceneController.Instance.LoadSpecific(selectedLevel, () => { Destroy(GameManager.Instance.Player); });

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            specificScene = EditorGUILayout.TextField(specificScene);
            if (GUILayout.Button("To Specific"))
                SceneController.Instance.LoadScene(specificScene);

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            setCurrentLevel = EditorGUILayout.IntField(setCurrentLevel);
            if (GUILayout.Button("Set Scene Index"))
                SetCurrentSceneIndex();

            GUILayout.EndHorizontal();

        });


    }

    private void SetCurrentSceneIndex() => SceneController.Instance._currentLevel = setCurrentLevel;


}
