using UnityEngine;
using UnityEngine.Assertions;

public class SpeedParticles : MonoBehaviour
{
	[SerializeField] private HeliController heli = null;
	[SerializeField] private ParticleSystemRenderer particles = null;
	[SerializeField] private float strenght = 100.0f;
	[SerializeField] private AnimationCurve efffectStrenght = new AnimationCurve(new Keyframe(0f,0f), new Keyframe(0.3f, 0f), new Keyframe(1.3f, 1f));

	void Start( )
	{
		Assert.IsNotNull( heli );
		Assert.IsNotNull( particles );
	}

	void Update( )
	{
		UpdateParticles( );
	}

	private void UpdateParticles( )
	{
		Vector3 velocity = heli.GetVelocityVector( );
		float magnitude = velocity.magnitude;

		if (magnitude > 0)
			transform.rotation = Quaternion.LookRotation( velocity );

		if ( magnitude > 0.3f )
			particles.lengthScale = strenght * efffectStrenght.Evaluate( magnitude );
		else
			particles.lengthScale = 0;
	}
}
