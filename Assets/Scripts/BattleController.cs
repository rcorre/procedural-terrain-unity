using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

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

    public void HandleTileHover(TerrainTile tile) {
	_state.HandleTileHover(tile);
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
	/// handle a tile click hover within this state
	/// </summary>
	/// <param name="tile">tile that mouse is hovering over</param>
	/// <returns>a new state if a state change is desired, otherwise null</returns>
        public virtual void HandleTileHover(TerrainTile tile) {}
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
        NavGraph _navGraph;
        TileOverlay _highlighter;

        public PlayerReady(Actor actor) {
            _actor = actor;
            var map = GameObject.FindObjectOfType<TileMap>();
            _navGraph = new NavGraph(map, _actor.Row, _actor.Col, _actor.AP);
            _highlighter = GameObject.FindObjectOfType<TileOverlay>();
            _highlighter.HighlightTiles(_navGraph.TilesInRange, TileOverlay.HighlightType.Move);
        }

        public override BattleState Update() {
            return null;
        }

        public override BattleState HandleTileClick(TerrainTile tile) {
            if (_navGraph.TilesInRange.Contains(tile)) {
                return new ExecuteMove(_actor, _navGraph.PathToTile(tile), _navGraph.CostToTile(tile));
            }
            return null;
        }

        public override void HandleTileHover(TerrainTile tile) {
            if (tile.UnitOnTile && tile.UnitOnTile.IsAttackableBy(_actor)) {
                _highlighter.DisplayMeleeIcon(_navGraph.CostToTile(tile));
            }
            else if (_navGraph.TilesInRange.Contains(tile) && tile != _actor.CurrentTile) {
                _highlighter.DisplayWalkIcon(_navGraph.CostToTile(tile));
                _highlighter.DrawPath(_navGraph.PathToTile(tile));
            }
            else {
                _highlighter.ClearIcon();
                _highlighter.ClearPath();
            }
        }

        public override void OnExit() {
            _highlighter.ClearAll();
        }
    }

    /// <summary>
    /// A unit is in the process of moving along the tilemap
    /// </summary>
    private class ExecuteMove : BattleState {
        const float CloseEnough = 0.01f;
        Actor _actor;
        Stack<TerrainTile> _nodes;

        public ExecuteMove(Actor unitToMove, Stack<TerrainTile> nodes, int moveCost) {
            _actor = unitToMove;
            _actor.AP -= moveCost;
            _nodes = nodes;
        }

        public override BattleState Update() {
            var targetPos = _nodes.Peek().SurfaceCenter + Vector3.up * _actor.ObjectHeight / 2;
            var currentPos = _actor.transform.position;
	    var disp = targetPos - currentPos;
	    var movement = Vector3.ClampMagnitude(disp, _actor.TileMapMoveSpeed * Time.deltaTime);
	    _actor.transform.position += movement;
	    // check new position and figure out if actor is close enough
            currentPos = _actor.transform.position;
            if (Vector3.Distance(currentPos, targetPos) < CloseEnough) { // reached tile
                var tile = _nodes.Pop();
                if (_nodes.Count == 0) { // reached destination
                    _actor.CurrentTile = tile;
                    return new PlayerReady(_actor);
                }
            }
	    return null;
        }
    }

    private class ExecuteAttack : BattleState {
        Actor _attacker;
        TerrainTile _targetTile;

	// TODO : Use targeted tile instead for AOE
        public ExecuteAttack(Actor attacker, TerrainTile target) {
            _attacker = attacker;
            _targetTile = target;
        }

        public override BattleState Update() {
            var targetUnit = _targetTile.UnitOnTile;
            var position = targetUnit.transform.position;
            var damageDealt = targetUnit.DealDamage(_attacker.Damage);
            var textManager = FindObjectOfType<BattleTextManager>();
            textManager.SpawnText(damageDealt, BattleTextManager.TextType.Damage, position);
            return new PlayerReady(_attacker);
        }
    }
}