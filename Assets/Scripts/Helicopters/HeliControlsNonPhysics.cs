using UnityEngine;
using UnityEngine.Assertions;

public class HeliControlsNonPhysics : MonoBehaviour
{
	[Header("Parts")]
	[SerializeField] private GameObject rotorMain = null;
	[SerializeField] private GameObject rotorTail = null;
	[SerializeField] private Rigidbody heliRigidbody = null;
	[SerializeField] private Transform centerOfMass = null;
	[SerializeField] private Transform altitudePoint = null;

	[Header("Parameters")]
	[SerializeField] private float speedMax = 6f;
	[SerializeField] private AnimationCurve dragCurve = new AnimationCurve( new Keyframe( 0f, 0f ), new Keyframe( 1f, 0.1f ) );
	[SerializeField] private AnimationCurve liftCurve = new AnimationCurve( new Keyframe( 0f, 1f ), new Keyframe( 1f, 0f ) );

	[Header("Throttle")]
	[SerializeField] private float throttleChangeSpeed = 3f;
	[SerializeField] private float throttleBackChangeSpeed = 6f;
	[SerializeField] private float throttleHitDampning = 3f;
	[SerializeField] private float maxThrottle = 10f;

	[Header("Pitch")]
	[SerializeField] private float pitchDesierdChangeSpeed = 50f;
	[SerializeField] private float pitchChangeSpeed = 0.7f;
	[SerializeField] private float maxPitch = 45f;

	[Header("Roll")]
	[SerializeField] private float rollDesierdChangeSpeed = 50f;
	[SerializeField] private float rollChangeSpeed = 0.7f;
	[SerializeField] private float maxRoll = 45f;

	[Header("Yaw")]
	[SerializeField] private float yawChangeSpeed = 3f;

	[Header("Other")]
	[SerializeField] private float deadZone = 10f;

	private float currentThrottle = 0;
	private float currentYaw = 0;
	private float currentPitch = 0;
	private float currentPitchDesiered = 0;
	private float currentRoll = 0;
	private float currentRollDesiered = 0;
	private float currentVelocity = 0;
	private float rollControllerLast = 0;
	private float pitchControllerLast = 0;
	private Vector3 lastPosition;

