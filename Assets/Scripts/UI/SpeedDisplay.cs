using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class SpeedDisplay : MonoBehaviour
{
	[SerializeField] private HeliControlsNonPhysics heli = null;
	[SerializeField] private TextMeshProUGUI label = null;

	void Start( )
	{
		Assert.IsNotNull( heli );
		Assert.IsNotNull( label );
	}

	void FixedUpdate( )
	{
		UpdateUI( );
	}

	private void UpdateUI( )
	{
		float velecity = heli.GetVelocity( );

		label.text = ( (int)( velecity * 3.6f ) ).ToString( ) + " km/h"; // m/s -> km/h
	}
}
