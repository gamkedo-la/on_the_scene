using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMover : MonoBehaviour
{

    private Quaternion targetRotation;
    public float carSpeed = 3.0f;
    private Rigidbody rb;
    public Vector3 targetVector3;
    public float closeEnoughToSlowDown = 1.0f;
    // Use this for initialization
    void Start()
    {
        targetRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3.0f);
        float speedScaleToTurnSharp = 1.0f;
        float distToWayPoint = Vector3.Distance(transform.position, targetVector3);
        if (distToWayPoint < closeEnoughToSlowDown)
        {
            speedScaleToTurnSharp = distToWayPoint / closeEnoughToSlowDown;
        }
        rb.velocity = (transform.forward * carSpeed * speedScaleToTurnSharp);
    }

    public void TurnTowards(Vector3 pointAt)
    {
        pointAt.y = transform.position.y;
        targetRotation = Quaternion.LookRotation(pointAt - transform.position);
        targetVector3 = pointAt;
    }
}
