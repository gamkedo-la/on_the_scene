using UnityEngine;

public class WayPoint : MonoBehaviour {

    private void Start()
    {
        Debug.Log("I'm a waypoint with the name: " + gameObject.name);
    }

    private void Update()
    {
        Debug.Log("I'm still alive");
    }

    private void OnTriggerEnter(Collider other)
    {
        WaypointFollower car = other.gameObject.GetComponent<WaypointFollower>();
        Debug.Log("Someone entered me! That some is: " + other.name);
        if (car != null)
        {
            car.reachedDesitnation = true;
        }
    }
}
