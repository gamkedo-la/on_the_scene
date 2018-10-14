using UnityEngine;
using UnityEngine.Assertions;

public class HeliControls : MonoBehaviour
{
	[Header("Parts")]
	[SerializeField] private GameObject rotorMain = null;
	[SerializeField] private GameObject rotorTail = null;
	[SerializeField] private GameObject wholeHeli = null;
	[SerializeField] private Transform centerOfMass;

	[Header("Forces")]
	[SerializeField] private float forceMain = 1500f;
	[SerializeField] private float forcePitch = 1f;
	[SerializeField] private float forceRoll = 1f;
	[SerializeField] private float forceYaw = 0.5f;

	[Header("Control Parameters")]
	[SerializeField] private float constantUpThrottle = 8f;
	[SerializeField] private float throttleChangeSpeed = 0.6f;
	[SerializeField] private float maxThrottle = 10f;
	[SerializeField] private float maxRoll = 1f;
	[SerializeField] private float maxPitch = 1f;
	[SerializeField] private float maxYaw = 1f;
	[SerializeField] private float backToNeutralSpeed = 10f;

	private float currentThrottle;
	private float currentRoll;
	private float currentPitch;
	private float currentYaw;
	private Rigidbody rb;

	void Start ()
	{
		Assert.IsNotNull( rotorMain );
		Assert.IsNotNull( rotorTail );
		Assert.IsNotNull( wholeHeli );
		Assert.IsNotNull( centerOfMass );

		rb = wholeHeli.GetComponent<Rigidbody>( );
		Assert.IsNotNull( rb );

		rb.centerOfMass = centerOfMass.localPosition;

		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update ()
	{
		HandleControls( );
	}

	void FixedUpdate( )
	{
		RotateAndMove( );
	}

	private void HandleControls( )
	{
		// Throttle
		if ( Input.GetKey( KeyCode.W ) )
			currentThrottle += throttleChangeSpeed * Time.deltaTime;
		else if ( Input.GetKey( KeyCode.S ) )
			currentThrottle -= throttleChangeSpeed * Time.deltaTime;
		else
			currentThrottle = Mathf.Lerp( currentThrottle, 0, backToNeutralSpeed * Time.deltaTime );

		// Yaw
		if ( Input.GetAxis( "Mouse X" ) > 0.1f )
			currentYaw -= 1 * Time.deltaTime;
		else if ( Input.GetAxis( "Mouse X" ) < -0.1f )
			currentYaw += 1 * Time.deltaTime;
		else
			currentYaw = Mathf.Lerp( currentYaw, 0, backToNeutralSpeed * Time.deltaTime );

		// Pitch
		if ( Input.GetAxis( "Mouse Y" ) > 0.1f )
			currentPitch += 1 * Time.deltaTime;
		else if ( Input.GetAxis( "Mouse Y" ) < -0.1f )
			currentPitch -= 1 * Time.deltaTime;
		else
			currentPitch = Mathf.Lerp( currentPitch, 0, backToNeutralSpeed * Time.deltaTime );

		// Roll
		if ( Input.GetKey( KeyCode.D ) )
			currentRoll += 1 * Time.deltaTime;
		else if ( Input.GetKey( KeyCode.A ) )
			currentRoll -= 1 * Time.deltaTime;
		else
			currentRoll = Mathf.Lerp( currentRoll, 0, backToNeutralSpeed * Time.deltaTime );

		currentYaw = Mathf.Clamp( currentYaw, -maxYaw, maxYaw );
		currentPitch = Mathf.Clamp( currentPitch, -maxPitch, maxPitch );
		currentRoll = Mathf.Clamp( currentRoll, -maxRoll, maxRoll );
		currentThrottle = Mathf.Clamp( currentThrottle, -maxThrottle, maxThrottle );
	}

	private void RotateAndMove( )
	{
		// Move force
		float force = ( forceMain * ( currentThrottle + constantUpThrottle ) );
		rb.AddRelativeForce( Vector3.up * force );

		// Heli rotation
		wholeHeli.transform.Rotate( new Vector3( currentPitch * forcePitch, currentRoll * forceRoll, currentYaw * forceYaw ) );

		// Rotor animations
		rotorMain.transform.Rotate( Vector3.up, Mathf.Lerp( 0, force, 0.36f * Time.deltaTime ) );
		rotorTail.transform.Rotate( Vector3.right, Mathf.Lerp( 0, ( force / 0.5f ), 0.36f * Time.deltaTime ) );
	}
}
