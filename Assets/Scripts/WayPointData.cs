using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointData : MonoBehaviour
{

    public float closeEnoughToArrive = 0.5f; // when distance is <=, consider destination reached
    
    void OnTriggerEnter(Collider other)
    {

        WaypointFollower car = other.gameObject.GetComponent<WaypointFollower>();

        if (car != null)
        {
            car.reachedDestination = true;
        }
    }
}
