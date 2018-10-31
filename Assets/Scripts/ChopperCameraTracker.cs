using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChopperCameraTracker : MonoBehaviour {

    int targetToWatch = 0;
    public Text timeRemainingDisplay;
    public Trackables[] targets;
    public float timeForStoryLock = 20.0f;
    public Camera targetCamera;

    private float numberOfSecondsRemaining = 20.0f;
	// Use this for initialization
	void Start () {
        numberOfSecondsRemaining = timeForStoryLock;
	}
	
	// Update is called once per frame
	void Update () {
        if (targetToWatch >= targets.Length) {
            if (targetCamera.enabled) {
                targetCamera.enabled = false;
            }
            return;
        }
        if (!targetCamera.enabled) {
            targetCamera.enabled = true;
        }
        transform.LookAt(targets[targetToWatch].transform);
        RaycastHit rhInfo;
        if (Physics.Raycast(transform.position, transform.forward, out rhInfo)) {
            if (rhInfo.transform.gameObject.layer == LayerMask.NameToLayer("Subject")) {
                numberOfSecondsRemaining -= Time.deltaTime;
                if (numberOfSecondsRemaining < 0.0f) {
                    numberOfSecondsRemaining = timeForStoryLock;
                    targetToWatch += 1;
                }
            }

        }
        timeRemainingDisplay.text = "" + numberOfSecondsRemaining;
	}
}
