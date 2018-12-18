using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDustClouds : MonoBehaviour {
    ParticleSystem ps;
    public Mesh ground;
    float distanceFromGround = 3.0f;

    // Use this for initialization
    void Start () {
        ps = gameObject.GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
		////var distanceFromGround = Vector3.Distance(ground.position, transform.position);
        //Debug.Log("Distance to other: " + distanceFromGround);
        //if (distanceFromGround < distanceCheck) {
        //    ps.Play();
        //} else {
        //    ps.Stop();
        //}
    }
}
