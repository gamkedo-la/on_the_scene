﻿using UnityEngine;
using UnityEngine.Assertions;

enum ConnectedController
{
	None,
	XBox,
	PS4orOther,
}

public class HeliInput : MonoBehaviour
{
	[HideInInspector] public float currentThrottle = 0;
	[HideInInspector] public float currentYaw = 0;
	[HideInInspector] public float currentPitch = 0;
	[HideInInspector] public float currentRoll = 0;
	[HideInInspector] public float currentPitchDesired = 0;
	[HideInInspector] public float currentRollDesired = 0;

	[Header("Throttle")]
	[SerializeField] private float throttleChangeSpeed = 3f;
	[SerializeField] private float throttleBackChangeSpeed = 6f;
	[SerializeField] public float maxThrottle = 10f;

	[Header("Pitch")]
	[SerializeField] private float pitchDesiredChangeSpeed = 100f;
	[SerializeField] private float pitchChangeSpeed = 1f;
	[SerializeField] public float maxPitch = 45f;

	[Header("Roll")]
	[SerializeField] private float rollDesiredChangeSpeed = 100f;
	[SerializeField] private float rollChangeSpeed = 1f;
	[SerializeField] public float maxRoll = 45f;

	[Header("Yaw")]
	[SerializeField] private float yawChangeSpeed = 30f;

	[Header("Other")]
	[SerializeField] private float deadZone = 10f;

	private float rollControllerLast = 0;
	private float pitchControllerLast = 0;

	private ConnectedController connectedController = ConnectedController.None;

	void Start ()
	{
		//Assert.IsNotNull(  );
	}

	void Update( )
	{
		HandleThrottleControls( );
		HandleRollControls( );
		HandlePitchControls( );
		HandleYawControls( );
	}

	void FixedUpdate( )
	{
		CheckConnectedControllers( ); // Or do we only check on start?
	}

	private void HandleThrottleControls( )
	{
		float throttleController = -Input.GetAxis( "Throttle" );
		if ( throttleController != 0 )
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
	}

	private void HandleRollControls( )
	{
		float rollController = 0;

		if ( connectedController == ConnectedController.XBox )
		{
			rollController = -Input.GetAxis( "Roll" );
		}
		else
		{
			rollController = -Input.GetAxis( "RollPS4" );
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
	}

	private void HandlePitchControls( )
	{
		float pitchController = 0;

		if ( connectedController == ConnectedController.XBox )
		{
			pitchController = -Input.GetAxis( "Pitch" );
		}
		else
		{
			pitchController = -Input.GetAxis( "PitchPS4" );
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

		currentPitchDesired = Mathf.Clamp( currentPitchDesired, -maxPitch - deadZone, maxPitch + deadZone );
		float target = currentPitchDesired < deadZone && currentPitchDesired > -deadZone ? 0 : currentPitchDesired; // Dead Zone
		currentPitch = Mathf.Lerp( currentPitch, target, pitchChangeSpeed * Time.deltaTime );
		pitchControllerLast = pitchController;
	}

	private void HandleYawControls( )
	{
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

	private void CheckConnectedControllers( )
	{
		string[] controllers = Input.GetJoystickNames( );

		connectedController = ConnectedController.None;
		for ( int i = 0; i < controllers.Length; i++ ) // Is there at least one?
		{
			if ( controllers[i].Length > 0 )
				connectedController = ConnectedController.PS4orOther; // We got a controller (PS4 or other)

			if ( controllers[i].ToLower( ).Contains( "xbox" ) ) // Is it an XBox controller?
			{
				connectedController = ConnectedController.XBox;
				return;
			}
		}
	}
}
