using UnityEngine;
using UnityEngine.Assertions;

public class HeliControlsNonPhysics : MonoBehaviour
{
	[Header("Parts")]
	[SerializeField] private GameObject rotorMain = null;
	[SerializeField] private GameObject rotorTail = null;
	[SerializeField] private GameObject wholeHeli = null;
	[SerializeField] private Transform centerOfMass;
	[SerializeField] private Transform altitudePoint;

	[Header("Parameters")]
	[SerializeField] private float speedMax = 3f;
	[SerializeField] private AnimationCurve dragCurve = new AnimationCurve( new Keyframe( 0f, 0f ), new Keyframe( 1f, 0.1f ) );
	[SerializeField] private AnimationCurve liftCurve = new AnimationCurve( new Keyframe( 0f, 1f ), new Keyframe( 1f, 0f ) );

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

	[Header("Other")]
	[SerializeField] private float backToNeutralSpeed = 1.5f;
	[SerializeField] private float deadZone = 10f;

	private float currentThrottle = 0;
	private float currentRoll = 0;
	private float currentPitch = 0;
	private float currentPitchDesiered = 0;
	private float currentYaw = 0;
	private float currentYawDesiered = 0;
	private float currentVelocity = 0;
	private Rigidbody rb;
	private Vector3 lastPosition;

	void Start( )
	{
		Assert.IsNotNull( rotorMain );
		Assert.IsNotNull( rotorTail );
		Assert.IsNotNull( wholeHeli );
		Assert.IsNotNull( centerOfMass );
		Assert.IsNotNull( altitudePoint );

		rb = wholeHeli.GetComponent<Rigidbody>( );
		Assert.IsNotNull( rb );

		rb.centerOfMass = centerOfMass.localPosition;
		lastPosition = transform.position;
	}

	void OnEnable( )
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	void OnDisable( )
	{
		Cursor.lockState = CursorLockMode.None;
	}

	void Update( )
	{
		HandleControls( );
	}

	void FixedUpdate( )
	{
		RotateAndMove( );
	}

	public float GetPitchPercent()
	{
		return currentPitch / maxPitch;
	}

	public float GetYawPercent( )
	{
		return currentYaw / maxYaw;
	}

	public float GetPitchDesieredPercent( )
	{
		return currentPitchDesiered / maxPitch;
	}

	public float GetYawDesieredPercent( )
	{
		return currentYawDesiered / maxYaw;
	}

	public float GetThrottlePercent( )
	{
		return currentThrottle / maxThrottle;
	}

	public float GetVelocity( )
	{
		return currentVelocity;
	}

	public Transform GetAltitudePoint( )
	{
		return altitudePoint;
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

		// Pitch
		if ( Input.GetAxis( "Mouse Y" ) > 0.1f )
			currentPitchDesiered += pitchDesierdChangeSpeed * 1 * Time.deltaTime;
		else if ( Input.GetAxis( "Mouse Y" ) < -0.1f )
			currentPitchDesiered -= pitchDesierdChangeSpeed * 1 * Time.deltaTime;

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

		currentPitchDesiered = Mathf.Clamp( currentPitchDesiered, -maxPitch-deadZone, maxPitch+deadZone );
		float target = currentPitchDesiered < deadZone && currentPitchDesiered > -deadZone ? 0 : currentPitchDesiered; // Dead Zone
		currentPitch = Mathf.Lerp( currentPitch, target, pitchChangeSpeed * Time.deltaTime );

		currentYawDesiered = Mathf.Clamp( currentYawDesiered, -maxYaw, maxYaw );
		target = currentYawDesiered < deadZone && currentYawDesiered > -deadZone ? 0 : currentYawDesiered; // Dead Zone
		currentYaw = Mathf.Lerp( currentYaw, target, yawChangeSpeed * Time.deltaTime );
	}

	private void RotateAndMove( )
	{
		// Velocity
		currentVelocity = Vector3.Distance( transform.position, lastPosition ) / Time.fixedDeltaTime;
		lastPosition = transform.position;

		// Heli rotation
		rb.MoveRotation( Quaternion.Euler( currentPitch, 0, currentYaw ) );
		float yawPercent = currentYaw / maxYaw;
		float pitchPercent = currentPitch / maxPitch;

		// Rotor animations
		float rotationControl = currentThrottle + maxThrottle + 5;
		rotationControl *= 90 * Time.fixedDeltaTime;
		rotorMain.transform.Rotate( Vector3.up, rotationControl );
		rotorTail.transform.Rotate( Vector3.right, rotationControl );

		// Move
		Vector3 moveVector = Vector3.zero;

		// Starting Yaw ans Pitch
		moveVector.x = -yawPercent;
		moveVector.z = pitchPercent;
		moveVector = moveVector.normalized;
		moveVector = Mathf.Abs( yawPercent ) > Mathf.Abs( pitchPercent ) ?
			moveVector * Mathf.Abs( yawPercent ) : moveVector * Mathf.Abs( pitchPercent );

		float horizontalThrottle = currentThrottle;
		horizontalThrottle = horizontalThrottle < 0 ? 1 : horizontalThrottle; // Always give some speed
		horizontalThrottle = horizontalThrottle >= 0 && horizontalThrottle < 1 ? 1 : horizontalThrottle;
		//moveVector = moveVector.magnitude > speedMax ? moveVector.normalized * speedMax : moveVector;
		moveVector = moveVector * speedMax * horizontalThrottle * Time.fixedDeltaTime;

		// Ascend / descend
		if ( currentThrottle >= 0 )
		{
			// How much "speed" is used for horizontal movement?
			float horizontalPercent = Mathf.Abs( yawPercent ) >= Mathf.Abs( pitchPercent ) ?
				Mathf.Abs( yawPercent ) : Mathf.Abs( pitchPercent );

			// How much each "force" contributes to vertical movement?
			float downDrag = -maxThrottle * dragCurve.Evaluate( horizontalPercent );
			float throttle = currentThrottle * liftCurve.Evaluate( horizontalPercent );

			// Final vertical movement
			moveVector.y = ( downDrag + throttle ) * speedMax * Time.fixedDeltaTime;

		}
		else
		{
			moveVector.y = currentThrottle * speedMax * Time.fixedDeltaTime;
		}

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
