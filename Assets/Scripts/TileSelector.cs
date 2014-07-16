using UnityEngine;
using System.Collections;

public class TileSelector : MonoBehaviour {
    private TerrainTile _tileUnderMouse;

    public TerrainTile TileUnderMouse {
        get { return _tileUnderMouse; }
        set {
            if (value) { // place tile selector on a tile
                gameObject.SetActive(true);
                _tileUnderMouse = value;
                transform.position = _tileUnderMouse.SurfaceCenter;
            }
            else { // indicates no tile is under mouse -- hide selector
                gameObject.SetActive(false);
            }
        }
    }
}
