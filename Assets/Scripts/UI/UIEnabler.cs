using UnityEngine;
using UnityEngine.Assertions;

public class UIEnabler : MonoBehaviour
{
	[SerializeField] private CanvasGroup ui = null;
	[SerializeField] private float showTime = 1f;

	private float timeRunning = 0;

	void Start ()
	{
		Assert.IsNotNull( ui );
		ui.alpha = 0;
	}

	void Update ()
	{
		timeRunning += Time.deltaTime;
		ui.alpha = timeRunning / showTime;

		if ( timeRunning > showTime )
			enabled = false;
	}
}
