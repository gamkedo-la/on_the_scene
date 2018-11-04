using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	public Waypoint[] nextWaypoints;

    [SerializeField]
    public bool stopHere; //could find a way to properly encapsulate this ;/

    public bool isInitial; 
    public bool isTerminal; //will search for nearby initial waypoints and link on startup

    public float recommendedSpeed = 0; //max speed that we should be going from this point to the next
    public float recommendedApproachSpeed = 0; //max speed at which we should approach this point


    public void Start ()
    {
        if (isTerminal) {
            
            Collider col = GetComponent<SphereCollider>();

            if (col != null) {
                Waypoint next = SearchForOverlappingWaypoint();

                if (next != null && next.isInitial && next != this) {
                    nextWaypoints[0] = next;

                    if (next.recommendedApproachSpeed != 0) {
                        this.recommendedApproachSpeed = next.recommendedApproachSpeed;
                    }
                }
            }
        }
    }

    public Waypoint SearchForOverlappingWaypoint ()
    {

        Collider[] hit = Physics.OverlapSphere(transform.position, 0.5f);

        for (var i = 0; i < hit.Length; i++)
        {
            Waypoint other = hit[i].gameObject.GetComponent<Waypoint>();
            
            if (other != null && other != this)
            {
                return other;
            }
        }

        return null;
    }

    public Waypoint SelectRandomWaypoint ()
    {
        if (nextWaypoints.Length == 0) { return null; }

        return nextWaypoints[Random.Range(0,nextWaypoints.Length)];
    }

}
