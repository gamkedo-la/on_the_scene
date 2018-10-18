using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class AltitudeDisplay : MonoBehaviour
{
	[SerializeField] private HeliControlsNonPhysics heli = null;
	[SerializeField] private TextMeshProUGUI altitudeToSeaLabel = null;
	[SerializeField] private TextMeshProUGUI altitudeToGroundLabel = null;

	private Transform altitudePoint = null;
	private RaycastHit hit;

	void Start( )
	{
		Assert.IsNotNull( heli );
		Assert.IsNotNull( altitudeToSeaLabel );
		Assert.IsNotNull( altitudeToGroundLabel );

		altitudePoint = heli.GetAltitudePoint( );
		Assert.IsNotNull( altitudePoint );
	}

	void FixedUpdate( )
	{
		UpdateUI( );
	}

	private void UpdateUI( )
	{
		Physics.Raycast( altitudePoint.position, Vector3.down, out hit );

		altitudeToSeaLabel.text = ( (int)( altitudePoint.position.y ) ).ToString( ) + " m";
		altitudeToGroundLabel.text = ( (int)( hit.distance ) ).ToString( ) + " m";
	}
}
