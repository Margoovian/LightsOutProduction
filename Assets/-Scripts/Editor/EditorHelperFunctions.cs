using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class EditorHelperFunctions
{
    public static void GroupWrapper(string label, Action element)
    {
        EditorGUILayout.LabelField(label);
        GUILayout.BeginVertical("GroupBox");
        {
            element();
        }
        GUILayout.EndVertical();
    }

    public static void LabeledWrapper(string label, Action element)
    {
        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(label);
            element();
        }
        GUILayout.EndHorizontal();
    }

    public static void GreyOutWrapper(bool enabled, Action element)
    {
        GUI.enabled = enabled;
        element();
        GUI.enabled = true;
    }

    public static Vector2 MinMaxSlider(Vector2 range, float minLimit, float maxLimit, string title = "Range")
    {
        Vector2 currentRange = new();
        LabeledWrapper(title, () =>
        {
            float min = range.x;
            float max = range.y;

            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.MinMaxSlider(ref min, ref max, minLimit, maxLimit);
            currentRange = new Vector2(min, max);
            currentRange = Vector2.Max(Vector2.Min(EditorGUILayout.Vector2Field("", currentRange), new Vector2(max, maxLimit)), new Vector2(minLimit, min));
            EditorGUILayout.EndVertical();
        });
        return currentRange;
    }

    public static bool EnableWrapper(bool enable, Action element)
    {
        EditorGUILayout.BeginHorizontal();
        bool _enable = EditorGUILayout.Toggle(enable);
        GreyOutWrapper(_enable, element);
        EditorGUILayout.EndHorizontal();
        return _enable;
    }
}
