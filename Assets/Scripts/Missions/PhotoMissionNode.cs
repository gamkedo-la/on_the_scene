using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoMissionNode : MonoBehaviour {

    HeliController player;
    public float maxTime = 20.0f;

    private void OnTriggerEnter(Collider other)
    {
        HeliController temp = other.gameObject.GetComponentInChildren<HeliController>();
        if (temp != null)
        {
            player = temp;
            player.IndicatorUpdate(IndicatorManager.signState.StopSign);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (player != null)
        {
            if (player.timeSinceLastMove >= maxTime)
            {
                //Debug.Log("MaxTime has been reached");
                MissionController.ObjectiveReportingComplete(gameObject);
                player.IndicatorUpdate(IndicatorManager.signState.None);
                Destroy(gameObject);
                MissionController.GetNearestObjective();
            }
        }
    }

}
