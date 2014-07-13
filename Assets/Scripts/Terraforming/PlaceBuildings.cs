using UnityEngine;
using System.Collections;

public class PlaceBuildings : Terraformer {
    TerrainTile[,] _tiles;
    int _numRows, _numCols;
    public BasicUnit WallPrefab;
    public int BuildingSize;
    public bool FlattenTerrain;
    public bool DestroyObstacles;

    public override void Apply(TerrainTile[,] tiles) {
        _tiles = tiles;
        _numRows = tiles.GetLength(0);
        _numCols = tiles.GetLength(1);
        int minRow = Mathf.Max(0, Row - Range);
        int maxRow = Mathf.Min(_numRows, Row + Range);
        int minCol = Mathf.Max(0, Col - Range);
        int maxCol = Mathf.Min(_numRows, Col + Range);
        int nSplotches = (int)(Density * Range * Range);
        for (int i = 0; i < nSplotches; i++) {
            int row = Random.Range(minRow, maxRow);
            int col = Random.Range(minCol, maxCol);
            PlaceBuilding(row, col, BuildingSize, WallPrefab);
        }
    }

    // create a building centered around (centerRow, centerCol)
    private void PlaceBuilding(int centerRow, int centerCol, int size, BasicUnit wallPrefab) {
	// start/end row are the rows of the top and bottom diamond points
        int startRow = Mathf.Clamp(centerRow - size / 2, 0, _numRows - 1);
        int endRow   = Mathf.Clamp(centerRow + size / 2, 0, _numRows - 1);
        int startCol = Mathf.Clamp(centerCol - size / 2, 0, _numCols - 1);
        int endCol   = Mathf.Clamp(centerCol + size / 2, 0, _numCols - 1);
        var floorElevation = _tiles[centerRow, centerCol].Elevation;
	// place row walls
        for (int row = startRow; row < endRow; row++) {
            PlaceWall(startCol, row, wallPrefab, floorElevation);
            PlaceWall(endCol, row, wallPrefab, floorElevation);
        }
	// place column walls
        for (int col = startCol; col < endCol; col++) {
            PlaceWall(col, startRow, wallPrefab, floorElevation);
            PlaceWall(col, endRow, wallPrefab, floorElevation);
        }
    }

    private void PlaceWall(int row, int col, BasicUnit wallPrefab, int buildingElevation) {
        var tile = _tiles[row, col];
        if (tile.UnitOnTile != null) {
            if (DestroyObstacles) {
                DestroyImmediate(tile.UnitOnTile.gameObject);
                tile.UnitOnTile = null;
            }
            else {
                return;
            }
        }
        if (FlattenTerrain) {
            tile.Elevation = buildingElevation;
        }
        var obstacle = (BasicUnit)GameObject.Instantiate(wallPrefab);
        obstacle.transform.parent = tile.transform.parent;	// nest obstacle under the tilemap
        tile.UnitOnTile = obstacle;
    }
}