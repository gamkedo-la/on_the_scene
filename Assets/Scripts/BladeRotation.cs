using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeRotation : MonoBehaviour {
    public float direction = 1.0f;
    public Vector3 spinAxis = Vector3.up;
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, 480.0f * Time.deltaTime * direction);
	}
}
