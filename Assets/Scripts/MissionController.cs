using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionController : MonoBehaviour {
    private static MissionController instance;

    private GameObject missionAcceptPanel;
    private Text missionAcceptTitle;
    private Text missionAcceptDescription;
	
	private MissionNode activeMission;

    void Awake () {
        if (!instance) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

	void Start () {
		instance.SetMissionAcceptPanelObjects();
        HideMissionAcceptPanel();
	}

	void SetMissionAcceptPanelObjects () {
        instance.missionAcceptPanel = GameObject.Find("MissionAcceptPanel");
        instance.missionAcceptTitle = GameObject.Find("MissionTitle").GetComponent<Text>();
        instance.missionAcceptDescription = GameObject.Find("MissionDescription").GetComponent<Text>();
	}

    public static void HideMissionAcceptPanel () {
        instance.missionAcceptPanel.SetActive(false);
	}

	public static void ShowMissionAcceptPanel (string missionTitle, string missionDescription) {
        instance.missionAcceptTitle.text = missionTitle;
        instance.missionAcceptDescription.text = missionDescription;
        instance.missionAcceptPanel.SetActive(true);
	}

	public static void SetActiveMission (MissionNode mission) {
		instance.activeMission = mission;
		HideMissionAcceptPanel();
	}
}
