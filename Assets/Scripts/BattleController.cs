using UnityEngine;
using System.Collections;
using System.Linq;

public class BattleController : MonoBehaviour {
    public float PhaseTime = 1.0f; // time in seconds to wait between phases
    private float _phaseTimer;     // times wait between phases
    Actor[] _allUnits;
    Actor _activeUnit;

    // Use this for initialization
    void Start() {
        _allUnits = GameObject.FindObjectsOfType<Actor>();
	_phaseTimer = PhaseTime;
    }

    // Update is called once per frame
    void Update() {
        if (_activeUnit) {

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
