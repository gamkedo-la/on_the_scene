using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDustClouds : MonoBehaviour {
    ParticleSystem ps;
    private Transform altitudePoint = null;
    private RaycastHit objectBelow;
    float returnControlTime = 0.75f;
    float distanceFromGround = 1.5f;

    // Use this for initialization
    void Start () {
        ps = gameObject.GetComponent<ParticleSystem>();
        StartCoroutine(checkForGround());
    }

    IEnumerator checkForGround ()
    {
        while (true) {
            altitudePoint = HeliController.instance.GetAltitudePoint();
            Physics.Raycast(altitudePoint.position, Vector3.down, out objectBelow);
            yield return new WaitForSeconds(returnControlTime);
        }
    }

    // Update is called once per fram
    void Update () {
        //Debug.Log("Distance to other: " + objectBelow.distance);
        if (objectBelow.distance < distanceFromGround) {
            ps.Play();
        } else {
            ps.Stop();
        }
    }
}
