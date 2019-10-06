using UnityEngine;
using System.Collections;

public class TorqueLookRotation : MonoBehaviour
{
    public enum directions
    {
        forward,
        backwards,
        allDirs
    }

    public directions direction;
    public Transform target;
    public float force = 0.1f;

    private Rigidbody rig;
    private Vector3 currentDirection;

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        switch (direction)
        {
            case directions.forward:
                currentDirection = transform.forward;
                break;
            case directions.backwards:
                currentDirection = transform.forward*-1;
                break;
            default:
                break;
        }
    }

    void FixedUpdate()
    {
        if(target)
        {
            Vector3 targetDelta = target.position - transform.position;

            //get the angle between transform.forward and target delta
            float angleDiff = Vector3.Angle(currentDirection, targetDelta);

            // get its cross product, which is the axis of rotation to
            // get from one vector to the other
            Vector3 cross = Vector3.Cross(currentDirection, targetDelta);

            // apply torque along that axis according to the magnitude of the angle.
            rig.AddTorque(cross * angleDiff * force);
        }
    }
}
