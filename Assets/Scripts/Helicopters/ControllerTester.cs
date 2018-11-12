using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class ControllerTester : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI log;

	void Start ()
	{
		Assert.IsNotNull( log );
	}

	void Update ()
	{
		/*float throttle = Input.GetAxis( "Vertical" );

		float rollL = -Input.GetAxis( "RollLeft" );
		float rollR = Input.GetAxis( "RollRight" );
		float roll = rollL + rollR;

		float pitch = Input.GetAxis( "Pitch" );
		float yaw = Input.GetAxis( "Yaw" );

		string message = "Throttle: " + throttle + " Pitch: " + pitch + " Yaw: " + yaw + " Roll: " + roll;
		log.text = message;*/
		System.Text.StringBuilder s = new System.Text.StringBuilder( );
		s.Append( "Controller buttons pressed: " );

		for ( int i = 0; i < 20; i++ )
		{
			bool buttonPressed = Input.GetKey( "joystick button " + i );
			if ( buttonPressed )
				s.Append( "joystick button " + i + ", " );
		}

		log.text = s.ToString( );
	}
}
