using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    CarMover carMover;
    Vector3 pointAt;

    public WayPointData[] waypoints;
    
    public bool reachedDestination = false;

    private int seekingWayPoint = 0;
    private WayPointData targetWayPoint;

    WayPointData currentWaypoint;
    

    // Use this for initialization
    void Start()
    {
        carMover = GetComponent<CarMover>();
        currentWaypoint = waypoints[seekingWayPoint];

        pointAt = currentWaypoint.transform.position;
        carMover.TurnTowards(pointAt);
    }

    // Update is called once per frame
    void Update()
    {

        carMover.TurnTowards(pointAt);

    }

    public void NextWaypoint()
    {
        currentWaypoint = currentWaypoint.SelectRandomWaypoint();
        pointAt = currentWaypoint.transform.position;

    }
}
