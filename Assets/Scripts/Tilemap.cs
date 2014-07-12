using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tilemap : MonoBehaviour {
    const string TileNameFormat = "Tile<{0},{1}>";
    /// <summary>
    /// tilemap will be constructed from these tiles
    /// </summary>
    public GameObject TileObject;

    public int NumRows = 10; 
    public int NumCols = 10;

    GameObject[,] _tiles;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private float tileSizeX { get { return TileObject.transform.localScale.x; } }
    private float tileSizeY { get { return TileObject.transform.localScale.y; } }
    private float tileSizeZ { get { return TileObject.transform.localScale.z; } }

    public void GenerateTerrain() {
        Clear();
        _tiles = new GameObject[NumRows, NumCols];
        for (int row = 0; row < NumRows; row++) {
            for (int col = 0; col < NumCols; col++) {
                AddTile(row, col, 0);
            }
        }
    }

    public void Clear() {
        foreach (var tile in GetComponentsInChildren<TerrainTile>()) {
            DestroyImmediate(tile.gameObject);
        }
        _tiles = null;
    }

    private void AddTile(int row, int col, int elevation) {
        var tile = (GameObject)GameObject.Instantiate(TileObject);
        tile.transform.parent = transform; // tile is a subobject of the Tilemap
        tile.transform.localScale = new Vector3(tileSizeX, (elevation + 1) * tileSizeY, tileSizeZ);
        tile.transform.position = new Vector3(col * tileSizeX, 0, row * tileSizeZ);
        tile.name = string.Format(TileNameFormat, row, col);
        _tiles[row, col] = tile;
    }
}
