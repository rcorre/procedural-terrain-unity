using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Tilemap))]
public class TileMapInspector : Editor {
    float value = 0.5f;
    /// <summary>
    /// Create GUI within editor
    /// </summary>
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        if (GUILayout.Button("regenerate")) {
            Tilemap tileMap = (Tilemap)target;
            tileMap.GenerateTerrain();
        }

        if (GUILayout.Button("clear")) {
            Tilemap tileMap = (Tilemap)target;
            tileMap.Clear();
        }

	// just for fun -- create a slider that does nothing
        EditorGUILayout.BeginVertical();
        value = EditorGUILayout.Slider(value, 0f, 2.0f);
        EditorGUILayout.EndVertical();
    }
}
