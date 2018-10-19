using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionController : MonoBehaviour {
    private static MissionController instance;

    private GameObject fireworkParticles;
    private GameObject playerHelicopter;

    private GameObject missionAcceptPanel;
    private Text missionAcceptTitle;
    private Text missionAcceptDescription;
    private Text idealHelicopterText;

    private GameObject missionStatusPanel;
    private Text missionStatusType;
    private Text missionStatusTitle;

    private GameObject missionFailedPanel;
    private Text missionFailedText;

    private GameObject currentMissionPanel;
    private Text currentMissionDescription;
	
    private MissionNode[] allMissionNodes;
	private MissionNode activeMission;

    private Color missionStartColor = new Color32(255, 255, 66, 255);
    private Color missionCompleteColor = new Color32(66, 255, 106, 255);
    private Color missionFailedColor = new Color32(255, 79, 66, 255);

    private float secondsToShowMissionStatus = 3.0f;
    public static bool showingFailedMessage = false;
    private bool showingFireworks = true;

    void Awake () {
        if (!instance) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

	void Start () {
		instance.SetMissionPanelObjects();
        instance.GetFireworkParticles();
		instance.GetMissionNodes();
		instance.GetPlayerHelicopter();
        HideMissionAcceptPanel();
        HideMissionStatusPanel();
        HideCurrentMissionPanel();
        HideMissionFailedPanel();
        HideFireworkParticles();
	}

    void Update () {
        if (showingFailedMessage && Input.GetKeyDown(KeyCode.C)) {
            instance.StartCoroutine(instance.RetryMission());
        }
        if (showingFailedMessage && Input.GetKeyDown(KeyCode.X)) {
            instance.AbortMission();
        }
        if (showingFireworks) {
            SetFireworkPosition();
        }
    }

	void SetMissionPanelObjects () {
        instance.missionAcceptPanel = GameObject.Find("MissionAcceptPanel");
        instance.missionAcceptTitle = GameObject.Find("MissionTitle").GetComponent<Text>();
        instance.missionAcceptDescription = GameObject.Find("MissionDescription").GetComponent<Text>();
        instance.idealHelicopterText = GameObject.Find("IdealHelicopterText").GetComponent<Text>();

        instance.missionStatusPanel = GameObject.Find("MissionStatusPanel");
        instance.missionStatusType = GameObject.Find("MissionTypeText").GetComponent<Text>();
        instance.missionStatusTitle = GameObject.Find("MissionTitleText").GetComponent<Text>();

        instance.missionFailedPanel = GameObject.Find("MissionFailedPanel");
        instance.missionFailedText = GameObject.Find("MissionFailedText").GetComponent<Text>();

        instance.currentMissionPanel = GameObject.Find("CurrentMissionPanel");
        instance.currentMissionDescription = GameObject.Find("CurrentMissionDescription").GetComponent<Text>();
	}

    void GetMissionNodes() {
        instance.allMissionNodes = GameObject.FindObjectsOfType<MissionNode>();
    }

    void GetFireworkParticles() {
        instance.fireworkParticles = GameObject.Find("FireworkParticles");
    }

    void GetPlayerHelicopter() {
        HeloController[] heloControllers = GameObject.FindObjectsOfType<HeloController>();
        if (heloControllers.Length > 0) {
            instance.playerHelicopter = heloControllers[0].gameObject;
        }
    }

    void HideFireworkParticles() {
        showingFireworks = false;
        instance.fireworkParticles.SetActive(showingFireworks);
    }

    void ShowFireworkParticles() {
        showingFireworks = true;
        instance.fireworkParticles.SetActive(showingFireworks);
    }

    void SetFireworkPosition() {
        if (instance.playerHelicopter) {
            Vector3 helicopterPosition = instance.playerHelicopter.transform.position;
            Vector3 fireWorksPosition = new Vector3(helicopterPosition.x, helicopterPosition.y - 0.7f, helicopterPosition.z + 2f);
            instance.fireworkParticles.transform.position = fireWorksPosition;
        }
    }

    public static void HideMissionAcceptPanel () {
        instance.missionAcceptPanel.SetActive(false);
	}

	public static void ShowMissionAcceptPanel (string missionTitle, string missionDescription, string idealHelicopterType) {
        instance.missionAcceptTitle.text = missionTitle;
        instance.missionAcceptDescription.text = missionDescription;

        instance.idealHelicopterText.text = "";
        if (HeloController.instance.helicopterType.ToString() != idealHelicopterType) {
            instance.idealHelicopterText.text = "A " + idealHelicopterType.ToLower() + " helicopter would be best suited for this mission.";
        }
        instance.missionAcceptPanel.SetActive(true);
	}

    public static void HideMissionStatusPanel() {
        instance.missionStatusPanel.SetActive(false);
    }

    public static void ShowMissionStatusPanel (string typeText, Color32 typeColor) {
        instance.missionStatusPanel.SetActive(true);
        instance.missionStatusType.text = instance.activeMission.type.ToString() + " " + typeText;
        instance.missionStatusType.color = typeColor;
        instance.missionStatusTitle.text = instance.activeMission.missionTitle;
    }

    public static void HideCurrentMissionPanel () {
        instance.currentMissionPanel.SetActive(false);
    }

    public static void ShowCurrentMissionPanel() {
        instance.currentMissionPanel.SetActive(true);
        instance.currentMissionDescription.text = instance.activeMission.missionDescription;
    }

    public static void HideMissionFailedPanel() {
        instance.missionFailedPanel.SetActive(false);
        showingFailedMessage = false;
    }

    public static void ShowMissionFailedPanel(string missionFailedText) {
        instance.missionFailedPanel.SetActive(true);
        instance.missionFailedText.text = missionFailedText;
    }

	public static void SetActiveMission (MissionNode mission) {
		instance.activeMission = mission;
		HideMissionAcceptPanel();
        instance.StartCoroutine(instance.HandleMissionStart());
        instance.DisableOtherMissionNodes();
	}

    public static void HandleMissionComplete() {
        HideCurrentMissionPanel();
        instance.StartCoroutine(instance.ShowMissionCompleteMessage());
    }

    public static void HandleMissionFailed() {
        HideCurrentMissionPanel();
        instance.ShowMissionFailedMessage();
    }

    void DisableOtherMissionNodes() {
        for (int i = 0; i < instance.allMissionNodes.Length; i++) {
            if (instance.allMissionNodes[i] != instance.activeMission) {
                instance.allMissionNodes[i].gameObject.SetActive(false);
            }
        }
    }

    void EnableAllMissionNodes() {
        for (int i = 0; i < instance.allMissionNodes.Length; i++) {
            instance.allMissionNodes[i].gameObject.SetActive(true);
        }
    }

    IEnumerator HandleMissionStart() {
        ShowMissionStatusPanel("Mission Start", instance.missionStartColor);
        yield return new WaitForSeconds(secondsToShowMissionStatus);
        HideMissionStatusPanel();
        ShowCurrentMissionPanel();
    }

    IEnumerator ShowMissionCompleteMessage() {
        ShowMissionStatusPanel("Mission Complete", instance.missionCompleteColor);
        ShowFireworkParticles();
        yield return new WaitForSeconds(secondsToShowMissionStatus);
        HideMissionStatusPanel();
        instance.EnableAllMissionNodes();
        instance.activeMission.HandleMissionComplete();
        yield return new WaitForSeconds(5f);
        HideFireworkParticles();
    }

    void ShowMissionFailedMessage() {
        ShowMissionStatusPanel("Mission Failed", instance.missionFailedColor);
        ShowMissionFailedPanel("You dropped a puppy! D:<");
        showingFailedMessage = true;
    }

    IEnumerator RetryMission() {
        yield return new WaitForSeconds(0.1f);
        HideMissionFailedPanel();
        instance.StartCoroutine(instance.HandleMissionStart());
    }

    void AbortMission() {
        HideMissionFailedPanel();
        HideMissionStatusPanel();
        instance.EnableAllMissionNodes();
        instance.activeMission.HandleMissionFailed();
    }
}
