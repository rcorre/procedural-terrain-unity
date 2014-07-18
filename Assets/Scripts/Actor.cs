using UnityEngine;
using System.Collections;

public class Actor : BasicUnit {
    public enum Team {
	Player,
	Enemy,
	Neutral
    }

    public const int MaxAp = 100;

    public Team ActorTeam;
    public int StartRow, StartCol;
    // stats -- TODO: can these be settable only in the editor? custom inspector maybe?
    public int Initiative = 20;
    public int Damage = 40;
    public int AttackRange = 1;
    public int AttackAPCost = 40;

    public int AP { get; private set; }

    // Use this for initialization
    void Start() {
        var tileMap = GameObject.FindObjectOfType<TileMap>();
        if (tileMap.TileAt(StartRow, StartCol).UnitOnTile) {
            Debug.LogError("Cannot place unit on object");
        }
        tileMap.TileAt(StartRow, StartCol).UnitOnTile = this;
        Health = MaxHealth;
    }

    // Update is called once per frame
    void Update() {

    }

    public bool ReadyToAct {
        get {
            return AP >= MaxAp;
        }
    }

    public void PassPhase() {
        AP += Initiative;
    }
}
