using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialEnd : MonoBehaviour
{
	public HeliStartAndLanding heli = null;
	private bool isHere = false;

	void Update( )
	{
		if ( isHere && heli.GetEngineState( ) == HeliStartAndLanding.EngineState.CanBeTurnedOn )
			Invoke( "ChangeLevel", 2f );
	}

	private void OnTriggerEnter( Collider other )
	{
		if ( other.gameObject.CompareTag( "Player" ) )
			isHere = true;
	}

	private void OnTriggerExit( Collider other )
	{
		if ( other.gameObject.CompareTag( "Player" ) )
			isHere = false;
	}

	private void ChangeLevel()
	{
		SceneManager.LoadScene( "MainMenu" );
	}
}
