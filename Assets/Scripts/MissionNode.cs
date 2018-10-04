﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionNode : MonoBehaviour {
	public MissionType type;
	public string missionTitle;
	[TextArea]
	public string missionDescription;

	private bool missionAccepted;
	private bool canAcceptMission;
	private bool missionComplete;
    private MeshRenderer meshRenderer;

    void Start () {
		meshRenderer = gameObject.GetComponent<MeshRenderer>();
	}

	// Update is called once per frame
	void Update () {
		bool playerHasAcceptedMission = Input.GetKeyDown(KeyCode.Space);
		if (canAcceptMission && !missionAccepted && playerHasAcceptedMission) {
			missionAccepted = true;
			meshRenderer.enabled = false;
			Debug.Log("Player has accepted the mission!");
			Debug.Log("MISSION: " + missionTitle);
			Debug.Log("GOAL: " + missionDescription);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			Debug.Log("Player touched the node!");
			canAcceptMission = true;
		}
	}

	void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("Player stopped touching the node!");
            canAcceptMission = false;
        }
	}
}

public enum MissionType {
	Delivery,
	Rescue,
	Report
}
