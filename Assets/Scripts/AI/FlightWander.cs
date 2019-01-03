using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightWander : MonoBehaviour {

    
    public AIManager manager;

    [SerializeField] float flightSpeed = 5f;
    [SerializeField] float turnSpeed = 2f;
    private Vector3 desiredPosition;
    bool turning = false;

    // Use this for initialization
    void Start()
    {
        desiredPosition = transform.position;
    }
	// Update is called once per frame
	void Update () {
        Bounds box = new Bounds(manager.transform.position, manager.flightZone);

        if (!box.Contains(point: transform.position)) //if the plane is not inside the bounds box
        {
            turning = true; //start turning
        }
        else
            turning = false; //as you were

        if (turning)
        {
            Vector3 direction = manager.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
        }
        else
        {
            if(Random.Range(0, 100) < 75)
            {
                ApplyRules();
            }
        }

        transform.Translate(0, 0, Time.deltaTime * flightSpeed);

    }

    void ApplyRules()
    {
        GameObject[] planes;
        planes = manager.planes;

        Vector3 targetPosition = manager.targetPosition;

        foreach(GameObject plane in planes)
        {
            Vector3 direction = targetPosition - transform.position;

            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(direction),
                                                      turnSpeed * Time.deltaTime);

        }

    }

}
