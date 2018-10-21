using System.Collections;
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

	private float timeElapsed = 0f;
	private float bestTime;
	private string missionTimeKey;

	void Start () {
		meshRenderer = gameObject.GetComponent<MeshRenderer>();
		missionParticles = this.gameObject.transform.GetChild(0).gameObject;

		int baseParticleIndex = missionParticles.transform.childCount - 1;
		baseParticles = missionParticles.transform.GetChild(baseParticleIndex).gameObject;
		initialBaseScale = baseParticles.transform.localScale;

		missionTimeKey = "BestTime" + missionTitle;
		bestTime = PlayerPrefs.GetFloat(missionTimeKey, 0);
		Debug.Log("Best time for " + missionTitle + " is " + bestTime);
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
		if (missionAccepted) {
			timeElapsed += Time.deltaTime;
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

	public void SetTimeElapsed() {
		timeElapsed = 0f;
	}

	void HideMissionParticles() {
		missionParticles.SetActive(false);
        baseParticles.transform.localScale = initialBaseScale;
	}

    void ShowMissionParticles() {
        missionParticles.SetActive(true);
    }

	string GetFormattedTime(float timeToFormat) {
        int seconds = (int)(timeToFormat % 60);
        int minutes = (int)(timeToFormat / 60) % 60;
        return string.Format("{0:0}m {1:00}s", minutes, seconds);
	}

	public void HandleMissionComplete() {
		ShowMissionParticles();
		missionAccepted = false;
		missionComplete = true;
		Debug.Log("timeElapsed " + timeElapsed);

		string timeElapsedString = GetFormattedTime(timeElapsed);
		string bestTimeString = GetFormattedTime(bestTime);

		Debug.Log("Formatted time elapsed: " + timeElapsedString);
		Debug.Log("Formatted best time: " + bestTimeString);
	
		if (timeElapsed < bestTime) {
			PlayerPrefs.SetFloat(missionTimeKey, timeElapsed);
			PlayerPrefs.Save();
			bestTime = timeElapsed;
		}
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