﻿using UnityEngine;
using UnityEngine.Assertions;

public class TutorialMessage : MonoBehaviour
{
	[SerializeField] private HeliControlsNonPhysics heli = null;

	void Start( )
	{
		Assert.IsNotNull( heli );

		heli.enabled = false;
	}
	void Update ()
	{
		if ( Input.anyKeyDown )
		{
			heli.enabled = true;
			Destroy( gameObject );
		}
	}
}
