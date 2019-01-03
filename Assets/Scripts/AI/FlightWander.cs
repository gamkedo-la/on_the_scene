using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightWander : MonoBehaviour {

    public AIManager manager;

    [SerializeField] float flightSpeed = 5f;
    [SerializeField] float turnSpeed = 2f;
    private Vector3 desiredPosition;

    // Use this for initialization
    void Start()
    {
        desiredPosition = transform.position;
    }
	// Update is called once per frame
	void Update () {
		this.transform.Translate (Vector3.forward * flightSpeed * Time.deltaTime);
	}
}
