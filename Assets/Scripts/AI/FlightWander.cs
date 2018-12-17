using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightWander : MonoBehaviour {

    public float flightSpeed = 5f;
    public float turnSpeed = 2f;
    public Transform targetTransform;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Translate (Vector3.forward * flightSpeed * Time.deltaTime);
	}
}
