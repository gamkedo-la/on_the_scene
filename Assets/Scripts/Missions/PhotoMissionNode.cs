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
            Debug.Log("Player has been set");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (player != null)
        {
            Debug.Log("Player is in collider and staying");
            if (player.timeSinceLastMove >= maxTime)
            {
                //Debug.Log("MaxTime has been reached");
                MissionController.ObjectiveReportingComplete(gameObject);
                Destroy(gameObject);
            }
        }
    }

}
