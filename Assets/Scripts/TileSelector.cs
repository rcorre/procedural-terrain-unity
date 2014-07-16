using UnityEngine;
using System.Collections;

public class TileSelector : MonoBehaviour {
    private TerrainTile TileUnderMouse;

    public TerrainTile SelectedTile {
        get { return TileUnderMouse; }
        set {
            TileUnderMouse = value;
	    transform.position = TileUnderMouse.SurfaceCenter;
        }
    }
}
