using UnityEngine;
using System.Collections;

// TODO: This shouldn't be a separate class/object
// have TileMapMouse handle this
// it should have a prefab used to create selector
// as well as prefabs for move/attack range, ect
// and provide access to tileundermouse
public class TileSelector : MonoBehaviour {
    private TerrainTile _tileUnderMouse;
    private BattleController _controller; // controller to send notifications to

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

    void Start() {
        _controller = GameObject.FindObjectOfType<BattleController>();
        if (!_controller) { 
            Debug.LogError("TileSelector could not find a BattleController to send mouse notifications to"); 
        }
    }

    void Update() {
        if (Input.GetMouseButtonUp(0)) {
            _controller.HandleTileClick(TileUnderMouse);
        }
    }
}
