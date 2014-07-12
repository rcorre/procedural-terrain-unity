using UnityEngine;
using System.Collections;

public class TerrainTile : MonoBehaviour {
    const string TileNameFormat = "Tile<{0},{1}>";
    private int _elevation;
    private int _row, _col;

    float _sizeX, _sizeY, _sizeZ;

    public void Initialize() {
        var scale = transform.localScale;
        _sizeX = scale.x;
        _sizeY = scale.y;
        _sizeZ = scale.z;
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

    void EvaluatePosition() {
        transform.localPosition = new Vector3(_col * _sizeX, 0, _row * _sizeZ);
        name = string.Format(TileNameFormat, _row, _col);
    }
}