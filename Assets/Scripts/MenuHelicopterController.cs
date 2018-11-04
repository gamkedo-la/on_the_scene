using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHelicopterController : MonoBehaviour {
    private Quaternion startRot;
    private void Start()
    {
        startRot = transform.localRotation;
    }
    // Update is called once per frame
    void Update () {
        transform.rotation = startRot * Quaternion.AngleAxis(3.0f * Mathf.Cos(0.5f * Time.timeSinceLevelLoad), Vector3.right) *
            Quaternion.AngleAxis(2.0f * Mathf.Cos(0.7f * Time.timeSinceLevelLoad), Vector3.forward);

    }
}
