using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]

public class RopeScript : MonoBehaviour
{
    // physics rope using a line renderer
    // based on code by Jacob Fletcher and Chelsea Hash

    public Transform target;
    public int segments = 8;            // how many joints
    public float ropeDrag = 0.1F;       // per joint
    public float ropeMass = 0.1F;       // per joint
    public float ropeColRadius = 0.5F;  // thickness for collisions
    //public float ropeBreakForce = 25.0F; // TODO
    private Vector3[] segmentPos;       // line renderer nodes
    private GameObject[] joints;        // all the rope joints
    private LineRenderer line;          // for rendering
    private bool rope = false;          // is it active

    public Vector3 swingAxis = new Vector3(1, 1, 1);    // for 2d movement make it 1,0,0
    public float lowTwistLimit = -100.0F;               // min joint angle
    public float highTwistLimit = 100.0F;               // max joint angle
    public float swing1Limit = 20.0F;                   // max angle of first joint at helicopter

    void Awake()
    {
        BuildRope();
    }

    void Update()
    {
        // do most gameplay stuff here
        // for example 
        // DestroyRope();
    }

    // sets the line renderer node positions
    // done here so it comes after physics
    void LateUpdate()
    {
        if (rope)
        {
            for (int i = 0; i < segments; i++)
            {
                if (i == 0)
                {
                    line.SetPosition(i, transform.position);
                }
                else
                if (i == segments - 1)
                {
                    line.SetPosition(i, target.transform.position);
                }
                else
                {
                    line.SetPosition(i, joints[i].transform.position);
                }
            }
            line.enabled = true;
        }
        else
        {
            line.enabled = false;
        }
    }


    // create the rope nodes on init
    void BuildRope()
    {
        line = gameObject.GetComponent<LineRenderer>();

        if (segments < 2) segments = 2; // sanity check

        line.SetVertexCount(segments);

        // remember all positions and joints
        segmentPos = new Vector3[segments];
        joints = new GameObject[segments];

        // the first segment is touching the helicopter
        segmentPos[0] = transform.position;

        // the last segment is where the end of the rope is
        segmentPos[segments - 1] = target.position;

        // Find the distance between each segment
        var segs = segments - 1;
        var seperation = ((target.position - transform.position) / segs);

        Debug.Log("BuildRope() creating " + segments + " segments with seperation of " + seperation + " for a total length of " + Vector3.Distance(transform.position, target.position));

        for (int s = 1; s < segments; s++)
        {
            // Find the each segments position using the slope from above
            Vector3 vector = (seperation * s) + transform.position;
            segmentPos[s] = vector;

            //Add Physics to the segments
            AddJointPhysics(s);
        }

        // Attach the joints to the target object and parent it to this object	
        CharacterJoint end = target.gameObject.AddComponent<CharacterJoint>();
        end.connectedBody = joints[joints.Length - 1].transform.GetComponent<Rigidbody>();
        end.swingAxis = swingAxis;
        SoftJointLimit limit_setter = end.lowTwistLimit;
        limit_setter.limit = lowTwistLimit;
        end.lowTwistLimit = limit_setter;
        limit_setter = end.highTwistLimit;
        limit_setter.limit = highTwistLimit;
        end.highTwistLimit = limit_setter;
        limit_setter = end.swing1Limit;
        limit_setter.limit = swing1Limit;
        end.swing1Limit = limit_setter;
        target.parent = transform;

        // the rope now exists in the scene!
        rope = true;
    }

    void AddJointPhysics(int n)
    {
        joints[n] = new GameObject("Rope_Joint_" + n);
        joints[n].transform.parent = transform;
        Rigidbody rigid = joints[n].AddComponent<Rigidbody>();
        SphereCollider col = joints[n].AddComponent<SphereCollider>();
        CharacterJoint ph = joints[n].AddComponent<CharacterJoint>();
        ph.swingAxis = swingAxis;
        SoftJointLimit limit_setter = ph.lowTwistLimit;
        limit_setter.limit = lowTwistLimit;
        ph.lowTwistLimit = limit_setter;
        limit_setter = ph.highTwistLimit;
        limit_setter.limit = highTwistLimit;
        ph.highTwistLimit = limit_setter;
        limit_setter = ph.swing1Limit;
        limit_setter.limit = swing1Limit;
        ph.swing1Limit = limit_setter;

        // hmmm maybe try 
        // ph.swingLimitSpring.damper = ???

        //ph.breakForce = ropeBreakForce; <--------------- TODO

        joints[n].transform.position = segmentPos[n];

        rigid.drag = ropeDrag;
        rigid.mass = ropeMass;
        col.radius = ropeColRadius;

        if (n == 1)
        {
            ph.connectedBody = transform.GetComponent<Rigidbody>();
        }
        else
        {
            ph.connectedBody = joints[n - 1].GetComponent<Rigidbody>();
        }

    }

    void DestroyRope()
    {
        // Stop Rendering Rope then Destroy all of its components
        rope = false;
        for (int dj = 0; dj < joints.Length - 1; dj++)
        {
            Destroy(joints[dj]);
        }

        segmentPos = new Vector3[0];
        joints = new GameObject[0];
        segments = 0;
    }
}