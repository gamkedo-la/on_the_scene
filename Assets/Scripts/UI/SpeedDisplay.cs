﻿using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class SpeedDisplay : MonoBehaviour
{
	[SerializeField] private HeliController heli = null;
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
		float velocity = heli.GetVelocity( );

		label.text = ( (int)( velocity * 3.6f ) ).ToString( ) + " km/h"; // m/s -> km/h
	}
}
