using UnityEngine;

public class WayPoint : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        WaypointFollower car = other.gameObject.GetComponent<WaypointFollower>();
        //Debug.Log("Someone entered me! That some is: " + other.name);
        if (car != null)
        {
            car.reachedDesitnation = true;
        }
    }
}
