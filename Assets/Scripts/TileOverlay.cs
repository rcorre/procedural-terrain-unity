using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileOverlay : MonoBehaviour {
    public enum HighlightType {
	Move,
	Attack,
    }

    public GameObject MovementOverlay;
    public GameObject AttackOverlay;
    public GameObject WalkIcon;

    private List<GameObject> _currentOverlay = new List<GameObject>();
    private GameObject _mouseIcon;

    public void HighlightTiles(TerrainTile[] tiles, HighlightType type) {
        var prefab = (type == HighlightType.Move) ? MovementOverlay : AttackOverlay;
        foreach (var tile in tiles) {
            var overlay = (GameObject)Instantiate(prefab);
            overlay.transform.position = tile.SurfaceCenter;
            overlay.transform.parent = transform;
            _currentOverlay.Add(overlay);
        }
    }

    public void DisplayWalkIcon(int moveCost) {
        if (!_mouseIcon) {
            _mouseIcon = (GameObject)Instantiate(WalkIcon);
        }
        var mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        _mouseIcon.transform.position = mousePos;
        _mouseIcon.guiText.text = moveCost.ToString();
    }

    public void ClearIcon() {
        if (_mouseIcon) {
            Destroy(_mouseIcon);
            _mouseIcon = null;
        }
    }

    public void ClearOverlay() {
        foreach (var overlay in _currentOverlay) {
            Destroy(overlay);
        }
        _currentOverlay.Clear();
        ClearIcon();
    }
}
