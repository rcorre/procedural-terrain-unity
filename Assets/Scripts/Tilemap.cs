using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tilemap : MonoBehaviour {
    /// <summary>
    /// tilemap will be constructed from these tiles
    /// </summary>
    public GameObject TileObject;

    public TerrainShaper shaper;

    public int NumRows = 10;
    public int NumCols = 10;

    TerrainTile[,] _tiles;

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
        _tiles = new TerrainTile[NumRows, NumCols];
        for (int row = 0; row < NumRows; row++) {
            for (int col = 0; col < NumCols; col++) {
                AddTile(row, col, 0);
            }
        }
        shaper.Apply(_tiles);
    }

    public void Clear() {
        foreach (var tile in GetComponentsInChildren<TerrainTile>()) {
            DestroyImmediate(tile.gameObject);
        }
        _tiles = null;
    }

    private void AddTile(int row, int col, int elevation) {
	// create a tile as a subobject of the tilemap
        var tile = (GameObject)GameObject.Instantiate(TileObject);
        tile.transform.parent = transform;
	// set the row, column, and elevation of the tile
        var terrainData = tile.GetComponent<TerrainTile>();
        terrainData.Row = row;
        terrainData.Col = col;
        terrainData.Elevation = elevation;
        _tiles[row, col] = terrainData;
    }

}
