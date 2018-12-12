using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayWhenHelicopterIsNear : MonoBehaviour {
    public Transform helicopter;
	
	void Start () {
       
    }
	
	void Update () {
        if (Vector3.Distance(helicopter.position, transform.position) < 10.0f) {
            Debug.Log("close enough");
        }
    }
}
