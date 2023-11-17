using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Stem))]
public class StemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUI.enabled = true;
        DrawDefaultInspector();

        Stem self = (Stem)target;

        float minPitchValue = self.RandomPitchRange.x;
        float maxPitchValue = self.RandomPitchRange.y;
        float minPitchLimit = -3;
        float maxPitchLimit = 3;

        EditorGUILayout.BeginHorizontal();
        self.EnableRandomPitch = EditorGUILayout.Toggle(self.EnableRandomPitch);
        GUI.enabled = self.EnableRandomPitch;
        EditorHelperFunctions.LabeledWrapper("Random Pitch Range", () => {
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.MinMaxSlider(ref minPitchValue, ref maxPitchValue, minPitchLimit, maxPitchLimit);
            self.RandomPitchRange = new Vector2(minPitchValue, maxPitchValue);
            self.RandomPitchRange = Vector2.Max(Vector2.Min(EditorGUILayout.Vector2Field("", self.RandomPitchRange), new Vector2(maxPitchValue, maxPitchLimit)), new Vector2(minPitchLimit, minPitchValue));
            EditorGUILayout.EndVertical();
        });
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        float minPanValue = self.RandomPanRange.x;
        float maxPanValue = self.RandomPanRange.y;
        float minPanLimit = -1;
        float maxPanLimit = 1;

        EditorGUILayout.BeginHorizontal();
        self.EnableRandomPan = EditorGUILayout.Toggle(self.EnableRandomPan);
        GUI.enabled = self.EnableRandomPan;
        EditorHelperFunctions.LabeledWrapper("Random Pan Range", () =>
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.MinMaxSlider(ref minPanValue, ref maxPanValue, minPanLimit, maxPanLimit);
            self.RandomPanRange = new Vector2(minPanValue, maxPanValue);
            self.RandomPanRange = Vector2.Max(Vector2.Min(EditorGUILayout.Vector2Field("", self.RandomPanRange), new Vector2(maxPanValue, maxPanLimit)), new Vector2(minPanLimit, minPanValue));
            EditorGUILayout.EndVertical();
        });
        EditorGUILayout.EndHorizontal();

        GUI.enabled = true;

        float minVolumeValue = self.RandomVolumeRange.x;
        float maxVolumeValue = self.RandomVolumeRange.y;
        float minVolumeLimit = 0;
        float maxVolumeLimit = 1;

        EditorGUILayout.BeginHorizontal();
        self.EnableRandomVolume = EditorGUILayout.Toggle(self.EnableRandomVolume);
        GUI.enabled = self.EnableRandomVolume;
        EditorHelperFunctions.LabeledWrapper("Random Volume Range", () =>
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.MinMaxSlider(ref minVolumeValue, ref maxVolumeValue, minVolumeLimit, maxVolumeLimit);
            self.RandomVolumeRange = new Vector2(minVolumeValue, maxVolumeValue);
            self.RandomVolumeRange = Vector2.Max(Vector2.Min(EditorGUILayout.Vector2Field("", self.RandomVolumeRange), new Vector2(maxVolumeValue, maxVolumeLimit)), new Vector2(minVolumeLimit, minVolumeValue));
            EditorGUILayout.EndVertical();
        });
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();
    }
}
