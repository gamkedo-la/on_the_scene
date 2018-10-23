using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class AltitudeDisplay : MonoBehaviour
{
	[SerializeField] private HeliController heli = null;
	[SerializeField] private TextMeshProUGUI altitudeToSeaLabel = null;
	[SerializeField] private TextMeshProUGUI altitudeToGroundLabel = null;

	private Transform altitudePoint = null;
	private float worldScale = 1f;
	private RaycastHit hit;

	void Start( )
	{
		Assert.IsNotNull( heli );
		Assert.IsNotNull( altitudeToSeaLabel );
		Assert.IsNotNull( altitudeToGroundLabel );

		altitudePoint = heli.GetAltitudePoint( );
		worldScale = heli.GetWorldScale( );
		Assert.IsNotNull( altitudePoint );
	}

	void FixedUpdate( )
	{
		UpdateUI( );
	}

	private void UpdateUI( )
	{
		Physics.Raycast( altitudePoint.position, Vector3.down, out hit );

		altitudeToSeaLabel.text = ( (int)( altitudePoint.position.y * worldScale ) ).ToString( ) + " m";
		altitudeToGroundLabel.text = ( (int)( hit.distance * worldScale ) ).ToString( ) + " m";
	}
}
