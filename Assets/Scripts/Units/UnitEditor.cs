using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Unit))]
public class UnitEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var unit = target as Unit;

        DrawDefaultInspector();

        if (GUILayout.Button("Initialize"))
        {
            unit.FindTile();
        }
    }

    [MenuItem("Tools/Initialize units")]
    private static void InitializeUnits()
    {
        foreach(Unit unit in FindObjectsOfType<Unit>())
        {
            unit.FindTile();

            EditorUtility.SetDirty(unit);
        }

        AssetDatabase.SaveAssets();
    }
}
