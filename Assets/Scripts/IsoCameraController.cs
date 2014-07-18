using UnityEngine;
using System.Collections;

public class IsoCameraController : MonoBehaviour {
    public Vector3 MinPos;
    public Vector3 MaxPos;
    public Vector3 FocalPoint;
    public float MinFov = 5;
    public float MaxFov = 20;
    public float ScrollSpeed = 20f;
    public float ZoomSpeed = 200f;
    public float RotateSpeed = 100f;
    public float MouseDragSpeed = 0.25f;

    private Vector3 _dragStartMousePos;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
	// keyboard movement
        if (Input.GetKey(KeyCode.W) && transform.position.z < MaxPos.z) {
            transform.Translate(Vector3.up * Time.deltaTime * ScrollSpeed); // y
        }
        else if (Input.GetKey(KeyCode.S) && transform.position.z > MinPos.z) {
            transform.Translate(Vector3.down * Time.deltaTime * ScrollSpeed);
        }

        if (Input.GetKey(KeyCode.D) && transform.position.x < MaxPos.x) {
            transform.Translate(Vector3.right * Time.deltaTime * ScrollSpeed);
        }
        else if (Input.GetKey(KeyCode.A) && transform.position.x > MinPos.x) {
            transform.Translate(Vector3.left * Time.deltaTime * ScrollSpeed); // x
        }

	// mouse scroll zoom
        var scroll = -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * ZoomSpeed;
        if (scroll > 0 && camera.fieldOfView < MaxFov || scroll < 0 && camera.fieldOfView > MinFov) {
            camera.fieldOfView += scroll;
        }

	// rotation
        if (Input.GetKey(KeyCode.E)) {
            transform.RotateAround(FocalPoint, Vector3.up, RotateSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Q)) {
            transform.RotateAround(FocalPoint, Vector3.up, -RotateSpeed * Time.deltaTime);
        }

	// right click rotation
        if (Input.GetMouseButtonDown(1)) {
            _dragStartMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(1)) {
            var mouseMovement = Input.mousePosition - _dragStartMousePos;
            _dragStartMousePos = Input.mousePosition;
	    // mouse rotation about z
            transform.RotateAround(FocalPoint, Vector3.up, RotateSpeed * Time.deltaTime * mouseMovement.x * MouseDragSpeed);
        }
    }
}
