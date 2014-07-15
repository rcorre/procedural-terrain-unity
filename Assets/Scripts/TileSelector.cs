using UnityEngine;
using System.Collections;

public class TileSelector : MonoBehaviour {
    private TerrainTile _selectedTile;

    public TerrainTile SelectedTile {
        get { return _selectedTile; }
        set {
            _selectedTile = value;
            transform.position = _selectedTile.SurfaceCenter;
        }
    }
}
