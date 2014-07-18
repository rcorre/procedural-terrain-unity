using UnityEngine;
using System.Collections;

public class TextPopup : MonoBehaviour {
    public float ScrollSpeed = 0.05f;
    public float Duration = 2.0f;

    // Use this for initialization
    void Start() {
        Destroy(gameObject, Duration); // set up delayed destruction
    }

    // Update is called once per frame
    void Update() {
        transform.position += Vector3.up * ScrollSpeed * Time.deltaTime;
	var alpha = guiText.material.color.a ;
        if (alpha > 0) {
	    Color c = guiText.material.color;
	    c.a = alpha - Time.deltaTime / Duration;
	    guiText.material.color = c;
        }
    }
}
