using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoMissionObjective : MonoBehaviour
{

    HeliController player;
    public float maxTime = 20.0f;

    [FMODUnity.EventRef]
    public string ObjectiveSuccessEvent = "event:/Missions/MissionObjectiveComplete";
    private FMOD.Studio.EventInstance objectiveSuccessSound;
    private Rigidbody cachedRigidBody;

    private void Start()
    {
        cachedRigidBody = HeliController.instance.GetComponentInParent<Rigidbody>();
        if (cachedRigidBody == null)
        {
            Debug.Log("Unable to get rigidbody from HeliController");
        }

        objectiveSuccessSound = FMODUnity.RuntimeManager.CreateInstance(ObjectiveSuccessEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(objectiveSuccessSound, GetComponent<Transform>(), GetComponent<Rigidbody>());
        objectiveSuccessSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, cachedRigidBody));
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
            if (player.timeSinceLastMove >= maxTime)
            {
                //Debug.Log("MaxTime has been reached");
                objectiveSuccessSound.start();
                MissionController.ObjectiveReportingComplete(gameObject);
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
