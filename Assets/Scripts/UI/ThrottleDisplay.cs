using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class ThrottleDisplay : MonoBehaviour
{
	[SerializeField] private HeliControlsNonPhysics heli = null;
	[SerializeField] private RectTransform throttlDot = null;
	[SerializeField] private TextMeshProUGUI label = null;
	[SerializeField] private float maxDelta = 93f;

	private Vector2 dotStartPos;

	void Start( )
	{
		Assert.IsNotNull( heli );
		Assert.IsNotNull( throttlDot );
		Assert.IsNotNull( label );

		dotStartPos = throttlDot.localPosition;
	}

	void FixedUpdate( )
	{
		UpdateUI( );
	}

	private void UpdateUI( )
	{
		float throttle = heli.GetThrottlePercent( );

		throttlDot.localPosition = new Vector2
		(
			dotStartPos.x,
			dotStartPos.y + throttle * maxDelta
		);

		label.text = ( (int)( throttle * 50 + 50) ).ToString( ) + "%";
	}
}
