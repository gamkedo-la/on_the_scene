using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampLight : MonoBehaviour {

    // Use this for initialization
    private Transform lightCone;

    public void Cycle(bool isDay)
    {
        lightCone = transform.Find("LightCone");
        //Debug.Log(isDay);
        lightCone.gameObject.SetActive(!isDay);
    }
}
