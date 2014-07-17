using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Flags] 
public enum Direction {
    North = 0, // points toward positive z
    East  = 1, // points toward positive x
    South = 2, // points toward negative z
    West  = 4, // points toward negative x
}

public class TileMap : MonoBehaviour {
    /// <summary>
    /// tilemap will be constructed from these tiles
    /// </summary>
    public GameObject TileObject;

    public List<Terraformer> Terraformers;

    public int NumRows = 10;
    public int NumCols = 10;

    private TerrainTile[,] _tiles;

    // Use this for initialization
    void Awake() {
        GenerateTerrain();
    }

    // Update is called once per frame
    void Update() {

    }

    public Vector3 TileSize { get { return TileObject.transform.localScale; } }

    public void GenerateTerrain() {
        Clear();
        _tiles = new TerrainTile[NumRows, NumCols];
        for (int row = 0; row < NumRows; row++) {
            for (int col = 0; col < NumCols; col++) {
                AddTile(row, col, 0);
            }
        }
        foreach (var shaper in Terraformers) {
            shaper.Apply(_tiles);
        }
    }

    public TerrainTile TileAt(int row, int col) {
        return _tiles[row, col];
    }

    public TerrainTile[] TileNeighbors(int row, int col) {
        List<TerrainTile> neighbors = new List<TerrainTile>();
        if (row > 0) { neighbors.Add(_tiles[row - 1, col]); }
        if (col > 0) { neighbors.Add(_tiles[row, col - 1]); }
        if (row < NumRows - 1) { neighbors.Add(_tiles[row + 1, col]); }
        if (col < NumCols - 1) { neighbors.Add(_tiles[row, col + 1]); }
        return neighbors.ToArray();
    }

    public TerrainTile[] TileNeighbors(TerrainTile tile) {
        return TileNeighbors(tile.Row, tile.Col);
    }

    public void Clear() {
	// destroy all tile objects
        foreach (var tile in GetComponentsInChildren<TerrainTile>()) {
            DestroyImmediate(tile.gameObject);
        }
	// destroy all unit objects
        foreach (var tile in GetComponentsInChildren<BasicUnit>()) {
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
        terrainData.Initialize();
        terrainData.Row = row;
        terrainData.Col = col;
        terrainData.Elevation = elevation;
        _tiles[row, col] = terrainData;
    }

}