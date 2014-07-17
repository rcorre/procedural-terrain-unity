using UnityEngine;
using System.Collections;
using System.Linq;

public class BattleController : MonoBehaviour {
    public float PhaseTime = 1.0f; // time in seconds to wait between phases
    Actor[] _allUnits;
    BattleState _state;

    // Use this for initialization
    void Start() {
        _allUnits = GameObject.FindObjectsOfType<Actor>();
        _state = new PassPhases(_allUnits, PhaseTime);
    }

    // Update is called once per frame
    void Update() {
        var nextstate = _state.Update();
        if (nextstate != null) { // state transition requested
            _state.OnExit();
            _state = nextstate;
        }
    }

    public void HandleTileClick(TerrainTile tile) {
        var nextstate = _state.HandleTileClick(tile);
        if (nextstate != null) { // state transition requested
            _state.OnExit();
            _state = nextstate;
        }
    }

    /// <summary>
    /// represents a state of the battle controller
    /// </summary>
    private abstract class BattleState {
	/// <summary>
	/// update this state
	/// </summary>
	/// <returns>a new state if a state change is desired, otherwise null</returns>
        public virtual BattleState Update() { return null; }
	/// <summary>
	/// handle a tile click event within this state
	/// </summary>
	/// <param name="tile"></param>
	/// <returns>a new state if a state change is desired, otherwise null</returns>
        public virtual BattleState HandleTileClick(TerrainTile tile) { return null; }
	/// <summary>
	/// A BattleState may define this to perform some cleanup on a state transition
	/// </summary>
        public virtual void OnExit() { }
    }

    /// <summary>
    /// no unit is active -- this state passes phases until a unit is active
    /// </summary>
    private class PassPhases : BattleState {
        private float _secondsPerPhase;
        private float _phaseTimer;
        private Actor[] _units;

        public PassPhases(Actor[] units, float secondsPerPhase) {
            _units = units;
            _secondsPerPhase = secondsPerPhase;
            _phaseTimer = secondsPerPhase;
        }

        public override BattleState Update() {
            var readyActor = _units.OrderByDescending(a => a.AP).First();
            if (readyActor.ReadyToAct) { // TODO : check whether it is a friend or enemy
                return new PlayerReady(readyActor);
            }

            // nobody ready, pass phases
            _phaseTimer -= Time.deltaTime;
            if (_phaseTimer < 0) {
                Debug.Log("A phase passes");
                _phaseTimer = _secondsPerPhase;
                foreach (var unit in _units) {
                    unit.PassPhase();
                }
            }
            return null;
        }
    }

    /// <summary>
    /// A player's unit is the active unit, but to action has been selected
    /// </summary>
    private class PlayerReady : BattleState {
        Actor _actor;

        public PlayerReady(Actor actor) {
            _actor = actor;
        }

        public override BattleState Update() {
            if (Input.GetKeyUp(KeyCode.W)) {
                return new PlayerConsiderMove(_actor);
            }
            return null;
        }
    }

    /// <summary>
    /// A player has selected to move a unit but not which tile to move it to
    /// </summary>
    private class PlayerConsiderMove : BattleState {
        Actor _actor;
        TerrainTile[] _tilesInRange;
        TileHighlight _highlighter;

        public PlayerConsiderMove(Actor actor) {
            _actor = actor;
            var pathFinder = GameObject.FindObjectOfType<PathFinder>();
            _tilesInRange = pathFinder.TilesInMoveRange(_actor.Row, _actor.Col, _actor.AP);
            _highlighter = GameObject.FindObjectOfType<TileHighlight>();
            _highlighter.ShowMoveableTiles(_tilesInRange);
        }

        public override BattleState Update() {
            if (Input.GetKeyUp(KeyCode.W)) { // stop movement selection
                return new PlayerReady(_actor);
            }
            return null;
        }

        public override BattleState HandleTileClick(TerrainTile tile) {
            return null; // TODO : move to clicked tile if it is in range
        }

        public override void OnExit() {
            _highlighter.ClearOverlay();
        }
    }
}