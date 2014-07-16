using UnityEngine;
using System.Collections;
using System.Linq;

public class BattleController : MonoBehaviour {
    public int PhaseTimeMs = 30;
    private float _phaseTimer;
    Actor[] _allUnits;
    Actor _activeUnit;

    // Use this for initialization
    void Start() {
        _allUnits = GameObject.FindObjectsOfType<Actor>();
    }

    // Update is called once per frame
    void Update() {
        if (_activeUnit) {
            Debug.Log("Unit Ready: " + _activeUnit.name);
        }
        else {
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
            _phaseTimer = PhaseTimeMs;
            foreach (var unit in _allUnits) {
                unit.PassPhase();
            }
        }
        return null;
    }
}
