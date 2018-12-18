using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayWhenHelicopterIsNear : MonoBehaviour {
    public Quaternion originalRotation;
    float rotationResetSpeed = 1.0f;
    float distanceCheck = 5.0f;

    void Start () {
        originalRotation = transform.rotation;
    }
	
	void Update () {
        var treeWobbleScale = Vector3.Distance(HeliController.instance.transform.position, transform.position);
        if (treeWobbleScale < distanceCheck) {
            Debug.Log("swaying");
            transform.rotation = Quaternion.AngleAxis(Random.Range(-4.0f, 4.0f) * (distanceCheck / treeWobbleScale),
                Vector3.up)
            * Quaternion.AngleAxis(Random.Range(-4.0f, 4.0f) * (distanceCheck / treeWobbleScale),
                Vector3.right);
        } else {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, 
                Time.deltaTime * rotationResetSpeed);
        }
    }
}
