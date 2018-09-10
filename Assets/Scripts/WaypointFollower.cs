using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{

    public WayPointData[] waypoints;
    public bool reachedDesitnation = false;
    private int seekingWayPoint = 0;
    CarMover carMover;
    Vector3 pointAt;

    // Use this for initialization
    void Start()
    {
        carMover = GetComponent<CarMover>();
        WayPointData nextWP = waypoints[seekingWayPoint];
        pointAt = nextWP.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        carMover.TurnTowards(pointAt);

        if (reachedDesitnation)
        {
            seekingWayPoint += 1;
            if (seekingWayPoint >= waypoints.Length)
            {
                seekingWayPoint = 0;
            }
            WayPointData nextWP = waypoints[seekingWayPoint];
            pointAt = nextWP.transform.position;
            reachedDesitnation = false;
        }
    }
}
