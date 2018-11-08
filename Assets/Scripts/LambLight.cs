using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LambLight : MonoBehaviour {

    // Use this for initialization
    private Transform lightCone;

	void Start () {
        lightCone =  transform.Find("LightCone");

    }
	
	// Update is called once per frame
	void Update () {
        if (!WorldState.instance.isDay)
        {
            lightCone.gameObject.SetActive(true);
        }
        else
        {
            lightCone.gameObject.SetActive(false);
        }
	}
}
