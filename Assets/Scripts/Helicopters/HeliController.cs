﻿using UnityEngine;
using UnityEngine.Assertions;

enum ConnectedController
{
	None,
	XBox,
	PS4orOther,
}

public class HeliController : MonoBehaviour
{
	[HideInInspector] public static HeliController instance;

	[Header("World Scale")]
	[Tooltip("If the everything is scaled to 0.1f then put 10 - as in the world is 10x smaller. Used to give real world values for UI and stuff.")]
	[SerializeField] private float worldScale = 1f;

	[Header("Parts")]
	[SerializeField, Tooltip("The main rotor.")] private GameObject rotorMain = null;
	[SerializeField, Tooltip("The tail rotor.")] private GameObject rotorTail = null;
	[SerializeField, Tooltip("Rigidbody attached to the root of the helicopter.")] private Rigidbody heliRigidbody = null;
	[SerializeField, Tooltip("Point that acts a the center of the helicopter.")] private Transform centerOfMass = null;
	[SerializeField, Tooltip("Lowest point of the helicopter.")] private Transform altitudePoint = null;

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
	[SerializeField] private float pitchDesiredChangeSpeed = 100f;
	[SerializeField] private float pitchChangeSpeed = 0.3f;
	[SerializeField] private float maxPitch = 45f;

	[Header("Roll")]
	[SerializeField] private float rollDesiredChangeSpeed = 100f;
	[SerializeField] private float rollChangeSpeed = 0.3f;
	[SerializeField] private float maxRoll = 45f;

	[Header("Yaw")]
	[SerializeField] private float yawChangeSpeed = 30f;

	[Header("Other")]
	[SerializeField] private float deadZone = 10f;

	private float currentThrottle = 0;
	private float currentYaw = 0;
	private float currentPitch = 0;
	private float currentPitchDesired = 0;
	private float currentRoll = 0;
	private float currentRollDesired = 0;
	private float currentVelocity = 0;
	private float rollControllerLast = 0;
	private float pitchControllerLast = 0;
	private ConnectedController connectedController = ConnectedController.None;
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

	void Awake( )
	{
		TakeInstance( );
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
		CheckConnectedControllers( ); // Or do we only check on start?
		RotateAndMove( );
	}

	void OnCollisionStay( Collision collision )
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

	void OnCollisionEnter( Collision collision )
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

	public void TakeInstance( )
	{
		instance = this;
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
		return currentPitchDesired / maxPitch;
	}

	public float GetRollDesieredPercent( )
	{
		return currentRollDesired / maxRoll;
	}

	public float GetThrottlePercent( )
	{
		return currentThrottle / maxThrottle;
	}

	public float GetVelocity( )
	{
		return currentVelocity * worldScale;
	}

	public Transform GetAltitudePoint( )
	{
		return altitudePoint;
	}

	public float GetWorldScale( )
	{
		return worldScale;
	}

	private void CheckConnectedControllers()
	{
		string[] controllers = Input.GetJoystickNames( );

		connectedController = ConnectedController.None;
		for ( int i = 0; i < controllers.Length; i++ ) // Is there at least one?
		{
			if ( controllers[i].Length > 0)
				connectedController = ConnectedController.PS4orOther; // We got a controller (PS4 or other)

			if ( controllers[i].ToLower( ).Contains( "xbox" ) ) // Is it an XBox controller?
			{
				connectedController = ConnectedController.XBox;
				return;
			}
		}
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
		float rollController = 0;

		if ( connectedController == ConnectedController.XBox )
        {
            rollController = -Input.GetAxis("Roll");
        }
        else
        {
            rollController = -Input.GetAxis("RollPS4");
        }

        if ( Input.GetAxis( "Mouse X" ) > 0.1f )
        {
            currentRollDesired -= rollDesiredChangeSpeed * Time.deltaTime;
        }
        else if ( Input.GetAxis( "Mouse X" ) < -0.1f )
        {
            currentRollDesired += rollDesiredChangeSpeed * Time.deltaTime;
        }
		else if ( rollController != 0 )
        {
            currentRollDesired = rollController * maxRoll;
        }
		else if ( rollController == 0 && rollController != rollControllerLast )
        {
            currentRollDesired = 0;
        }

		currentRollDesired = Mathf.Clamp( currentRollDesired, -maxRoll, maxRoll );
		float target = currentRollDesired < deadZone && currentRollDesired > -deadZone ? 0 : currentRollDesired; // Dead Zone
		currentRoll = Mathf.Lerp( currentRoll, target, rollChangeSpeed * Time.deltaTime );
		rollControllerLast = rollController;

		// Pitch
		float pitchController = 0;

		if ( connectedController == ConnectedController.XBox )
        {
            pitchController = -Input.GetAxis("Pitch");
        }
        else {
            pitchController = -Input.GetAxis("PitchPS4");
        }

        if ( Input.GetAxis( "Mouse Y" ) > 0.1f )
        {
            currentPitchDesired += pitchDesiredChangeSpeed * Time.deltaTime;
        }
        else if ( Input.GetAxis( "Mouse Y" ) < -0.1f )
        {
            currentPitchDesired -= pitchDesiredChangeSpeed * Time.deltaTime;
        }
		else if ( pitchController != 0 )
        {
            currentPitchDesired = pitchController * maxPitch;
        }
		else if ( pitchController == 0 && pitchController != pitchControllerLast )
        {
            currentPitchDesired = 0;
        }

		currentPitchDesired = Mathf.Clamp( currentPitchDesired, -maxPitch-deadZone, maxPitch+deadZone );
		target = currentPitchDesired < deadZone && currentPitchDesired > -deadZone ? 0 : currentPitchDesired; // Dead Zone
		currentPitch = Mathf.Lerp( currentPitch, target, pitchChangeSpeed * Time.deltaTime );
		pitchControllerLast = pitchController;

		// Yaw
		float yawL = 0;
		float yawR = 0;

		if ( connectedController == ConnectedController.XBox )
		{
			yawL = -Input.GetAxis( "YawLeft" );
			yawR = Input.GetAxis( "YawRight" );
		}
		else
		{
			yawL = 0; // Jeremy, please add Yaw Input.GetAxis or Buttons for PS4
			yawR = 0;
		}

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

		moveVector = moveVector * ( speedMax / worldScale ) * horizontalThrottle * Time.fixedDeltaTime;

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
			moveVector.y = ( downDrag + throttle ) * ( speedMax / worldScale ) * Time.fixedDeltaTime;
		}
		else
		{
			moveVector.y = currentThrottle * ( speedMax / worldScale ) * Time.fixedDeltaTime;
		}

		heliRigidbody.MovePosition( heliRigidbody.position + moveVector );
	}
}
