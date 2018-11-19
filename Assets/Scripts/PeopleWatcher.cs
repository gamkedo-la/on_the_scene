using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleWatcher : MonoBehaviour {

    private Transform cameraTransform;
	// Use this for initialization
	void Start () {
        cameraTransform = Camera.main.transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 flatPt = cameraTransform.position;
        flatPt.y = transform.position.y;
        transform.LookAt(flatPt);
        transform.Rotate(Vector3.up, 180.0f);
	}
}
