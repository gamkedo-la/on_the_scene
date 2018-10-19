﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarMoverNavMesh : MonoBehaviour {

	[SerializeField]
	Waypoint _destination;

	NavMeshAgent navMeshAgent;

	private float lastDist = 0;

	// Use this for initialization
	void Start ()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		
		if (_destination != null) {
			SetDestination(_destination.transform.position);
			if (_destination.recommendedSpeed != 0) {
				navMeshAgent.speed = _destination.recommendedSpeed;
			}
		}
	}

	void Update () {

		//Debug.Log(navMeshAgent.remainingDistance + " " + lastDist);
		//Debug.Log(lastDist - navMeshAgent.remainingDistance);

		if (Mathf.Sign(lastDist - navMeshAgent.remainingDistance) == -1 && lastDist != 0) {
			NextWaypoint();
			return; //we're done here
		}

		lastDist = navMeshAgent.remainingDistance;
	}
	
	void SetDestination (Vector3 targetVector) {

		navMeshAgent.SetDestination(targetVector);
		navMeshAgent.isStopped = false;

	}
	

	void NextWaypoint() {

		// Stuff to do BEFORE we switch


		//Switch happens from here
		if (_destination.nextWaypoints.Length != 0) {

			Waypoint next = _destination.SelectRandomWaypoint();
			if(next) {
			_destination = next;
			}
		}

		//Check at what speed we should be aiming for heading to the next Waypoint
		//Note: does not immediately change speed, only sets max speed which is then handled by the NavMeshAgent
		if (_destination.recommendedSpeed != 0) {
			navMeshAgent.speed = _destination.recommendedSpeed;
		}

		lastDist = 0; // we consider that we just passed the point, hence the 0

		navMeshAgent.SetDestination(_destination.transform.position);

		// Leaving this here because more functionalities could be added
		if (_destination.stopHere || _destination.nextWaypoints.Length == 0) {
			navMeshAgent.autoBraking = true;
		} else { navMeshAgent.autoBraking = false;}

		
	}
}
