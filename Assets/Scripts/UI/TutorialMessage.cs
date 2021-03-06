﻿using UnityEngine;
using UnityEngine.Assertions;

public class TutorialMessage : MonoBehaviour
{
	[SerializeField] private HeliController heli = null;

	void Start( )
	{
		Assert.IsNotNull( heli );

		heli.enabled = false;
	}
	void Update ()
	{
		if ( Input.GetKeyDown( KeyCode.Escape ) )
		{
			heli.enabled = true;
			Destroy( gameObject );
		}
	}
}
