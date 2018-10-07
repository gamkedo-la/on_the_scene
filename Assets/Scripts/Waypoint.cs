using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	public Waypoint[] nextWaypoints;

    [SerializeField]
    public bool stopHere; //could find a way to properly encapsulate this ;/


    public Waypoint SelectRandomWaypoint ()
    {
        
        return nextWaypoints[Random.Range(0,nextWaypoints.Length)];
    }
}
