using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(TileMap))]
public class PathFinder : MonoBehaviour {
    TileMap _tileMap;
    // Use this for initialization
    void Start() {
        _tileMap = GetComponent<TileMap>();
    }

    public TerrainTile[] TilesInMoveRange(int startRow, int startCol, int ap) {
        int numNodes = _tileMap.NumRows * _tileMap.NumCols;
        var distance = new int[numNodes];
        for (int i = 0; i < numNodes; i++) {
            distance[i] = int.MaxValue;
        }
	int startIndex = TileToIndex(_tileMap.TileAt(startRow, startCol));
        distance[startIndex] = 0;

        var parent = new int[numNodes];

        var queue = new List<int>();
        queue.Add(startIndex);
        while (queue.Count > 0) {
	    var u = queue.OrderBy(a => distance[a]).First();
	    queue.Remove(u);

	    var tile = IndexToTile(u);
	    foreach (var v in _tileMap.TileNeighbors(tile)) {
		int alt = distance[u] + v.MoveCost;
		int idx = TileToIndex(v);
		if (alt < distance[idx]) {
		    distance[idx] = alt;
		    parent[idx] = u;
		    queue.Add(idx);
		}
	    }
        }
        List<TerrainTile> tilesInRange = new List<TerrainTile>();
        for (int i = 0; i < numNodes; i++) {
            if (distance[i] <= ap) {
                tilesInRange.Add(IndexToTile(i));
            }
        }
        return tilesInRange.ToArray();
    }

    private int TileToIndex(TerrainTile tile) {
        return tile.Row * _tileMap.NumCols + tile.Col;
    }

    private TerrainTile IndexToTile(int idx) {
        return _tileMap.TileAt(idx / _tileMap.NumRows, idx % _tileMap.NumCols);
    }
}
