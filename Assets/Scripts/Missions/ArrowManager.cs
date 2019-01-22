using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public Transform nearestObjective;

    private void Update()
    {
        nearestObjective = MissionController.GetNearestObjective();
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
        nearestObjective = MissionController.GetNearestObjective();
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
