using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLights : MonoBehaviour
{
	public Light[] Lights = null;
	public float changeTime = 2f;

	private int lastIndex = -1;

	void Start ()
	{
		Invoke( "ChangeLights", 0.1f );
	}

	private void ChangeLights()
	{
		lastIndex++;

		if ( lastIndex >= Lights.Length )
			lastIndex = 0;

		foreach ( var light in Lights )
		{
			light.enabled = false;
		}

		Lights[lastIndex].enabled = true;

		lastIndex++;

		if ( lastIndex >= Lights.Length )
			lastIndex = 0;

		Lights[lastIndex].enabled = true;

		Invoke( "ChangeLights", changeTime );
	}
}
