using UnityEngine;
using System.Collections;

public class Actor : BasicUnit {
    public const int MaxAp = 100;

    public int StartRow, StartCol;
    public int AP;
    public int Initiative;

    // Use this for initialization
    void Start() {
        var tileMap = GameObject.FindObjectOfType<Tilemap>();
        CurrentTile = tileMap.TileAt(StartRow, StartCol);
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
