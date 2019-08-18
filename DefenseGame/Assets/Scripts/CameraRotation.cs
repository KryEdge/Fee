using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class CameraRotation : MonoBehaviour
{

    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    private Rigidbody rig;

    private float x = 0.0f;
    private float y = 0.0f;
    private Vector3 position;
    private Vector3 negDistance;
    private Quaternion rotation;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rig = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rig != null)
        {
            rig.freezeRotation = true;
        }

        RotateCamera();
    }

    void LateUpdate()
    {
        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

        if (Input.GetMouseButton(1))
        {
            RotateCamera();
        }

        if (target)
        {
            negDistance = new Vector3(0.0f, 0.0f, -distance);
            position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    private void RotateCamera()
    {
        if (target)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            rotation = Quaternion.Euler(y, x, 0);

            RaycastHit hit;
            if (Physics.Linecast(target.position, transform.position, out hit))
            {
                if (distance <= 30)
                {
                    distance += (hit.distance / 10);
                    Debug.Log((hit.distance / 10));
                }
            }

        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}