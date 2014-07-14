using UnityEngine;
using System.Collections;

public class TextureTerrain : Terraformer {
    TerrainTile[,] _tiles;
    int _numRows, _numCols;
    public Material TerrainMaterial;
    public int MoveCost = 10;

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
            ApplyMaterialSplotch(row, col, 5);
        }
    }

    // create a diamond shaped splotch extending <radius> tiles from (centerRow, centerCol)
    private void ApplyMaterialSplotch(int centerRow, int centerCol, int radius) {
	// start/end row are the rows of the top and bottom diamond points
        int startRow = Mathf.Clamp(centerRow - radius, 0, _numRows);
        int endRow   = Mathf.Clamp(centerRow + radius, 0, _numRows);
        for (int row = startRow; row < endRow; row++) {
	    // width is how many columns to fill out within this row. it is highest at centerRow
            int width = radius - Mathf.Abs(row - centerRow);
            int startCol = Mathf.Clamp(centerCol - width, 0, _numCols);
            int endCol = Mathf.Clamp(centerCol + width, 0, _numCols);
            for (int col = startCol; col < endCol; col++) {
                var tile = _tiles[row, col];
                tile.SetTerrain(MoveCost, TerrainMaterial);
            }
        }
    }

}
