using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class TutorialMessageController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textToShow = null;

	void Start ()
	{
		Assert.IsNotNull( textToShow );
	}

	private void OnTriggerEnter( Collider other )
	{
		if ( !other.CompareTag( "Player" ) )
			return;

		textToShow.gameObject.SetActive( true );
	}

	private void OnTriggerExit( Collider other )
	{
		if ( !other.CompareTag( "Player" ) )
			return;

		textToShow.gameObject.SetActive( false );
	}
}
