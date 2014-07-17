using UnityEngine;
using System.Collections;

public class TerrainTile : MonoBehaviour {
    const string TileNameFormat = "Tile<{0},{1}>";
    const int ImpassableCost = 9999;
    public bool WasTerrainApplied { get; private set; }
    private int _elevation;
    private int _row, _col;
    private BasicUnit _unitOnTile;
    private int _moveCost = 10;

    public int MoveCost {
        get {
            if (_unitOnTile && _unitOnTile.Impassable) {
                return ImpassableCost;
            }
            return _moveCost;
        }
        private set {
            _moveCost = value;
        }
    }

    float _sizeX, _sizeY, _sizeZ;

    public void Initialize() {
        var scale = transform.localScale;
        _sizeX = scale.x;
        _sizeY = scale.y;
        _sizeZ = scale.z;
    }

    public void SetTerrain(int moveCost, Material terrainMaterial) {
        gameObject.renderer.material = terrainMaterial;
        MoveCost = moveCost;
        WasTerrainApplied = true;
    }

    /// <summary>
    /// Row within containing tilemap
    /// </summary>
    public int Row {
        get { return _row; }
        set {
            _row = value;
            transform.localPosition = new Vector3(_col * _sizeX, 0, _row * _sizeZ);
            EvaluatePosition();
        }
    }

    /// <summary>
    /// Column within containing tilemap
    /// </summary>
    public int Col {
        get { return _col; }
        set {
            _col = value;
            EvaluatePosition();
        }
    }

    public int Elevation {
        get { return _elevation; }
        set {
            _elevation = value;
            transform.localScale = new Vector3(_sizeX, (value + 1) * _sizeY, _sizeZ);
        }
    }

    /// <summary>
    /// point at which an object "standing" on this tile should be positioned
    /// </summary>
    public Vector3 SurfaceCenter {
        get {
            var trans = transform;
            MeshFilter mf = GetComponent<MeshFilter>();
            Vector3 objSize = mf.sharedMesh.bounds.size;
            Vector3 objScale = trans.localScale;
            float objHeight = objSize.y * objScale.y;
            return new Vector3(trans.position.x, trans.position.y + objHeight / 2, trans.position.z);
        }
    }

    public BasicUnit UnitOnTile {
        get { return _unitOnTile; }
        set {
            _unitOnTile = value;
            if (_unitOnTile != null) {
                _unitOnTile.CurrentTile = this;
            }
        }
    }

    void EvaluatePosition() {
        transform.localPosition = new Vector3(_col * _sizeX, 0, _row * _sizeZ);
        name = string.Format(TileNameFormat, _row, _col);
    }
}