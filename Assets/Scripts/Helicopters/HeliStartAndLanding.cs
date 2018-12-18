using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliStartAndLanding : MonoBehaviour
{
	private enum EngineState
	{
		CanBeTurnedOn,
		Running,
		CanBeTurnedOff
	}

	[SerializeField] private Behaviour[] behavioursToChange = null;
	[SerializeField] private GameObject[] gameObjectsToChange = null;
	[SerializeField] private GameObject tutorialMessage = null;

	private EngineState engineState = EngineState.CanBeTurnedOn;

	void Start ()
	{

	}

	void Update ()
	{
		if ( engineState == EngineState.CanBeTurnedOn && Input.GetKeyDown( KeyCode.E ) )
		{
			engineState = EngineState.Running;
			if ( tutorialMessage )
				tutorialMessage.SetActive( false );
			SetAll( true );
		}
	}

	private void SetAll( bool state )
	{
		foreach ( var behaviour in behavioursToChange )
			behaviour.enabled = state;

		foreach ( var gameObj in gameObjectsToChange )
			gameObj.SetActive( state );
	}
}
