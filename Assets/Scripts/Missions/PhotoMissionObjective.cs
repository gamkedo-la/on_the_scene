using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhotoMissionObjective : MonoBehaviour
{

    HeliController player;
    public float maxTime = 20.0f;

    [FMODUnity.EventRef]
    public string ObjectiveSuccessEvent = "event:/Missions/MissionObjectiveComplete";
    private FMOD.Studio.EventInstance objectiveSuccessSound;

    public TextMeshProUGUI countDownText;

    private void Start()
    {
        countDownText.text = "";
        objectiveSuccessSound = FMODUnity.RuntimeManager.CreateInstance(ObjectiveSuccessEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(objectiveSuccessSound, GetComponent<Transform>(), GetComponent<Rigidbody>());
    }

    private void OnTriggerEnter(Collider other)
    {
        HeliController temp = other.gameObject.GetComponentInChildren<HeliController>();
        if (temp != null)
        {
            player = temp;
            player.IndicatorUpdate(IndicatorManager.signState.StopSign);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        HeliController temp = other.gameObject.GetComponentInChildren<HeliController>();
        if (temp != null)
        {
            player = temp;
            player.IndicatorUpdate(IndicatorManager.signState.Arrow);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (player != null)
        {
            countDownText.text = "Time remaining: " + ((int)maxTime - (int)player.timeSinceLastMove);
            if (player.timeSinceLastMove >= maxTime)
            {
                //Debug.Log("MaxTime has been reached");
                objectiveSuccessSound.start();
                MissionController.ObjectiveReportingComplete(gameObject);
                countDownText.text = "";
                int objectivesLeft = MissionController.GetMissionObjectives();
                if (objectivesLeft <= 0)
                {
                    player.IndicatorUpdate(IndicatorManager.signState.None);
                }
                else
                {
                    player.IndicatorUpdate(IndicatorManager.signState.Arrow);
                }
            } // end of player.timeSinceLastMove >= maxTime
        } // end of if player != null
    } // end of OnTriggerStay
} // end of PhotoMissionNode class
