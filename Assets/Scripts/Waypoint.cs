using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	public Waypoint[] nextWaypoints;

    [SerializeField]
    public bool stopHere; //could find a way to properly encapsulate this ;/

    public float recommendedSpeed = 0; //max speed that we should be going from this point to the next


    public Waypoint SelectRandomWaypoint ()
    {
        if (nextWaypoints.Length == 0) {return null;}

        return nextWaypoints[Random.Range(0,nextWaypoints.Length)];
    }
}
