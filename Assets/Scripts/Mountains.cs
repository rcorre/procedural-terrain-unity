using UnityEngine;
using System.Collections;

public class Mountains : TerrainShaper {
    TerrainTile[,] _tiles;
    int _numRows, _numCols;

    public override void Apply(TerrainTile[,] tiles) {
        _tiles = tiles;
        _numRows = tiles.GetLength(0);
        _numCols = tiles.GetLength(1);
	int minRow = Mathf.Max(0, Row - Range);
	int maxRow = Mathf.Min(_numRows, Row + Range);
	int minCol = Mathf.Max(0, Col - Range);
	int maxCol = Mathf.Min(_numRows, Col + Range);
        int nMountains = (int)(Density * tiles.LongLength);
        for (int i = 0; i < nMountains; i++) {
            int row = Random.Range(minRow, maxRow);
            int col = Random.Range(minCol, maxCol);
            MakeMountain(row, col, 5, 1);
        }
    }

    private void MakeMountain(int peakRow, int peakCol, int height, float slope) {
        int startRow = Mathf.Clamp(peakRow - (int)(height / slope), 0, _numRows);
        int endRow = Mathf.Clamp(peakRow + (int)(height / slope), 0, _numRows);
        int startCol = Mathf.Clamp(peakCol - (int)(height / slope), 0, _numCols);
        int endCol = Mathf.Clamp(peakCol + (int)(height / slope), 0, _numCols);
        for (int row = startRow; row < endRow; row++) {
            for (int col = startCol; col < endCol; col++) {
                float distance = Mathf.Sqrt(Mathf.Pow(row - peakRow, 2) + Mathf.Pow(col - peakCol, 2));
                int heightAdjustment = Mathf.Max(0, (int)(height - slope * distance)); // no negative adjustments
                var tile = _tiles[row, col];
                tile.GetComponent<TerrainTile>().Elevation += heightAdjustment;
            }
        }
    }
}
