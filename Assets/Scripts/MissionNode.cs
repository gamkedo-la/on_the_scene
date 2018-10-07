using System.Collections;
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
		// bool playerHasAcceptedMission = Input.GetKeyDown(KeyCode.Space);
		bool playerHasAcceptedMission = Input.GetAxis("Jump") > 0f;
		if (canAcceptMission && !missionAccepted && playerHasAcceptedMission) {
			missionAccepted = true;
			meshRenderer.enabled = false;
			MissionController.SetActiveMission(this);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			canAcceptMission = true;
			MissionController.ShowMissionAcceptPanel(missionTitle, missionDescription);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			canAcceptMission = false;
			MissionController.HideMissionAcceptPanel();
		}
	}
}

public enum MissionType {
	Delivery,
	Rescue,
	Report
}
