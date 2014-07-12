using UnityEngine;
using System.Collections;

public class Mountains : TerrainShaper {
    TerrainTile[,] _tiles;
    int _numRows, _numCols;

    public override void Apply(TerrainTile[,] tiles) {
        _tiles = tiles;
        _numRows = tiles.GetLength(0);
        _numCols = tiles.GetLength(1);
        MakeMountain(Row, Col, 5, 1);
    }

    private void MakeMountain(int peakRow, int peakCol, int height, float slope) {
        Debug.Log(string.Format("{0} making mountain at {1}, {2}. h={3}, s={4}", name, peakCol, peakRow, height, slope));
        int startRow = Mathf.Clamp(peakRow - (int)(height / slope), 0, _numRows);
        int endRow = Mathf.Clamp(peakRow + (int)(height / slope), 0, _numRows);
        int startCol = Mathf.Clamp(peakCol - (int)(height / slope), 0, _numCols);
        int endCol = Mathf.Clamp(peakCol + (int)(height / slope), 0, _numCols);
        for (int row = 0; row < _numRows; row++) {
            for (int col = 0; col < _numCols; col++) {
                float distance = Mathf.Sqrt(Mathf.Pow(row - peakRow, 2) + Mathf.Pow(col - peakCol, 2));
                int heightAdjustment = Mathf.Max(0, (int)(height - slope * distance)); // no negative adjustments
                var tile = _tiles[row, col];
                tile.GetComponent<TerrainTile>().Elevation += heightAdjustment;
            }
        }
    }
}
