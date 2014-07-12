using UnityEngine;
using System.Collections;

public class TerrainTile : MonoBehaviour {
    const string TileNameFormat = "Tile<{0},{1}>";
    private int _elevation;
    private int _row, _col;


    public float SizeX { get { return transform.localScale.x; } }
    public float SizeY { get { return transform.localScale.y; } }
    public float SizeZ { get { return transform.localScale.z; } }

    /// <summary>
    /// Row within containing tilemap
    /// </summary>
    public int Row {
        get { return _row; }
        set {
            _row = value;
            transform.localPosition = new Vector3(_col * SizeX, 0, _row * SizeZ);
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
            transform.localScale = new Vector3(SizeX, (value + 1) * SizeY, SizeZ);
        }
    }

    void EvaluatePosition() {
        transform.localPosition = new Vector3(_col * SizeX, 0, _row * SizeZ);
        name = string.Format(TileNameFormat, _row, _col);
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}