using UnityEngine;
using System.Collections;
using System.Linq;

public class BattleController : MonoBehaviour {
    public float PhaseTime = 1.0f; // time in seconds to wait between phases
    private float _phaseTimer;     // times wait between phases
    Actor[] _allUnits;
    Actor _activeUnit;
    PathFinder _pathFinder;
    TerrainTile[] _tilesInRange;

    // Use this for initialization
    void Start() {
        _allUnits = GameObject.FindObjectsOfType<Actor>();
	_phaseTimer = PhaseTime;
	_pathFinder = GameObject.FindObjectOfType<PathFinder>();
    }

    // Update is called once per frame
    void Update() {
        if (_activeUnit) {
            if (Input.GetKeyUp(KeyCode.W)) {
                _tilesInRange = _pathFinder.TilesInMoveRange(_activeUnit.CurrentTile.Row, _activeUnit.CurrentTile.Col, _activeUnit.AP);
                var highlighter = GameObject.FindObjectOfType<TileHighlight>();
                highlighter.HighlightTiles(_tilesInRange);
            }
        }
        else { // no unit is active, pass phases until one is ready
            _activeUnit = PassTimeUntilUnitReady();
        }
    }

    private Actor PassTimeUntilUnitReady() {
        var readyActor = _allUnits.OrderByDescending(a => a.AP).First();
        if (readyActor.ReadyToAct) {
            return readyActor;
        }

	// nobody ready, pass phases
        _phaseTimer -= Time.deltaTime;
        if (_phaseTimer < 0) {
            Debug.Log("A phase passes");
            _phaseTimer = PhaseTime;
            foreach (var unit in _allUnits) {
                unit.PassPhase();
            }
        }
        return null;
    }
}
