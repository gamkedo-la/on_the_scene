using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayWhenHelicopterIsNear : MonoBehaviour {
    public Quaternion originalRotation;
    float rotationResetSpeed = 1.0f;
    float distanceCheck = 10.0f;

    void Start () {
        originalRotation = transform.rotation;
    }
	
	void Update () {
        var treeWobbleScale = Vector3.Distance(HeliController.instance.transform.position, transform.position);
        if (treeWobbleScale < distanceCheck) {
            Debug.Log("swaying");
            transform.rotation = Quaternion.FromToRotation(Vector3.up,
                HeliController.instance.transform.forward * treeWobbleScale)
                * Quaternion.AngleAxis(treeWobbleScale,
                HeliController.instance.transform.forward);
        } else {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, 
                Time.deltaTime * rotationResetSpeed);
        }
    }
}
