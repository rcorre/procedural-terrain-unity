using UnityEngine;
using System.Collections;

public class TileHighlight : MonoBehaviour {

    public GameObject HighlightOverlay;

    public void HighlightTiles(TerrainTile[] tiles) {
        foreach (var tile in tiles) {
            var overlay = (GameObject)Instantiate(HighlightOverlay);
            overlay.transform.position = tile.SurfaceCenter;
        }
    }
}