	void Start( )
	{
		Assert.IsNotNull( rotorMain );
		Assert.IsNotNull( rotorTail );
		Assert.IsNotNull( heliRigidbody );
		Assert.IsNotNull( centerOfMass );
		Assert.IsNotNull( altitudePoint );

		heliRigidbody.centerOfMass = centerOfMass.localPosition;
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

	public float GetRollPercent( )
	{
		return currentRoll / maxRoll;
	}

	public float GetPitchDesieredPercent( )
	{
		return currentPitchDesiered / maxPitch;
	}

	public float GetRollDesieredPercent( )
	{
		return currentRollDesiered / maxRoll;
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
		float throttleController = -Input.GetAxis( "Throttle" );
		if (throttleController != 0 )
		{
			currentThrottle += throttleController * throttleChangeSpeed * Time.deltaTime;
			currentThrottle = Mathf.Clamp
			(
				currentThrottle,
				-maxThrottle * Mathf.Abs( throttleController ),
				maxThrottle * Mathf.Abs( throttleController )
			);
		}

		float throttleKeyboard = Input.GetAxis( "Vertical" );
		if ( throttleKeyboard != 0 )
		{
			currentThrottle += throttleKeyboard * throttleChangeSpeed * Time.deltaTime;
		}
		else if ( throttleController == 0 )
		{
			if ( currentThrottle > 0.1f )
				currentThrottle -= throttleBackChangeSpeed * Time.deltaTime;
			else if ( currentThrottle < -0.1f )
				currentThrottle += throttleBackChangeSpeed * Time.deltaTime;
			else
				currentThrottle = 0;
		}

		currentThrottle = Mathf.Clamp( currentThrottle, -maxThrottle, maxThrottle );

		// Roll
		float rollController = -Input.GetAxis( "Roll" );

		if ( Input.GetAxis( "Mouse X" ) > 0.1f )
			currentRollDesiered -= rollDesierdChangeSpeed * Time.deltaTime;
		else if ( Input.GetAxis( "Mouse X" ) < -0.1f )
			currentRollDesiered += rollDesierdChangeSpeed * Time.deltaTime;
		else if ( rollController != 0 )
			currentRollDesiered = rollController * maxRoll;
		else if ( rollController == 0 && rollController != rollControllerLast )
			currentRollDesiered = 0;

		currentRollDesiered = Mathf.Clamp( currentRollDesiered, -maxRoll, maxRoll );
		float target = currentRollDesiered < deadZone && currentRollDesiered > -deadZone ? 0 : currentRollDesiered; // Dead Zone
		currentRoll = Mathf.Lerp( currentRoll, target, rollChangeSpeed * Time.deltaTime );
		rollControllerLast = rollController;

		// Pitch
		float pitchController = -Input.GetAxis( "Pitch" );

		if ( Input.GetAxis( "Mouse Y" ) > 0.1f )
			currentPitchDesiered += pitchDesierdChangeSpeed * Time.deltaTime;
		else if ( Input.GetAxis( "Mouse Y" ) < -0.1f )
			currentPitchDesiered -= pitchDesierdChangeSpeed * Time.deltaTime;
		else if ( pitchController != 0 )
			currentPitchDesiered = pitchController * maxPitch;
		else if ( pitchController == 0 && pitchController != pitchControllerLast )
			currentPitchDesiered = 0;

		currentPitchDesiered = Mathf.Clamp( currentPitchDesiered, -maxPitch-deadZone, maxPitch+deadZone );
		target = currentPitchDesiered < deadZone && currentPitchDesiered > -deadZone ? 0 : currentPitchDesiered; // Dead Zone
		currentPitch = Mathf.Lerp( currentPitch, target, pitchChangeSpeed * Time.deltaTime );
		pitchControllerLast = pitchController;

		// Yaw
		float yawL = -Input.GetAxis( "YawLeft" );
		float yawR = Input.GetAxis( "YawRight" );
		float yawController = yawL + yawR;
		float yawKeyboard = Input.GetAxis( "Horizontal" );
		float yaw = yawController + yawKeyboard;
		yaw = Mathf.Clamp( yaw, -1, 1 );

		currentYaw += yaw * yawChangeSpeed * Time.deltaTime;
		currentYaw = currentYaw > 360 ? currentYaw - 360 : currentYaw; // So it's not bigger then 360*
		currentYaw = currentYaw < -360 ? currentYaw + 360 : currentYaw; // So it's not smaller then -360*
	}

	private void RotateAndMove( )
	{
		// Velocity
		currentVelocity = Vector3.Distance( transform.position, lastPosition ) / Time.fixedDeltaTime;
		lastPosition = transform.position;

		// Heli rotation
		heliRigidbody.MoveRotation( Quaternion.Euler( currentPitch, currentYaw, currentRoll ) );
		float rollPercent = currentRoll / maxRoll;
		float pitchPercent = currentPitch / maxPitch;

		// Rotor animations
		float rotationControl = currentThrottle + maxThrottle + 5;
		rotationControl *= 90 * Time.fixedDeltaTime;
		rotorMain.transform.Rotate( Vector3.up, rotationControl );
		rotorTail.transform.Rotate( Vector3.right, rotationControl );

		// Move
		Vector3 moveVector = Vector3.zero;

		// Starting Roll and Pitch
		moveVector.x = -rollPercent;
		moveVector.z = pitchPercent;
		moveVector = moveVector.normalized;
		moveVector = Mathf.Abs( rollPercent ) > Mathf.Abs( pitchPercent ) ?
			moveVector * Mathf.Abs( rollPercent ) : moveVector * Mathf.Abs( pitchPercent );

		float horizontalThrottle = currentThrottle;
		horizontalThrottle = horizontalThrottle < 0 ? 1 : horizontalThrottle; // Always give some speed
		horizontalThrottle = horizontalThrottle >= 0 && horizontalThrottle < 1 ? 1 : horizontalThrottle;

		moveVector = moveVector * speedMax * horizontalThrottle * Time.fixedDeltaTime;

		// Rotation
		moveVector = Quaternion.Euler( 0, heliRigidbody.rotation.eulerAngles.y, 0 ) * moveVector;

		// Ascend / descend
		if ( currentThrottle >= 0 )
		{
			// How much "speed" is used for horizontal movement?
			float horizontalPercent = Mathf.Abs( rollPercent ) >= Mathf.Abs( pitchPercent ) ?
				Mathf.Abs( rollPercent ) : Mathf.Abs( pitchPercent );

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

		heliRigidbody.MovePosition( heliRigidbody.position + moveVector );
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
