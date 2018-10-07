using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMover : MonoBehaviour
{

    private Quaternion targetRotation; //lerp towards this value every frame
    private Vector3 targetVector3; //approach every frame

    private Rigidbody rb;
    private WaypointFollower follower;
    
    // Public params
    public float carSpeed = 3.0f;
    
    //Internal vars
    private float speedScaleToTurnSharp = 1.0f;

    private float lastDist = 0;

    // Use this for initialization
    void Start()
    {
        targetRotation = transform.rotation; // no target initially
        rb = GetComponent<Rigidbody>();
        follower = GetComponent<WaypointFollower>();

    }

    // Update is called once per frame
    void Update()
    {

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3.0f);
        
        float distToWaypoint = Vector3.Distance(transform.position, targetVector3);

        //Debug.Log("Distance from waypoint:" + distToWaypoint);

        if (Mathf.Sign(lastDist - distToWaypoint) == -1 && distToWaypoint < 10) {

            follower.NextWaypoint();

        }


        rb.velocity = (transform.forward * carSpeed * speedScaleToTurnSharp);

        lastDist = distToWaypoint;
    }

    public void TurnTowards(Vector3 pointAt)
    {
        pointAt.y = transform.position.y; // make sure that we don't look slightly downwards (might refactor)
        targetRotation = Quaternion.LookRotation(pointAt - transform.position);
        targetVector3 = pointAt;
    }
}
