using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMover : MonoBehaviour
{

    private Quaternion targetRotation; //lerp towards this value every frame
    private Vector3 targetVector3; //approach every frame

    private Rigidbody rb;
    
    // Public params
    public float carSpeed = 3.0f;
    public float closeEnoughToSlowDown = 1.0f;
    
    //Internal vars
    private float speedScaleToTurnSharp = 1.0f;

    // Use this for initialization
    void Start()
    {
        targetRotation = transform.rotation; // no target initially
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3.0f);
        
        float distToWayPoint = Vector3.Distance(transform.position, targetVector3);
        if (distToWayPoint < closeEnoughToSlowDown)
        {
            speedScaleToTurnSharp = distToWayPoint / closeEnoughToSlowDown;
        } else { speedScaleToTurnSharp = 1.0f; }

        rb.velocity = (transform.forward * carSpeed * speedScaleToTurnSharp);
    }

    public void TurnTowards(Vector3 pointAt)
    {
        pointAt.y = transform.position.y; // make sure that we don't look slightly downwards (might refactor)
        targetRotation = Quaternion.LookRotation(pointAt - transform.position);
        targetVector3 = pointAt;
    }
}
