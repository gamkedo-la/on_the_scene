using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public Transform nearestObjective;

    private void Update()
    {
        if (MissionController.GetMissionObjectives() <= 0)
        {
            return;
        }
        nearestObjective = MissionController.GetNearestObjective().transform;
        if (nearestObjective != null)
        {
            transform.LookAt(nearestObjective);
        }
        else
        {
            Debug.Log("Indicator is on and there are no objectives.");
        }
    }

    private void LateUpate()
    {
        if (MissionController.GetMissionObjectives() <= 0)
        {
            return;
        }
        nearestObjective = MissionController.GetNearestObjective().transform;
        Debug.Log("Late Update happened and nearestObjective is: " + nearestObjective);
        if (nearestObjective != null)
        {
            transform.LookAt(nearestObjective);
        }
        else
        {
            Debug.Log("Indicator is on and there are no objectives.");
        }
    }
}
