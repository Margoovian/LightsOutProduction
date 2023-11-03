using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerData))]
public class PlayerDataEditor : Editor
{

    public override void OnInspectorGUI()
    {

        PlayerData self = (PlayerData)target;

        HelperFunctions.GroupWrapper("Data", () => {

            if(GameManager.Instance)
                HelperFunctions.LabeledWrapper($"Fear Level {self.FearLevel/GameManager.Instance.GameSettings.MaxFear*100}%", () => {
                    GUILayout.BeginVertical("GroupBox");
                    EditorGUILayout.FloatField(self.FearLevel);
                    EditorGUILayout.Slider(self.FearLevel,0, GameManager.Instance.GameSettings.MaxFear);
                    GUILayout.EndVertical();
                });
            else
                HelperFunctions.LabeledWrapper($"Fear Level", () => {
                    GUILayout.BeginVertical("GroupBox");
                    EditorGUILayout.FloatField(self.FearLevel);
                    GUILayout.EndVertical();
                });

            GUILayout.Space(24);

            if (GameManager.Instance)
                HelperFunctions.LabeledWrapper($"Battery Level {self.BatteryLife / GameManager.Instance.GameSettings.GlowToyMaxBattery * 100}%", () => {
                    GUILayout.BeginVertical("GroupBox");
                    EditorGUILayout.FloatField(self.BatteryLife);
                    EditorGUILayout.Slider(self.BatteryLife, 0, GameManager.Instance.GameSettings.GlowToyMaxBattery);
                    GUILayout.EndVertical();
                });
            else
                HelperFunctions.LabeledWrapper($"Battery Level", () => {
                    GUILayout.BeginVertical("GroupBox");
                    EditorGUILayout.FloatField(self.BatteryLife);
                    GUILayout.EndVertical();
                });

            GUILayout.Space(24);

            HelperFunctions.LabeledWrapper($"Elapsed Time", () => {
                EditorGUILayout.LabelField($"{self.ElapsedTime}(s)");
            });
        });
    }

}
