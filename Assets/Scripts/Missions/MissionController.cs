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
    private Text bestTimeText;

    private GameObject missionStatusPanel;
    private Text missionStatusType;
    private Text missionStatusTitle;

    private GameObject missionFailedPanel;
    private Text missionFailedText;

    private GameObject timeToCompletePanel;
    private Text completedTimeText;
    private Text bestTimeCompletedText;
    private Text newBestTimeText;

    private GameObject currentMissionPanel;
    private Text currentMissionDescription;

    public MissionNode[] allMissionNodes;
	private MissionNode activeMission;
    public List<GameObject> missionObjectiveNodes;

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
        HideTimeToCompletePanel();
        missionObjectiveNodes = new List<GameObject>();

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

    public static Transform GetNearestObjective()
    {
		if ( instance == null )
			return null;

        if (instance.missionObjectiveNodes.Count == 0)
        {
            return null;
        }
        else
        {
            return instance.missionObjectiveNodes[0].transform;
        }

    }

    void SetMissionPanelObjects () {
        instance.missionAcceptPanel = GameObject.Find("MissionAcceptPanel");
        instance.missionAcceptTitle = GameObject.Find("MissionTitle").GetComponent<Text>();
        instance.bestTimeText = GameObject.Find("BestTime").GetComponent<Text>();
        instance.missionAcceptDescription = GameObject.Find("MissionDescription").GetComponent<Text>();
        instance.idealHelicopterText = GameObject.Find("IdealHelicopterText").GetComponent<Text>();

        instance.missionStatusPanel = GameObject.Find("MissionStatusPanel");
        instance.missionStatusType = GameObject.Find("MissionTypeText").GetComponent<Text>();
        instance.missionStatusTitle = GameObject.Find("MissionTitleText").GetComponent<Text>();

        instance.missionFailedPanel = GameObject.Find("MissionFailedPanel");
        instance.missionFailedText = GameObject.Find("MissionFailedText").GetComponent<Text>();

        instance.timeToCompletePanel = GameObject.Find("TimeToCompletePanel");
        instance.completedTimeText = GameObject.Find("CompletedTimeText").GetComponent<Text>();
        instance.bestTimeCompletedText = GameObject.Find("BestTimeCompleted").GetComponent<Text>();
        instance.newBestTimeText = GameObject.Find("NewBestTime").GetComponent<Text>();

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
        HeliController[] heloControllers = GameObject.FindObjectsOfType<HeliController>();
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

	public static void ShowMissionAcceptPanel (string missionTitle, string bestTime, string missionDescription, string idealHelicopterType) {
        instance.missionAcceptTitle.text = missionTitle;
        instance.missionAcceptDescription.text = missionDescription;

        instance.idealHelicopterText.text = "";
        if (HeliController.instance.helicopterType.ToString() != idealHelicopterType) {
            instance.idealHelicopterText.text = "A " + idealHelicopterType.ToLower() + " helicopter would be best suited for this mission.";
        }
        instance.bestTimeText.text = "Best Time: " + bestTime;
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

    public static void HideTimeToCompletePanel() {
        instance.timeToCompletePanel.SetActive(false);
    }

    public static void ShowTimeToCompletePanel() {
        instance.timeToCompletePanel.SetActive(true);
        instance.completedTimeText.text = "Time to Complete:\n" + instance.activeMission.GetCompletedTimeString();
        instance.bestTimeCompletedText.text = "Previous Best:\n" + instance.activeMission.GetBestTimeString();
        instance.newBestTimeText.text = "";
        if (instance.activeMission.HasNewBestTime()) {
            instance.newBestTimeText.text = "New Best Time!";
        }
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
        instance.missionObjectiveNodes = mission.GetObjectives();
        Debug.Log("objective nodes length: " + instance.missionObjectiveNodes.Count);
        Debug.Log("GetObjectives results: " + mission.GetObjectives().Count);
        HideMissionAcceptPanel();
        instance.StartCoroutine(instance.HandleMissionStart());
        instance.DisableOtherMissionNodes();
	}

    public static void HandleMissionComplete(string missionCompletedMessage = "You did it!") {
        HideCurrentMissionPanel();
        instance.StartCoroutine(instance.ShowMissionCompleteMessage(missionCompletedMessage));
    }

    public static void HandleMissionFailed(string missionFailedMessage = "You failed." ) {
        HideCurrentMissionPanel();
        instance.StartCoroutine(instance.ShowMissionFailedMessage(missionFailedMessage));
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
        instance.activeMission.SetTimeElapsed();
        ShowMissionStatusPanel("Mission Start", instance.missionStartColor);
        yield return new WaitForSeconds(secondsToShowMissionStatus);
        HideMissionStatusPanel();
        ShowCurrentMissionPanel();
    }

    IEnumerator ShowMissionCompleteMessage(string missionCompleteMessage) {
        ShowMissionStatusPanel(missionCompleteMessage, instance.missionCompleteColor);
        ShowFireworkParticles();

        instance.activeMission.HandleMissionComplete();

        ShowTimeToCompletePanel();

        instance.activeMission.CheckTimeElapsed();

        yield return new WaitForSeconds(secondsToShowMissionStatus);
        HideMissionStatusPanel();
        HideTimeToCompletePanel();

        instance.EnableAllMissionNodes();
        yield return new WaitForSeconds(5f);
        HideFireworkParticles();
    }

    IEnumerator ShowMissionFailedMessage(string failedMessage="You failed") {
        ShowMissionStatusPanel("Mission Failed", instance.missionFailedColor);
        ShowMissionFailedPanel(failedMessage);
        yield return new WaitForSeconds(0.25f);
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

    public static void ObjectiveReportingComplete(GameObject objectiveNode)
    {
        int index = instance.missionObjectiveNodes.IndexOf(objectiveNode);
        if (index != -1)
        {
            instance.missionObjectiveNodes.RemoveAt(index);
        }
        else
        {
            Debug.Log("reporting node doesn't exit? Node name: " + objectiveNode.name);
        }
    }
}
