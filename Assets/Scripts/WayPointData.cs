using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointData : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {

        WaypointFollower car = other.gameObject.GetComponent<WaypointFollower>();

        if (car != null)
        {
            car.reachedDesitnation = true;
        }
    }
}
