using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileHighlight : MonoBehaviour {

    public GameObject HighlightOverlay;
    private List<GameObject> _currentOverlay = new List<GameObject>();

    public void ShowMoveableTiles(TerrainTile[] tiles) {
        foreach (var tile in tiles) {
            var overlay = (GameObject)Instantiate(HighlightOverlay);
            overlay.transform.position = tile.SurfaceCenter;
            overlay.transform.parent = transform;
            _currentOverlay.Add(overlay);
        }
    }

    public void ClearOverlay() {
        foreach (var overlay in _currentOverlay) {
            Destroy(overlay);
        }
        _currentOverlay.Clear();
    }
}
