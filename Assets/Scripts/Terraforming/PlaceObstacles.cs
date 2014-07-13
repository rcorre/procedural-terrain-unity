using UnityEngine;
using System.Collections;

public class PlaceObstacles : Terraformer {
    TerrainTile[,] _tiles;
    int _numRows, _numCols;
    public BasicUnit ObstaclePrefab;

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
            PlaceObstacle(row, col, ObstaclePrefab);
        }
    }

    // create a diamond shaped splotch extending <radius> tiles from (centerRow, centerCol)
    private void PlaceObstacle(int targetRow, int targetCol, BasicUnit obstaclePrefab) {
        var tile = _tiles[targetRow, targetCol];
        if (tile.UnitOnTile == null) {
            var obstacle = (BasicUnit) GameObject.Instantiate(obstaclePrefab);
            obstacle.transform.parent = tile.transform.parent;	// nest obstacle under the tilemap
            tile.UnitOnTile = obstacle;
        }
    }
}