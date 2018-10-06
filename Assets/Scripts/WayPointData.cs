using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointData : MonoBehaviour
{

    public WayPointData[] nextWaypoints;


    public WayPointData SelectRandomWaypoint ()
    {
        return nextWaypoints[Random.Range(0,nextWaypoints.Length)];
    }
}
