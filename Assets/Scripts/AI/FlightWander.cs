using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightWander : MonoBehaviour {

    public AIManager manager;

    public float flightSpeed = 5f;
    public float turnSpeed = 2f;
    private Vector3 desiredPosition;

    // Use this for initialization
    void Start()
    {
        desiredPosition = transform.position;
        float terrainY = Terrain.activeTerrain.SampleHeight(transform.position);
        if (terrainY > transform.position.y)
        {
            Debug.Log("I was under the terrain!");
            desiredPosition.y = Mathf.Max(desiredPosition.y, (terrainY + 4.0f));
            transform.position = desiredPosition;
        }
    }
	
	// Update is called once per frame
	void Update () {
		//this.transform.Translate (Vector3.forward * flightSpeed * Time.deltaTime);
	}
}
