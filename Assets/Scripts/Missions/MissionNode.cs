using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionNode : MonoBehaviour
{

    public MissionType type;
    public HelicopterType idealHelicopter;
    public List<GameObject> missionObjectives = new List<GameObject>();
    public string missionTitle;
    public float missionMaxTime = 300.0f;
    [TextArea]
    public string missionDescription;
    [TextArea]
    public string missionFailedMessage = "Mission Failed";
    [TextArea]
    public string missionCompleteMessage = "Mission Completed!";

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

    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        missionParticles = this.gameObject.transform.GetChild(0).gameObject;

        int baseParticleIndex = missionParticles.transform.childCount - 1;
        baseParticles = missionParticles.transform.GetChild(baseParticleIndex).gameObject;
        initialBaseScale = baseParticles.transform.localScale;

        missionTimeKey = "BestTime" + missionTitle;
        bestTime = PlayerPrefs.GetFloat(missionTimeKey, 0);
    }

    // Update is called once per frame
    void Update()
    {
        bool playerHasAcceptedMission = Input.GetAxis("Jump") > 0f;
        if (canAcceptMission && !missionAccepted && playerHasAcceptedMission)
        {
            missionAccepted = true;
            meshRenderer.enabled = false;
            isExpandingBaseParticle = true;
            MissionController.SetActiveMission(this);
        }
        if (isExpandingBaseParticle)
        {
            ExpandBaseParticle();
        }
        // TESTING ONLY -- remove later
        if (missionAccepted && !MissionController.showingFailedMessage && Input.GetKeyDown(KeyCode.C))
        {
            MissionController.HandleMissionComplete();
        }
        if (missionAccepted && !MissionController.showingFailedMessage && Input.GetKeyDown(KeyCode.X))
        {
            MissionController.HandleMissionFailed(missionFailedMessage);
        }
        if (missionAccepted)
        {
            timeElapsed += Time.deltaTime;
        }
        if (missionAccepted && missionMaxTime <= timeElapsed)
        {
            MissionController.HandleMissionFailed(missionFailedMessage);
        }
        if (missionAccepted && CheckAcitveObjectives())
        {
            MissionController.HandleMissionComplete(missionCompleteMessage);
        }
    }

    private bool CheckAcitveObjectives()
    {
        for (int o = 0; o < missionObjectives.Count; o++)
        {
            if (missionObjectives[o].activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        HeliController temp = other.gameObject.GetComponentInChildren<HeliController>();
        if (temp != null)
        {
            canAcceptMission = true;
            string bestTimeString = GetBestTimeString();
            MissionController.ShowMissionAcceptPanel(missionTitle, bestTimeString, missionDescription, idealHelicopter.ToString());
        }
    }

    void OnTriggerExit(Collider other)
    {
        HeliController temp = other.gameObject.GetComponentInChildren<HeliController>();
        if (temp != null)
        {
            canAcceptMission = false;
            MissionController.HideMissionAcceptPanel();
        }
    }

    public List<GameObject> GetObjectives()
    {

        Debug.Log("I got called and my mission objectives are: " + missionObjectives);
        return missionObjectives;
    }

    void ExpandBaseParticle()
    {
        if (baseParticles.transform.localScale.x < maxBaseSize)
        {
            baseParticles.transform.localScale += new Vector3(baseExpandRate, baseExpandRate, 0f);
        }
        else
        {
            isExpandingBaseParticle = false;
            HideMissionParticles();
        }
    }

    public void SetTimeElapsed()
    {
        timeElapsed = 0f;
    }

    void HideMissionParticles()
    {
        missionParticles.SetActive(false);
        baseParticles.transform.localScale = initialBaseScale;
    }

    void ShowMissionParticles()
    {
        missionParticles.SetActive(true);
    }

    string GetFormattedTime(float timeToFormat)
    {
        int seconds = (int)(timeToFormat % 60);
        int minutes = (int)(timeToFormat / 60) % 60;
        if (seconds == 0 && minutes == 0)
        {
            return "--m --s";
        }
        return string.Format("{0:0}m {1:00}s", minutes, seconds);
    }

    public string GetBestTimeString()
    {
        return GetFormattedTime(bestTime);
    }

    public string GetCompletedTimeString()
    {
        return GetFormattedTime(timeElapsed);
    }

    public bool HasNewBestTime()
    {
        if (bestTime < 1f)
        {
            return true;
        }
        return timeElapsed < bestTime;
    }

    public void HandleMissionComplete()
    {
        ShowMissionParticles();
        missionAccepted = false;
        missionComplete = true;
    }

    public void CheckTimeElapsed()
    {

        string timeElapsedString = GetFormattedTime(timeElapsed);
        string bestTimeString = GetFormattedTime(bestTime);

        if (timeElapsed < bestTime || bestTime < 1f)
        {
            PlayerPrefs.SetFloat(missionTimeKey, timeElapsed);
            PlayerPrefs.Save();
            bestTime = timeElapsed;
        }
    }

    public void HandleMissionFailed()
    {
        ShowMissionParticles();
        missionAccepted = false;
        missionFailed = true;
    }
}

public enum MissionType
{
    Transport,
    Rescue,
    Report,
    Delivery
}