using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    CarMover carMover;
    Vector3 pointAt;

    public Waypoint[] waypoints;
    
    public bool reachedDestination = false;

    private int seekingWaypoint = 0;
    private Waypoint targetWaypoint;

    Waypoint currentWaypoint;
    

    // Use this for initialization
    void Start()
    {
        carMover = GetComponent<CarMover>();
        currentWaypoint = waypoints[seekingWaypoint];

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
