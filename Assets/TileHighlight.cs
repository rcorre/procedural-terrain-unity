using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileHighlight : MonoBehaviour {
    public enum HighlightType {
	Move,
	Attack,
    }

    public GameObject MovementOverlay;
    public GameObject AttackOverlay;
    private List<GameObject> _currentOverlay = new List<GameObject>();

    public void HighlightTiles(TerrainTile[] tiles, HighlightType type) {
        var prefab = (type == HighlightType.Move) ? MovementOverlay : AttackOverlay;
        foreach (var tile in tiles) {
            var overlay = (GameObject)Instantiate(prefab);
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
