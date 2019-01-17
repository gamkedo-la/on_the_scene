using UnityEngine;
using UnityEngine.Assertions;

public class HeliStartAndLanding : MonoBehaviour
{
	public enum EngineState
	{
		CanBeTurnedOn,
		Running,
		CanBeTurnedOff
	}

	[SerializeField] private Behaviour[] behavioursToChange = null;
	[SerializeField] private GameObject[] gameObjectsToChange = null;
	[SerializeField] private GameObject tutorialMessage = null;
	[SerializeField] private LayerMask layerMask;
	[SerializeField] private HeliController heli = null;
	[SerializeField] private float minLandingVelocity = 1f;
	[SerializeField] private float minLandingAltitude = 1f;

	private Rigidbody heliRigidbody;
	private EngineState engineState = EngineState.CanBeTurnedOn;
	private RaycastHit hit;
	private float worldScale = 1f;

	void Start ()
	{
		Assert.IsNotNull( heli );

		heliRigidbody = heli.transform.parent.GetComponent<Rigidbody>( );
		worldScale = heli.GetWorldScale( );
	}

	void Update ()
	{
		if ( engineState == EngineState.CanBeTurnedOn && ( Input.GetKeyDown( KeyCode.E ) || Input.GetButtonDown( "Engine" ) ) )
		{
			engineState = EngineState.Running;
			if ( tutorialMessage )
				tutorialMessage.SetActive( false );
			SetAll( true );
		}
		else if ( engineState == EngineState.Running && ( Input.GetKeyDown( KeyCode.E ) || Input.GetButtonDown( "Engine" ) ) && HeliIsAlmostStill( ) )
		{
			engineState = EngineState.CanBeTurnedOn;
			SetAll( false );
			heliRigidbody.velocity = Vector3.zero;
			heliRigidbody.angularVelocity = Vector3.zero;
		}
	}

	public EngineState GetEngineState()
	{
		return engineState;
	}

	private void FixedUpdate( )
	{
		if (engineState == EngineState.CanBeTurnedOn)
		{
			heliRigidbody.velocity = Vector3.zero;
			heliRigidbody.angularVelocity = Vector3.zero;
		}
	}

	private void SetAll( bool state )
	{
		foreach ( var behaviour in behavioursToChange )
			behaviour.enabled = state;

		foreach ( var gameObj in gameObjectsToChange )
			gameObj.SetActive( state );
	}

	private bool HeliIsAlmostStill()
	{
		Physics.Raycast( heli.GetAltitudePoint().position, Vector3.down, out hit, layerMask );

		return heli.GetVelocity( ) <= minLandingVelocity && ( hit.distance * worldScale ) <= minLandingAltitude;
	}
}
