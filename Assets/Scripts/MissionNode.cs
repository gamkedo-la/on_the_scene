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
	private GameObject missionParticles;
	private GameObject baseParticles;
	private Vector3 baseScale;

	void Start () {
		meshRenderer = gameObject.GetComponent<MeshRenderer>();
		missionParticles = this.gameObject.transform.GetChild(0).gameObject;
		int baseParticleIndex = missionParticles.transform.childCount - 1;
		baseParticles = missionParticles.transform.GetChild(baseParticleIndex).gameObject;
		baseScale = baseParticles.transform.localScale;
	}

	// Update is called once per frame
	void Update () {
		bool playerHasAcceptedMission = Input.GetAxis("Jump") > 0f;
		if (canAcceptMission && !missionAccepted && playerHasAcceptedMission) {
			missionAccepted = true;
			meshRenderer.enabled = false;
			// missionParticles.SetActive(false);
			StartCoroutine(ExpandBaseParticle());
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

    IEnumerator ExpandBaseParticle() {
		baseParticles.transform.localScale += new Vector3(1f, 1f, 0f);
		yield return new WaitForSeconds(0.10f);
		baseParticles.transform.localScale += new Vector3(1f, 1f, 0f);
		yield return new WaitForSeconds(0.10f);
		baseParticles.transform.localScale += new Vector3(1f, 1f, 0f);
		yield return new WaitForSeconds(0.10f);
		baseParticles.transform.localScale += new Vector3(1f, 1f, 0f);
		yield return new WaitForSeconds(0.10f);
		baseParticles.transform.localScale = baseScale;
		missionParticles.SetActive(false);
    }

	public void HandleMissionComplete() {

	}

	public void HandleMissionFailed() {

	}
}

public enum MissionType {
	Delivery,
	Rescue,
	Report
}
