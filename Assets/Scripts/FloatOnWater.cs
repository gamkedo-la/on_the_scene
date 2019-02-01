using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatOnWater : MonoBehaviour {
    public Transform waterLevel;
    Rigidbody rb;
    public float waterLevelY;
    public float MaxHeight = 2.0f;
    public float bounceDamp = 0.05f;
    public Vector3 bouyancyCenterOffset;
    private float forceFactor;
    private Vector3 actionPoint;
    private Vector3 upLift;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        waterLevelY = waterLevel.position.y;
    }

    // Update is called once per frame
    void Update () {
        actionPoint = transform.position + transform.TransformDirection(bouyancyCenterOffset);
        forceFactor = 1.0f - ((actionPoint.y - waterLevelY) / MaxHeight);

        if (forceFactor > 0f)
        {
            upLift = -Physics.gravity * (forceFactor - rb.velocity.y * bounceDamp);

            rb.AddForceAtPosition(upLift, actionPoint);
            
        }
    }
}
