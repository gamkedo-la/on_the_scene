using UnityEngine;
using UnityEngine.Assertions;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private float smoothSpeed = 0.125f;
	[SerializeField] private Vector3 offset;

	private float lastDistance = 0;
	private float normalDistance = 0;
    private Transform cameraTransform;

    public void SetTarget() {
        target = HeliController.instance.GetCenterOfMass();
    }

	void Start( )
	{
        SetTarget();
		Assert.IsNotNull( target );

        cameraTransform = Camera.main.transform;

        cameraTransform.position = target.position + offset;
		normalDistance = Vector3.Distance( target.position, cameraTransform.position );
		lastDistance = normalDistance;
	}

	void FixedUpdate( )
	{
		Vector3 desiredPosition = target.position + ( target.rotation * offset );

		float speed = smoothSpeed;
		if ( Vector3.Distance( target.position, cameraTransform.position ) < lastDistance )
			speed = smoothSpeed * 8 * ( 1.1f - lastDistance / normalDistance );

		Vector3 smoothedPosition = Vector3.Lerp(cameraTransform.position, desiredPosition, speed );

        cameraTransform.position = smoothedPosition;
		lastDistance = Vector3.Distance( target.position, cameraTransform.position );

        cameraTransform.LookAt( target );
	}
}
