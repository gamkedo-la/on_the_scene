using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeloController : MonoBehaviour
{

    public HelicopterType helicopterType;
    public float rotationSpeed = 15.0f;
    public float tiltDriftEffect = 30.0f;
    public float liftSpeed = 1.0f;
    private Rigidbody rb;
    public static HeloController instance;
    private float uprightTiltFactor = 0.15f;
    // Use this for initialization

    private void Awake()
    {
        TakeInstance();
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void TakeInstance() {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 flattenedTilt = Vector3.forward * transform.up.z * tiltDriftEffect +
                                       Vector3.right * transform.up.x * tiltDriftEffect;
        flattenedTilt.y = 0.0f;
        if (Input.GetButton("Ascend"))
        {
            flattenedTilt.y += liftSpeed;
        }
        else if (Input.GetButton("Descend"))
        {
            flattenedTilt.y -= liftSpeed;
        }

        rb.velocity = flattenedTilt;
        transform.Rotate(Vector3.forward, Input.GetAxis("Horizontal") * Time.deltaTime * -rotationSpeed);
        transform.Rotate(Vector3.right, Input.GetAxis("Vertical") * Time.deltaTime * rotationSpeed);
        transform.Rotate(Vector3.up, Input.GetAxis("Swivel") * Time.deltaTime * rotationSpeed, Space.World);

        float referenceFramerate = 30.0f;
        float blend = 1.0f - Mathf.Pow(1.0f - uprightTiltFactor, Time.deltaTime * referenceFramerate);

        if (Input.GetKeyDown(KeyCode.P)) {
            WorldState.instance.isDay = !WorldState.instance.isDay;
        }

        Quaternion levelOrientation = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up);
        if (Input.GetButton("Fire1"))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, levelOrientation, blend);
        }

        /*if (transform.up.y < 0.7f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, levelOrientation, uprightTiltFactor * 0.6f);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, levelOrientation, uprightTiltFactor * 0.3f);
        }*/
        transform.rotation = Quaternion.Slerp(transform.rotation, levelOrientation, blend * 0.3f);
    }
}

public enum HelicopterType {
    Transport,
    Rescue,
    HighSpeed
}