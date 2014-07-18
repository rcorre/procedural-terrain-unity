using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NavGraph {
    public TerrainTile[] TilesInRange { get; private set; }
    private int[] _distance;
    private int[] _parent;
    private TileMap _tileMap;

    public NavGraph(TileMap tileMap, int startRow, int startCol, int ap) {
        _tileMap = tileMap;
        int numNodes = tileMap.NumRows * tileMap.NumCols;
        _distance = new int[numNodes];
        for (int i = 0; i < numNodes; i++) {
            _distance[i] = int.MaxValue;
        }
	int startIndex = TileToIndex(tileMap.TileAt(startRow, startCol));
        _distance[startIndex] = 0;

        var _parent = new int[numNodes];

        var queue = new List<int>();
        queue.Add(startIndex);
        while (queue.Count > 0) {
	    var u = queue.OrderBy(a => _distance[a]).First();
	    queue.Remove(u);

	    var tile = IndexToTile(u);
	    foreach (var v in tileMap.TileNeighbors(tile)) {
		int alt = _distance[u] + v.MoveCost;
		int idx = TileToIndex(v);
		if (alt < _distance[idx]) {
		    _distance[idx] = alt;
		    _parent[idx] = u;
		    queue.Add(idx);
		}
	    }
        }

        List<TerrainTile> tilesInRange = new List<TerrainTile>();
        for (int i = 0; i < numNodes; i++) {
            if (_distance[i] <= ap) {
                tilesInRange.Add(IndexToTile(i));
            }
        }
        TilesInRange = tilesInRange.ToArray();
    }

    public int CostToTile(TerrainTile tile) {
        var idx = TileToIndex(tile);
        if (idx < _distance.Length) {
            return _distance[idx];
        }
        return int.MaxValue;
    }

    private int TileToIndex(TerrainTile tile) {
        return tile.Row * _tileMap.NumCols + tile.Col;
    }

    private TerrainTile IndexToTile(int idx) {
        return _tileMap.TileAt(idx / _tileMap.NumRows, idx % _tileMap.NumCols);
    }
}
