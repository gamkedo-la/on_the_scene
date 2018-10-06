using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionNode : MonoBehaviour {
	public MissionType type;
	public string missionTitle;
	[TextArea]
	public string missionDescription;

	private bool missionAccepted;
	private bool canAcceptMission;
	private bool missionComplete;
	private MeshRenderer meshRenderer;
	private GameObject missionAcceptPanel;
	private Text missionAcceptTitle;
	private Text missionAcceptDescription;

	void Start () {
		meshRenderer = gameObject.GetComponent<MeshRenderer>();
		missionAcceptPanel = GameObject.Find("MissionAcceptPanel");
		missionAcceptTitle = GameObject.Find("MissionTitle").GetComponent<Text>();
		missionAcceptDescription = GameObject.Find("MissionDescription").GetComponent<Text>();
		missionAcceptPanel.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		bool playerHasAcceptedMission = Input.GetKeyDown(KeyCode.Space);
		if (canAcceptMission && !missionAccepted && playerHasAcceptedMission) {
			missionAccepted = true;
			meshRenderer.enabled = false;
			missionAcceptPanel.SetActive(false);
			Debug.Log("Player has accepted the mission!");
			Debug.Log("MISSION: " + missionTitle);
			Debug.Log("GOAL: " + missionDescription);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			Debug.Log("Player touched the node!");
			canAcceptMission = true;
			missionAcceptPanel.SetActive(true);
			missionAcceptTitle.text = missionTitle;
			missionAcceptDescription.text = missionDescription;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			Debug.Log("Player stopped touching the node!");
			canAcceptMission = false;
			missionAcceptPanel.SetActive(false);
		}
	}
}

public enum MissionType {
	Delivery,
	Rescue,
	Report
}
