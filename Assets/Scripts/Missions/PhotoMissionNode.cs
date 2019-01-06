using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoMissionNode : MonoBehaviour {

    HeliController player;
    public float counter = 0.0f;
    public float maxTime = 20.0f;

    private void OnTriggerEnter(Collider other)
    {
        HeliController temp = other.gameObject.GetComponentInChildren<HeliController>();
        if (temp != null)
        {
            player = temp;
            StartCoroutine(UpdateCounter());
            Debug.Log("Player has been set");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (player != null)
        {
            Debug.Log("Player is in collider and staying");
            if (counter <= maxTime)
            {
                Debug.Log("MaxTime has been reached");
                //MissionController.ObjectiveReportingComplete(gameObject);
                //StopCoroutine(UpdateCounter());
                //Destroy(gameObject);
            }
        }
    }

    IEnumerator UpdateCounter()
    {
        yield return new WaitForSeconds(1);
        //if (player.timeSinceLastMove <= 0.0f)
        //{
        //    counter += 1.0f;
        //}
        //else
        //{
        //    counter = 0.0f;
        //}
        counter += 1;
        Debug.Log("Counter is " + counter);
    }

}
