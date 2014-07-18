using UnityEngine;
using System.Collections;

public class BasicUnit : MonoBehaviour {
    public enum Team {
	Player,
	Enemy,
	Neutral
    }

    public Team UnitTeam = Team.Neutral;

    // editable
    public bool Impassable = true;
    public int MaxHealth = 100;
    public int DamageResistance = 10;

    private TerrainTile _currentTile;

    public int Health { get; protected set; }
    public TerrainTile CurrentTile {
        get { return _currentTile; }
        set {
            _currentTile = value;
            CenterOnTile(value);
        }
    }

    public int Row { get { return CurrentTile.Row; } }
    public int Col { get { return CurrentTile.Col; } }

    void Start() {
        Health = MaxHealth;
    }

    private void CenterOnTile(TerrainTile tile) {
	// find the bounds of the object using its mesh filter
        MeshFilter mf = GetComponent<MeshFilter>();
        Vector3 objSize = mf.sharedMesh.bounds.size;
        Vector3 objScale = transform.localScale;
        float height = objSize.y * objScale.y;
	// position the base on the surface of the tile
        transform.position = tile.SurfaceCenter + new Vector3(0, height / 2, 0);
    }

    public virtual int DealDamage(int amount) {
        var result = Mathf.Max(amount - DamageResistance, 0);
        Health -= amount;
        if (Health <= 0) {
            Destroy(gameObject); // TODO: handle this better - could cause issues instantly destroying
        }
        return result;
    }

    public bool IsAttackableBy(BasicUnit other) {
        return UnitTeam == Team.Neutral || (UnitTeam != other.UnitTeam);
    }
}
