﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeloController : MonoBehaviour {

    public float rotationSpeed = 15.0f;
    public float tiltDriftEffect = 30.0f;
    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("transform.up.z is " + transform.up.z);
        Vector3 flattenedTilt = transform.forward * transform.up.z * tiltDriftEffect +
            transform.right * transform.up.x * tiltDriftEffect;
        flattenedTilt.y = 0;
        rb.velocity = flattenedTilt;
        //transform.Rotate(Vector3.forward, Input.GetAxis("Horizontal") * Time.deltaTime * -rotationSpeed);
        transform.Rotate(Vector3.right, Input.GetAxis("Vertical") * Time.deltaTime * rotationSpeed);
        transform.Rotate(Vector3.up, Input.GetAxis("Swivel") * Time.deltaTime * rotationSpeed);
	}
}
