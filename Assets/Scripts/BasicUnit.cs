using UnityEngine;
using System.Collections;

public class BasicUnit : MonoBehaviour {
    private TerrainTile _currentTile;

    public TerrainTile CurrentTile {
        get { return _currentTile; }
        set {
            _currentTile = value;
            CenterOnTile(value);
        }
    }

    /* For debugging object base positions
    void OnDrawGizmosSelected() {
        MeshFilter mf = GetComponent<MeshFilter>();
        Vector3 objSize = mf.sharedMesh.bounds.size;
        Vector3 objScale = transform.localScale;
        float height = objSize.y * objScale.y / 2;
	Gizmos.DrawSphere(transform.position - Vector3.up * height, 0.2f);
    }
    */

    private void CenterOnTile(TerrainTile tile) {
	// find the bounds of the object using its mesh filter
        MeshFilter mf = GetComponent<MeshFilter>();
        Vector3 objSize = mf.sharedMesh.bounds.size;
        Vector3 objScale = transform.localScale;
        float height = objSize.y * objScale.y;
	// position the base on the surface of the tile
        transform.position = tile.SurfaceCenter + new Vector3(0, height / 2, 0);
    }
}
