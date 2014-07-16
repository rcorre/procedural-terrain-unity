using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Tilemap))]
public class TileMapMouse : MonoBehaviour {
    public TileSelector TileSelector;
    Tilemap _tileMap;

    void Start() {
        _tileMap = GetComponent<Tilemap>();
    }

    // check every tile in the tilemap for a collision with a ray sent from the mouse position relative to the camera
    // place the selector on the closest tile
    void Update() {
        TerrainTile closestTileUnderMouse = null;
        float minDistance = float.MaxValue;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        for (int row = 0; row < _tileMap.NumRows; row++) {
            for (int col = 0; col < _tileMap.NumCols; col++) {
                TerrainTile tile = _tileMap.TileAt(row, col);
                if (tile.collider.Raycast(ray, out hitInfo, Mathf.Infinity)) {
                    if (hitInfo.distance < minDistance) {
                        minDistance = hitInfo.distance;
                        closestTileUnderMouse = tile;
                    }
                }
            }
        }
        TileSelector.TileUnderMouse = closestTileUnderMouse;
    }
}
