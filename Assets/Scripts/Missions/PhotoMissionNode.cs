using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoMissionNode : MonoBehaviour {

    HeliController player;
    public float counter = 0.0f;
    public float maxTime = 20.0f;
    public bool pictureTaken = false;

    private void OnTriggerEnter(Collider other)
    {
        HeliController temp = other.GetComponent<HeliController>();
        if (temp != null)
        {
            player = temp;
            StartCoroutine(UpdateCounter());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (player != null)
        {
            Debug.Log("Player is in collider and staying");
            if (counter <= maxTime)
            {
                pictureTaken = true;
                StopCoroutine(UpdateCounter());
            }
        }
    }

    IEnumerator UpdateCounter()
    {
        yield return new WaitForSeconds(1);
        if (player.timeSinceLastMove <= 0.0f)
        {
            counter += 1.0f;
        }
        else
        {
            counter = 0.0f;
        }
    }

}
