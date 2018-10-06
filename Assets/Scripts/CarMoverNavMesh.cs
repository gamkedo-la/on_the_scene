using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarMoverNavMesh : MonoBehaviour {

	[SerializeField]
	WayPointData _destination;

	NavMeshAgent navMeshAgent;

	// Use this for initialization
	void Start ()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		SetDestination();
	}
	
	void SetDestination () {

		if (_destination != null) {
			Vector3 targetVector = _destination.transform.position;
			navMeshAgent.SetDestination(targetVector);
		}
	}

	void Update () {

		if (navMeshAgent.remainingDistance == 0) {
			NextWaypoint();
		}
	}

	void NextWaypoint() {

		_destination = _destination.SelectRandomWaypoint();
		navMeshAgent.SetDestination(_destination.transform.position);
	}
}
