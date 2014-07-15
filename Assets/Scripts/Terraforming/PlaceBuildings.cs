using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Coordinate {
    public int row, col;

    public Coordinate(int row, int col) {
        this.row = row;
        this.col = col;
    }
}

public class PlaceBuildings : Terraformer {
    TerrainTile[,] _tiles;
    int _numRows, _numCols;
    List<Coordinate> _doorLocations;

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
            PlaceBuilding(row, col, BuildingSize, WallPrefab, Direction.North);
        }
    }

    // try to clear an area for the building based on EnableObstacleDestruction and EnableTerrainFlattening
    private void ClearArea(int startRow, int endRow, int startCol, int endCol, int floorElevation) {
        for (int row = startRow; row < endRow; row++) {
            for (int col = startCol; col < endCol; col++) {
                var tile = _tiles[row, col];
                if (tile.UnitOnTile && EnableObstacleDestruction) {
                    DestroyImmediate(tile.UnitOnTile.gameObject);
                    tile.UnitOnTile = null;
                }
                if (EnableTerrainFlattening) {
                    tile.Elevation = floorElevation;
                }
            }
        }
    }

    // create a building centered around (centerRow, centerCol)
    private void PlaceBuilding(int centerRow, int centerCol, int size, BasicUnit wallPrefab, Direction externalDoorSides) {
        // start/end row are the rows of the top and bottom diamond points
        int startRow = Mathf.Clamp(centerRow - size / 2, 0, _numRows);
        int endRow = Mathf.Clamp(centerRow + size / 2, 0, _numRows);
        int startCol = Mathf.Clamp(centerCol - size / 2, 0, _numCols);
        int endCol = Mathf.Clamp(centerCol + size / 2, 0, _numCols);
        var floorElevation = _tiles[centerRow, centerCol].Elevation;
        ClearArea(startRow, endRow, startCol, endCol, floorElevation);
        _doorLocations = GetDoorLocations(startRow, endRow - 1, startCol, endCol - 1, externalDoorSides);
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

    List<Coordinate> GetDoorLocations(int southRow, int northRow, int westCol, int eastCol, Direction doorSides) {
        var locations = new List<Coordinate>();
        if ((doorSides & Direction.South) == Direction.South) {
	    locations.Add(new Coordinate(southRow, Random.Range(westCol + 1, eastCol)));
        }
        if ((doorSides & Direction.North) == Direction.North) {
	    locations.Add(new Coordinate(northRow, Random.Range(westCol + 1, eastCol)));
        }
        if ((doorSides & Direction.East) == Direction.East) {
	    locations.Add(new Coordinate(Random.Range(southRow + 1, northRow), eastCol));
        }
        if ((doorSides & Direction.West) == Direction.West) {
	    locations.Add(new Coordinate(Random.Range(southRow + 1, northRow), westCol));
        }
        return locations;
    }

    private void PlaceWall(int row, int col, BasicUnit wallPrefab) {
        var tile = _tiles[row, col];
	// skip placing wall if it is a door spot.
        if (_doorLocations.Contains(new Coordinate(row, col))) {
            return;  //TODO: place a door object
        }
        if (!tile.UnitOnTile) { // only place wall if there is no obstruction
            var wall = (BasicUnit)GameObject.Instantiate(wallPrefab);
            wall.transform.parent = tile.transform.parent;	// nest obstacle under the tilemap
            tile.UnitOnTile = wall;
        }
    }
}