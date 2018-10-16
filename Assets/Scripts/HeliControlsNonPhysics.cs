using UnityEngine;
using UnityEngine.Assertions;

public class HeliControlsNonPhysics : MonoBehaviour
{
	[Header("Parts")]
	[SerializeField] private GameObject rotorMain = null;
	[SerializeField] private GameObject rotorTail = null;
	[SerializeField] private GameObject wholeHeli = null;
	[SerializeField] private Transform centerOfMass;

	[Header("Forces")]
	///[SerializeField] private float forceMain = 1500f;
	[SerializeField] private float speedMax = 3f;
	[SerializeField] private float forcePitch = 1f;
	[SerializeField] private float forceRoll = 1f;
	[SerializeField] private float forceYaw = 0.5f;

	[Header("Throttle")]
	[SerializeField] private float throttleChangeSpeed = 3f;
	[SerializeField] private float throttleHitDampning = 3f;
	[SerializeField] private float maxThrottle = 10f;

	[Header("Yaw")]
	[SerializeField] private float yawDesierdChangeSpeed = 80f;
	[SerializeField] private float yawChangeSpeed = 0.8f;
	[SerializeField] private float maxYaw = 45f;

	[Header("Pitch")]
	[SerializeField] private float pitchDesierdChangeSpeed = 80f;
	[SerializeField] private float pitchChangeSpeed = 0.8f;
	[SerializeField] private float maxPitch = 45f;

	[Header("Roll")]
	[SerializeField] private float rollChangeSpeed = 3f;
	[SerializeField] private float maxRoll = 45f;

	[SerializeField] private float backToNeutralSpeed = 1.5f;

	public float currentThrottle;
	public float currentRoll;
	public float currentPitch;
	public float currentPitchDesiered;
	public float currentYaw;
	public float currentYawDesiered;
	private Rigidbody rb;

	void Start( )
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

	void Update( )
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
			currentYawDesiered -= yawDesierdChangeSpeed * 1 * Time.deltaTime;
		else if ( Input.GetAxis( "Mouse X" ) < -0.1f )
			currentYawDesiered += yawDesierdChangeSpeed * 1 * Time.deltaTime;
		///else
			///currentYawDesiered = Mathf.Lerp( currentYawDesiered, 0, backToNeutralSpeed * Time.deltaTime );

		// Pitch
		if ( Input.GetAxis( "Mouse Y" ) > 0.1f )
			currentPitchDesiered += pitchDesierdChangeSpeed * 1 * Time.deltaTime;
		else if ( Input.GetAxis( "Mouse Y" ) < -0.1f )
			currentPitchDesiered -= pitchDesierdChangeSpeed * 1 * Time.deltaTime;
		///else
			///currentPitch = Mathf.Lerp( currentPitch, 0, backToNeutralSpeed * Time.deltaTime );

		// Roll
		/*		if ( Input.GetKey( KeyCode.D ) )
					currentRoll += 1 * Time.deltaTime;
				else if ( Input.GetKey( KeyCode.A ) )
					currentRoll -= 1 * Time.deltaTime;
				else
					currentRoll = Mathf.Lerp( currentRoll, 0, backToNeutralSpeed * Time.deltaTime );
		*/
		currentRoll = Mathf.Clamp( currentRoll, -maxRoll, maxRoll );
		currentThrottle = Mathf.Clamp( currentThrottle, -maxThrottle, maxThrottle );

		currentPitchDesiered = Mathf.Clamp( currentPitchDesiered, -maxPitch, maxPitch );
		currentPitch = Mathf.Lerp( currentPitch, currentPitchDesiered, pitchChangeSpeed * Time.deltaTime );

		currentYawDesiered = Mathf.Clamp( currentYawDesiered, -maxYaw, maxYaw );
		currentYaw = Mathf.Lerp( currentYaw, currentYawDesiered, yawChangeSpeed * Time.deltaTime );
	}

	private void RotateAndMove( )
	{
		// Heli rotation
		rb.MoveRotation( Quaternion.Euler( currentPitch, 0, currentYaw ) );
		//wholeHeli.transform.Rotate( new Vector3( currentPitch * forcePitch, currentRoll * forceRoll, currentYaw * forceYaw ) );

		// Rotor animations
		float rotationControl = currentThrottle + maxThrottle + 5;
		rotationControl *= 90 * Time.fixedDeltaTime;
		rotorMain.transform.Rotate( Vector3.up, rotationControl );
		rotorTail.transform.Rotate( Vector3.right, rotationControl );

		// Move
		Vector3 moveVector = Vector3.zero;
		moveVector.y = currentThrottle * speedMax * Time.fixedDeltaTime;

		rb.MovePosition( rb.position + moveVector );
	}

	private void OnCollisionStay( Collision collision )
	{
		if ( currentThrottle > 0 )
		{
			currentThrottle -= throttleHitDampning * 2 * Time.deltaTime;
			currentThrottle = currentThrottle < 0 ? 0 : currentThrottle;
		}
		else
		{
			currentThrottle += throttleHitDampning * 2 * Time.deltaTime;
			currentThrottle = currentThrottle > 0 ? 0 : currentThrottle;
		}
	}

	private void OnCollisionEnter( Collision collision )
	{
		Debug.Log( "Kolizja" );
		if (currentThrottle > 0)
		{
			currentThrottle -= throttleHitDampning;
			currentThrottle = currentThrottle < 0 ? 0 : currentThrottle;
		}
		else
		{
			currentThrottle += throttleHitDampning;
			currentThrottle = currentThrottle > 0 ? 0 : currentThrottle;
		}
	}
}
