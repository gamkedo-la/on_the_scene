using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionController : MonoBehaviour {
    private static MissionController instance;

    private GameObject missionAcceptPanel;
    private Text missionAcceptTitle;
    private Text missionAcceptDescription;

    private GameObject missionStartPanel;
    private Text missionStartType;
    private Text missionStartTitle;

    private GameObject currentMissionPanel;
    private Text currentMissionDescription;
	
    private MissionNode[] allMissionNodes;
	private MissionNode activeMission;

    public float secondsToShowMissionStart = 3.0f;

    void Awake () {
        if (!instance) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

	void Start () {
		instance.SetMissionPanelObjects();
		instance.GetMissionNodes();
        HideMissionAcceptPanel();
        HideMissionStartPanel();
        HideCurrentMissionPanel();
	}

	void SetMissionPanelObjects () {
        instance.missionAcceptPanel = GameObject.Find("MissionAcceptPanel");
        instance.missionAcceptTitle = GameObject.Find("MissionTitle").GetComponent<Text>();
        instance.missionAcceptDescription = GameObject.Find("MissionDescription").GetComponent<Text>();

        instance.missionStartPanel = GameObject.Find("MissionStartPanel");
        instance.missionStartType = GameObject.Find("MissionStartType").GetComponent<Text>();
        instance.missionStartTitle = GameObject.Find("MissionStartTitle").GetComponent<Text>();

        instance.currentMissionPanel = GameObject.Find("CurrentMissionPanel");
        instance.currentMissionDescription = GameObject.Find("CurrentMissionDescription").GetComponent<Text>();
	}

    void GetMissionNodes() {
        instance.allMissionNodes = GameObject.FindObjectsOfType<MissionNode>();
    }

    public static void HideMissionAcceptPanel () {
        instance.missionAcceptPanel.SetActive(false);
	}

	public static void ShowMissionAcceptPanel (string missionTitle, string missionDescription) {
        instance.missionAcceptTitle.text = missionTitle;
        instance.missionAcceptDescription.text = missionDescription;
        instance.missionAcceptPanel.SetActive(true);
	}

    public static void HideMissionStartPanel() {
        instance.missionStartPanel.SetActive(false);
    }

    public static void ShowMissionStartPanel () {
        instance.missionStartPanel.SetActive(true);
        instance.missionStartType.text = instance.activeMission.type.ToString() + " Mission Started";
        instance.missionStartTitle.text = instance.activeMission.missionTitle;
    }

    public static void HideCurrentMissionPanel () {
        instance.currentMissionPanel.SetActive(false);
    }

    public static void ShowCurrentMissionPanel() {
        instance.currentMissionPanel.SetActive(true);
        instance.currentMissionDescription.text = instance.activeMission.missionDescription;
    }

	public static void SetActiveMission (MissionNode mission) {
		instance.activeMission = mission;
		HideMissionAcceptPanel();
        instance.StartCoroutine(instance.HandleMissionStart());
        instance.DisableOtherMissionNodes();
	}

    public static void CompleteMission() {
        instance.activeMission.HandleMissionComplete();
        instance.EnableAllMissionNodes();
    }

    public static void HandleMissionFailed() {
        instance.activeMission.HandleMissionFailed();
        instance.EnableAllMissionNodes();
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
        ShowMissionStartPanel();
        yield return new WaitForSeconds(secondsToShowMissionStart);
        HideMissionStartPanel();
        ShowCurrentMissionPanel();
    }
}
