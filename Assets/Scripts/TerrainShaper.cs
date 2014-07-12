using UnityEngine;
using System.Collections;

/// <summary>
/// Many of these may be applied to a tilemap to generate terrain
/// </summary>
public abstract class TerrainShaper : MonoBehaviour {
    public int Row;
    public int Col;
    public int Range;
    public float Density;

    public abstract void Apply(TerrainTile[,] tiles);
}
