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
	[SerializeField] private float speedMax = 3f;
	[SerializeField] private AnimationCurve dragCurve = new AnimationCurve( new Keyframe( 0f, 0f ), new Keyframe( 1f, 0.1f ) );
	[SerializeField] private AnimationCurve liftCurve = new AnimationCurve( new Keyframe( 0f, 1f ), new Keyframe( 1f, 0f ) );

	[Header("Throttle")]
	[SerializeField] private float throttleChangeSpeed = 3f;
	[SerializeField] private float throttleBackChangeSpeed = 6f;
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
	private float yawControllerLast = 0;
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

		// Yaw
		float yawController = -Input.GetAxis( "Yaw" );

		if ( Input.GetAxis( "Mouse X" ) > 0.1f )
			currentYawDesiered -= yawDesierdChangeSpeed * Time.deltaTime;
		else if ( Input.GetAxis( "Mouse X" ) < -0.1f )
			currentYawDesiered += yawDesierdChangeSpeed * Time.deltaTime;
		else if ( yawController != 0 )
			currentYawDesiered = yawController * maxYaw;
		else if ( yawController == 0 && yawController != yawControllerLast )
			currentYawDesiered = 0;

		currentYawDesiered = Mathf.Clamp( currentYawDesiered, -maxYaw, maxYaw );
		float target = currentYawDesiered < deadZone && currentYawDesiered > -deadZone ? 0 : currentYawDesiered; // Dead Zone
		currentYaw = Mathf.Lerp( currentYaw, target, yawChangeSpeed * Time.deltaTime );
		yawControllerLast = yawController;

		// Pitch
		float pitchController = -Input.GetAxis( "Pitch" );

		if ( Input.GetAxis( "Mouse Y" ) > 0.1f )
			currentPitchDesiered += pitchDesierdChangeSpeed * Time.deltaTime;
		else if ( Input.GetAxis( "Mouse Y" ) < -0.1f )
			currentPitchDesiered -= pitchDesierdChangeSpeed * Time.deltaTime;
		else if ( pitchController != 0 )
			currentPitchDesiered = pitchController * maxYaw;
		else if ( pitchController == 0 && pitchController != pitchControllerLast )
			currentPitchDesiered = 0;

		currentPitchDesiered = Mathf.Clamp( currentPitchDesiered, -maxPitch-deadZone, maxPitch+deadZone );
		target = currentPitchDesiered < deadZone && currentPitchDesiered > -deadZone ? 0 : currentPitchDesiered; // Dead Zone
		currentPitch = Mathf.Lerp( currentPitch, target, pitchChangeSpeed * Time.deltaTime );
		pitchControllerLast = pitchController;

		// Roll
		float rollL = -Input.GetAxis( "RollLeft" );
		float rollR = Input.GetAxis( "RollRight" );
		float rollController = rollL + rollR;
		float rollKeyboard = Input.GetAxis( "Horizontal" );
		float roll = rollController + rollKeyboard;
		roll = Mathf.Clamp( roll, -1, 1 );

		currentRoll += roll * rollChangeSpeed * Time.deltaTime;
		currentRoll = currentRoll > 360 ? currentRoll - 360 : currentRoll; // So it's not bigger then 360*
		currentRoll = currentRoll < -360 ? currentRoll + 360 : currentRoll; // So it's not smaller then -360*
	}

	private void RotateAndMove( )
	{
		// Velocity
		currentVelocity = Vector3.Distance( transform.position, lastPosition ) / Time.fixedDeltaTime;
		lastPosition = transform.position;

		// Heli rotation
		heliRigidbody.MoveRotation( Quaternion.Euler( currentPitch, currentRoll, currentYaw ) );
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

		moveVector = moveVector * speedMax * horizontalThrottle * Time.fixedDeltaTime;

		// Rotation
		moveVector = Quaternion.Euler( 0, heliRigidbody.rotation.eulerAngles.y, 0 ) * moveVector;

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
