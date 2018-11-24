using UnityEngine;
using UnityEngine.Assertions;

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
	[SerializeField] private float speedMaxAlt = 3.7f;
	[SerializeField] private AnimationCurve dragCurve = new AnimationCurve( new Keyframe( 0f, 0f ), new Keyframe( 1f, 0.1f ) );
	[SerializeField] private AnimationCurve liftCurve = new AnimationCurve( new Keyframe( 0f, 1f ), new Keyframe( 1f, 0f ) );
	[SerializeField] private AnimationCurve dragCurveAlt = new AnimationCurve( new Keyframe( 0f, 0f ), new Keyframe( 1f, 0.1f ) );
	[SerializeField] private AnimationCurve liftCurveAlt = new AnimationCurve( new Keyframe( 0f, 1f ), new Keyframe( 1f, 0f ) );
	[SerializeField] private bool useAltValues = true;

	[SerializeField] private float throttleHitDampning = 3f;

	private HeliInput input = null;
	private float currentVelocity = 0;
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
		input = GetComponent<HeliInput>( );
		Assert.IsNotNull( input );

		TakeInstance( );
	}

	void OnEnable( )
	{
		//Cursor.lockState = CursorLockMode.Locked;
	}

	void OnDisable( )
	{
		//Cursor.lockState = CursorLockMode.None;
	}

	void FixedUpdate( )
	{
		RotateAndMove( );
	}

	void OnCollisionStay( Collision collision )
	{
		if ( input.currentThrottle > 0 )
		{
			input.currentThrottle -= throttleHitDampning * 2 * Time.deltaTime;
			input.currentThrottle = input.currentThrottle < 0 ? 0 : input.currentThrottle;
		}
		else
		{
			input.currentThrottle += throttleHitDampning * 2 * Time.deltaTime;
			input.currentThrottle = input.currentThrottle > 0 ? 0 : input.currentThrottle;
		}
	}

	void OnCollisionEnter( Collision collision )
	{
		if ( input.currentThrottle > 0)
		{
			input.currentThrottle -= throttleHitDampning;
			input.currentThrottle = input.currentThrottle < 0 ? 0 : input.currentThrottle;
		}
		else
		{
			input.currentThrottle += throttleHitDampning;
			input.currentThrottle = input.currentThrottle > 0 ? 0 : input.currentThrottle;
		}
	}

	public void TakeInstance( )
	{
		instance = this;
	}

	public float GetPitchPercent( )
	{
		return input.currentPitch / input.maxPitch;
	}

	public float GetRollPercent( )
	{
		return input.currentRoll / input.maxRoll;
	}

	public float GetPitchDesieredPercent( )
	{
		return input.currentPitchDesired / input.maxPitch;
	}

	public float GetRollDesieredPercent( )
	{
		return input.currentRollDesired / input.maxRoll;
	}

	public float GetThrottlePercent( )
	{
		return input.currentThrottle / input.maxThrottle;
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

    public Transform GetCenterOfMass() {
        return centerOfMass;
    }

	private void RotateAndMove( )
	{
		float maximumSpeed = useAltValues ? speedMaxAlt : speedMax;
		// Velocity
		currentVelocity = Vector3.Distance( transform.position, lastPosition ) / Time.fixedDeltaTime;
		lastPosition = transform.position;

		// Heli rotation
		heliRigidbody.MoveRotation( Quaternion.Euler( input.currentPitch, input.currentYaw, input.currentRoll ) );
		float rollPercent = input.currentRoll / input.maxRoll;
		float pitchPercent = input.currentPitch / input.maxPitch;

		// Rotor animations
		float rotationControl = input.currentThrottle + input.maxThrottle + 5;
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

		float horizontalThrottle = input.currentThrottle;
		horizontalThrottle = horizontalThrottle < 0 ? 1 : horizontalThrottle; // Always give some speed
		horizontalThrottle = horizontalThrottle >= 0 && horizontalThrottle < 1 ? 1 : horizontalThrottle;

		moveVector = moveVector * ( maximumSpeed / worldScale ) * horizontalThrottle * Time.fixedDeltaTime;

		// Rotation
		moveVector = Quaternion.Euler( 0, heliRigidbody.rotation.eulerAngles.y, 0 ) * moveVector;

		// Ascend / descend
		if ( input.currentThrottle >= 0 )
		{
			// How much "speed" is used for horizontal movement?
			float horizontalPercent = Mathf.Abs( rollPercent ) >= Mathf.Abs( pitchPercent ) ?
				Mathf.Abs( rollPercent ) : Mathf.Abs( pitchPercent );

			// How much each "force" contributes to vertical movement?
			AnimationCurve drag = useAltValues ? dragCurveAlt : dragCurve;
			AnimationCurve lift = useAltValues ? liftCurveAlt : liftCurve;
			float downDrag = -input.maxThrottle * drag.Evaluate( horizontalPercent );
			float throttle = input.currentThrottle * lift.Evaluate( horizontalPercent );

			// Final vertical movement
			moveVector.y = ( downDrag + throttle ) * ( maximumSpeed / worldScale ) * Time.fixedDeltaTime;
		}
		else
		{
			moveVector.y = input.currentThrottle * ( maximumSpeed / worldScale ) * Time.fixedDeltaTime;
		}

		heliRigidbody.MovePosition( heliRigidbody.position + moveVector );
	}
}
