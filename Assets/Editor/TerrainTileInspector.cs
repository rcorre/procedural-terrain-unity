using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TerrainTile))]
public class TerrainTileInspector : Editor {
    /// <summary>
    /// Selecting any tile within the TileMap should select the tilemap itself
    /// </summary>
    public override void OnInspectorGUI() {
        if (Selection.activeGameObject.GetComponent<TerrainTile>() != null) {
            Selection.activeGameObject = Selection.activeGameObject.transform.parent.gameObject;
        }
    }
}