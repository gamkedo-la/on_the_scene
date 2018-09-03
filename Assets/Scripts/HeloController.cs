using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeloController : MonoBehaviour {

    public float rotationSpeed = 15.0f;
    public float tiltDriftEffect = 30.0f;
    private Rigidbody rb;
    public static HeloController instance;
    // Use this for initialization

    private void Awake()
    {
        instance = this;
    }

    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        
        Vector3 flattenedTilt = Vector3.forward * transform.up.z * tiltDriftEffect +
                                       Vector3.right * transform.up.x * tiltDriftEffect;
        flattenedTilt.y = 0.0f;
        if (Input.GetButton("Ascend")) {
            flattenedTilt.y += 1.0f;
        } else if (Input.GetButton("Descend")) {
            flattenedTilt.y -= 1.0f;
        }

        rb.velocity = flattenedTilt;
        transform.Rotate(Vector3.forward, Input.GetAxis("Horizontal") * Time.deltaTime * -rotationSpeed);
        transform.Rotate(Vector3.right, Input.GetAxis("Vertical") * Time.deltaTime * rotationSpeed);
        transform.Rotate(Vector3.up, Input.GetAxis("Swivel") * Time.deltaTime * rotationSpeed, Space.World);

    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            //transform.rotation = Quaternion.identity;
            Quaternion levelOrientation = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, levelOrientation, 0.05f);
        }
    }
}
