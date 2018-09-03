using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    public float cameraElevation = 5.0f;
    public float cameraDistance = 15.0f;
    public float cameraLookAhead = 10.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        Vector3 flattenedOrientation = HeloController.instance.transform.forward * -1.0f;
        flattenedOrientation.y = 0.0f;
        flattenedOrientation.Normalize();
        transform.position = HeloController.instance.transform.position + flattenedOrientation * cameraDistance + Vector3.up * cameraElevation;
        transform.LookAt(HeloController.instance.transform.position + HeloController.instance.transform.forward * cameraLookAhead);
    }
}
