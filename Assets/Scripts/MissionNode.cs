﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionNode : MonoBehaviour {
	
	public MissionType type;
	public HelicopterType idealHelicopter;
	public string missionTitle;
	[TextArea]
	public string missionDescription;

	private bool missionAccepted;
	private bool canAcceptMission;
	private bool missionComplete;
	private bool missionFailed;
	private bool isExpandingBaseParticle;

	private MeshRenderer meshRenderer;
	private GameObject missionParticles;
	private GameObject baseParticles;
	private Vector3 initialBaseScale;
	private float baseExpandRate = 0.2f;
	private float maxBaseSize = 10;

	void Start () {
		meshRenderer = gameObject.GetComponent<MeshRenderer>();
		missionParticles = this.gameObject.transform.GetChild(0).gameObject;
		int baseParticleIndex = missionParticles.transform.childCount - 1;
		baseParticles = missionParticles.transform.GetChild(baseParticleIndex).gameObject;
		initialBaseScale = baseParticles.transform.localScale;
	}

	// Update is called once per frame
	void Update () {
		bool playerHasAcceptedMission = Input.GetAxis("Jump") > 0f;
		if (canAcceptMission && !missionAccepted && playerHasAcceptedMission) {
			missionAccepted = true;
			meshRenderer.enabled = false;
			isExpandingBaseParticle = true;
			MissionController.SetActiveMission(this);
		}
		if (isExpandingBaseParticle) {
			ExpandBaseParticle();
		}
		// TESTING ONLY -- remove later
		if (missionAccepted && !MissionController.showingFailedMessage && Input.GetKeyDown(KeyCode.C)) {
			MissionController.HandleMissionComplete();
		}
        if (missionAccepted && !MissionController.showingFailedMessage && Input.GetKeyDown(KeyCode.X)){
            MissionController.HandleMissionFailed();
        }
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			canAcceptMission = true;
			MissionController.ShowMissionAcceptPanel(missionTitle, missionDescription, idealHelicopter.ToString());
		}
	}
	

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			canAcceptMission = false;
			MissionController.HideMissionAcceptPanel();
		}
	}

    void ExpandBaseParticle() {
		if (baseParticles.transform.localScale.x < maxBaseSize) {
			baseParticles.transform.localScale += new Vector3(baseExpandRate, baseExpandRate, 0f);
		} else {
			isExpandingBaseParticle = false;
			HideMissionParticles();
		}
    }

	void HideMissionParticles() {
		missionParticles.SetActive(false);
        baseParticles.transform.localScale = initialBaseScale;
	}

    void ShowMissionParticles() {
        missionParticles.SetActive(true);
    }

	public void HandleMissionComplete() {
		ShowMissionParticles();
		missionAccepted = false;
		missionComplete = true;
	}

	public void HandleMissionFailed() {
        ShowMissionParticles();
		missionAccepted = false;
		missionFailed = true;
	}
}

public enum MissionType {
	Transport,
	Rescue,
	Report,
	Delivery
}