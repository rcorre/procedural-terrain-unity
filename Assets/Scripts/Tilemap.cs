using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tilemap : MonoBehaviour {
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
        MakeMountain(10, 10, 5, 1);
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
        _tiles[row, col] = tile;
    }

    private void MakeMountain(int peakRow, int peakCol, int height, float slope) {
        int startRow = Mathf.Clamp(peakRow - (int)(height / slope), 0, NumRows);
        int endRow = Mathf.Clamp(peakRow + (int)(height / slope), 0, NumRows);
        int startCol = Mathf.Clamp(peakCol - (int)(height / slope), 0, NumCols);
        int endCol = Mathf.Clamp(peakCol + (int)(height / slope), 0, NumCols);
        for (int row = 0; row < NumRows; row++) {
            for (int col = 0; col < NumCols; col++) {
                float distance = Mathf.Sqrt(Mathf.Pow(row - peakRow, 2) + Mathf.Pow(col - peakCol, 2));
                int heightAdjustment = Mathf.Max(0, (int)(height - slope * distance)); // no negative adjustments
                var tile = _tiles[row, col];
                tile.GetComponent<TerrainTile>().Elevation += heightAdjustment;
            }
        }
    }
}
