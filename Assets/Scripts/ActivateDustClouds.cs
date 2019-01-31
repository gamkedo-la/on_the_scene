using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDustClouds : MonoBehaviour {
    ParticleSystem ps;
    private Transform HelicopterTransform;
    private Transform altitudePoint = null;
    private RaycastHit objectBelow;
    float returnControlTime = 0.75f;
    float distanceFromGround = 1.5f;

    // Use this for initialization
    void Start () {
        ps = gameObject.GetComponent<ParticleSystem>();
        HelicopterTransform = HeliController.instance.transform;
        StartCoroutine(CheckForGround());
    }

    IEnumerator CheckForGround ()
    {
        while (true) {
            altitudePoint = HeliController.instance.GetAltitudePoint();
            Physics.Raycast(altitudePoint.position, Vector3.down, out objectBelow);
            ps.transform.position = new Vector3(altitudePoint.position.x - 3.0f,  // Takes ps to end of helicopter
                HelicopterTransform.position.y - objectBelow.distance,
                altitudePoint.position.z);
            yield return new WaitForSeconds(returnControlTime);
        }
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log("Distance to other: " + objectBelow.distance);
        if (objectBelow.distance < distanceFromGround) {
            StopCoroutine(CheckForGround());
            ps.Play();
        } else {
            ps.Stop();
            StartCoroutine(CheckForGround());
        }
    }
}
