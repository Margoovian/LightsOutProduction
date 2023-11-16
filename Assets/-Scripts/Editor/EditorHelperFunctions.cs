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
}
