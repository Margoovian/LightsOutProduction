using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TilemapToObjects))]
public class TImeMapToObjectsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TilemapToObjects obj = target as TilemapToObjects;
        DrawDefaultInspector();
        
        if(GUILayout.Button("Tiles To Objects")) obj.GenerateObjects();
        if (GUILayout.Button("Destoy Objects")) obj.DestroyObjects();
        if (GUILayout.Button("Reset Glossary")) obj.ResetGlossary();
    }
}
