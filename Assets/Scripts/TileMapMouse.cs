using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TerrainTile))]
public class TileMapMouse : MonoBehaviour {
    TileSelector _tileSelector;

    void Start() {
        _tileSelector = Component.FindObjectOfType<TileSelector>();
    }

    // Update is called once per frame
    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (collider.Raycast(ray, out hitInfo, Mathf.Infinity)) {
            _tileSelector.SelectedTile = GetComponent<TerrainTile>();
        }
    }
}
