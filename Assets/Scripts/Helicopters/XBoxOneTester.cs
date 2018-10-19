using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class XBoxOneTester : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI log;
	[SerializeField] private TextMeshProUGUI logT;

	void Start ()
	{
		Assert.IsNotNull( log );
		Assert.IsNotNull( logT );
	}

	void Update ()
	{
		float throttle = Input.GetAxis( "Vertical" );

		float rollL = -Input.GetAxis( "RollLeft" );
		float rollR = Input.GetAxis( "RollRight" );
		float roll = rollL + rollR;

		float pitch = Input.GetAxis( "Pitch" );
		float yaw = Input.GetAxis( "Yaw" );

		string message = "Throttle: " + throttle + " Pitch: " + pitch + " Yaw: " + yaw + " Roll: " + roll;
		log.text = message;

		/*logT.text = "";
		for ( int i = 0; i < 20; i++ )
			logT.text += "Button " + i + "=" + Input.GetKey( "joystick button " + i ) + "| ";*/
	}
}
