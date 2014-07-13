using UnityEngine;
using System.Collections;

public class PlaceBuildings : Terraformer {
    TerrainTile[,] _tiles;
    int _numRows, _numCols;
    public BasicUnit WallPrefab;
    public int BuildingSize;
    public bool EnableTerrainFlattening;    // if true, flatten terrain to a uniform elevation in any area designated for construction
    public bool EnableObstacleDestruction;  // if true, destroy existing obtacles in any area designated for construction

    public override void Apply(TerrainTile[,] tiles) {
        _tiles = tiles;
        _numRows = tiles.GetLength(0);
        _numCols = tiles.GetLength(1);
        int minRow = Mathf.Max(0, Row - Range);
        int maxRow = Mathf.Min(_numRows, Row + Range);
        int minCol = Mathf.Max(0, Col - Range);
        int maxCol = Mathf.Min(_numCols, Col + Range);
        int nSplotches = (int)(Density * Range * Range);
        for (int i = 0; i < nSplotches; i++) {
            int row = Random.Range(minRow, maxRow);
            int col = Random.Range(minCol, maxCol);
            PlaceBuilding(row, col, BuildingSize, WallPrefab);
        }
    }

    // try to clear an area for the building based on EnableObstacleDestruction and EnableTerrainFlattening
    private void ClearArea(int startRow, int endRow, int startCol, int endCol, int floorElevation) {
        Debug.Log("ClearArea");
        for (int row = startRow; row < endRow; row++) {
            for (int col = startCol; col < endCol; col++) {
                Debug.Log(string.Format("ClearArea: {0}, {1}", row, col));
                var tile = _tiles[row, col];
                if (tile.UnitOnTile && EnableObstacleDestruction) {
                    DestroyImmediate(tile.UnitOnTile.gameObject);
                    tile.UnitOnTile = null;
                    Debug.Log(string.Format("ClearArea: {0}, {1} destroying obstruction", row, col));
                }
                if (EnableTerrainFlattening) {
                    tile.Elevation = floorElevation;
                    Debug.Log(string.Format("ClearArea: {0}, {1} flattening terrain", row, col));
                }
            }
        }
    }

    // create a building centered around (centerRow, centerCol)
    private void PlaceBuilding(int centerRow, int centerCol, int size, BasicUnit wallPrefab) {
        // start/end row are the rows of the top and bottom diamond points
        int startRow = Mathf.Clamp(centerRow - size / 2, 0, _numRows);
        int endRow = Mathf.Clamp(centerRow + size / 2, 0, _numRows);
        int startCol = Mathf.Clamp(centerCol - size / 2, 0, _numCols);
        int endCol = Mathf.Clamp(centerCol + size / 2, 0, _numCols);
        var floorElevation = _tiles[centerRow, centerCol].Elevation;
        ClearArea(startRow, endRow, startCol, endCol, floorElevation);
        // place row walls
        for (int row = startRow; row < endRow; row++) {
            PlaceWall(row, startCol,   wallPrefab);
            PlaceWall(row, endCol - 1, wallPrefab);
        }
        // place column walls
        for (int col = startCol; col < endCol; col++) {
            PlaceWall(startRow,   col, wallPrefab);
            PlaceWall(endRow - 1, col, wallPrefab);
        }
    }

    private void PlaceWall(int row, int col, BasicUnit wallPrefab) {
        var tile = _tiles[row, col];
        if (!tile.UnitOnTile) { // only place wall if there is no obstruction
            var wall = (BasicUnit)GameObject.Instantiate(wallPrefab);
            wall.transform.parent = tile.transform.parent;	// nest obstacle under the tilemap
            tile.UnitOnTile = wall;
        }
    }
}