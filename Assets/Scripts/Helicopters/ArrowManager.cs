using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour {

void LateUpate()
    {
        Transform nearestObjective = MissionController.GetNearestObjective();
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
