using UnityEngine;
using System.Collections;

public class IsoCameraController : MonoBehaviour {
    public Vector3 MinPos;
    public Vector3 MaxPos;
    public Vector3 FocalPoint;
    public float ScrollSpeed = 10f;
    public float ZoomSpeed = 10f;
    public float RotateSpeed = 1f;

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

	// mouse scroll
        var scroll = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * ZoomSpeed;
        if (scroll < 0 && transform.position.y > MinPos.y) {
            transform.Translate(Vector3.forward * scroll); //z
        }
        else if (scroll > 0 && transform.position.y < MaxPos.y) {
            transform.Translate(Vector3.forward * scroll); //z
        }

	// rotation
        if (Input.GetKey(KeyCode.E)) {
            transform.RotateAround(FocalPoint, Vector3.up, RotateSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Q)) {
            transform.RotateAround(FocalPoint, Vector3.up, -RotateSpeed * Time.deltaTime);
        }
    }
}
