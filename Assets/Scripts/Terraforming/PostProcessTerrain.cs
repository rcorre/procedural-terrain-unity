using UnityEngine;
using System.Collections;

/// <summary>
/// Perform a final pass over terrain to fill texture of untextured tiles
/// </summary>
public class PostProcessTerrain : Terraformer {
    public override void Apply(TerrainTile[,] tiles) {
        for (int row = 0; row < tiles.GetLength(0); row++) {
            for (int col = 0; col < tiles.GetLength(1); col++) {
                var tile = tiles[row, col];
                if (!tile.WasTerrainApplied) {
                    var neighbor = pickNeighbor(tiles, row, col);
                    var mat = neighbor.renderer.sharedMaterial;
                    var cost = neighbor.MoveCost;
                    tile.SetTerrain(cost, mat);
                }
            }
          }
    }

    private TerrainTile pickNeighbor(TerrainTile[,] tiles, int row, int col) {
        int neighborRow = (row == 0) ? (row + 1) : (row - 1);
        int neighborCol = (col == 0) ? (col + 1) : (col - 1);
        var neighbor =  tiles[neighborRow, neighborCol];
	// TODO: this isn't perfect. maybe use breadth-first search
        return (neighbor.WasTerrainApplied) ? neighbor : pickNeighbor(tiles, neighborRow, neighborCol);
    }
}