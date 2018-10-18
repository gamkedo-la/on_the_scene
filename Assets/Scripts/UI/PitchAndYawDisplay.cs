using UnityEngine;
using UnityEngine.Assertions;

public class PitchAndYawDisplay : MonoBehaviour
{
	[SerializeField] private HeliControlsNonPhysics heli = null;
	[SerializeField] private RectTransform pitchAndYawDot = null;
	[SerializeField] private RectTransform pitchAndYawDotWhite = null;
	[SerializeField] private float maxDelta = 44f;

	private Vector2 pitchAndYawDotStartPos;

	void Start ()
	{
		Assert.IsNotNull( heli );
		Assert.IsNotNull( pitchAndYawDot );
		Assert.IsNotNull( pitchAndYawDotWhite );

		pitchAndYawDotStartPos = pitchAndYawDot.localPosition;
	}

	void FixedUpdate ()
	{
		UpdateUI( );
	}

	private void UpdateUI( )
	{
		float x = -heli.GetYawPercent( );
		float y = heli.GetPitchPercent( );
		pitchAndYawDot.localPosition = new Vector2
		(
			pitchAndYawDotStartPos.x + x * maxDelta,
			pitchAndYawDotStartPos.y + y * maxDelta
		);

		x = -heli.GetYawDesieredPercent( );
		y = heli.GetPitchDesieredPercent( );
		pitchAndYawDotWhite.localPosition = new Vector2
		(
			pitchAndYawDotStartPos.x + x * maxDelta,
			pitchAndYawDotStartPos.y + y * maxDelta
		);
	}
}
