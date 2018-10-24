using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (HeloController.instance != null)
			transform.position = HeloController.instance.transform.position + Vector3.up * 50.0f;
		else
			transform.position = HeliController.instance.transform.position + Vector3.up * 50.0f;
	}
}
