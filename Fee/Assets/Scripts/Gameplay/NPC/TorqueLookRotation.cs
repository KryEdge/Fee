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

    [Header("General Settings")]
    public directions direction;
    public Transform target;
    public float force = 0.1f;
    public bool notTorque;
    public float rotationSpeed;

    private Rigidbody rig;
    private Vector3 currentDirection;
    
    private void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(!notTorque)
        {
            switch (direction)
            {
                case directions.forward:
                    currentDirection = transform.forward;
                    break;
                case directions.backwards:
                    currentDirection = transform.forward * -1;
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (target)
            {             
                Quaternion newRotation = Quaternion.LookRotation(target.transform.position - transform.position, transform.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    void FixedUpdate()
    {
        if (!notTorque)
        {
            if (target)
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
}
