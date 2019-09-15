using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    public GameObject cameraGO;
    public Transform lowestZoom;
    public Transform highestZoom;

    [Header("Speed")]
    public int speed;
    public int torqueSpeed;
    
    public float zoomSpeed = 10;

    [Header("Acceleration Decay")]
    [Range(0, 15)]
    public float torqueDecay = 0.01f;
    [Range(0, 1)]
    public float accelerationDecay = 0.05f;

    [Header("Zoom Acceleration")]    
    [Range(0, 1)]
    public float zoomAmount = 1;

    [Header("Camera Rotation")]
    public bool isInverted;

    private Rigidbody rig;
    private Vector3 direction;
    private Vector3 torque;
    private float h;
    private float accelerationInput = 0;
    private float acceleration = 0;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if(isInverted)
            {
                h = Input.GetAxis("Mouse X") * -1 * torqueSpeed * Time.deltaTime;
            }
            else
            {
                h = Input.GetAxis("Mouse X") * torqueSpeed * Time.deltaTime;
            }
        }

        accelerationInput = Input.GetAxis("Mouse ScrollWheel") *-1;

        if(Mathf.Abs(accelerationInput) > 0.01f)
        {
            acceleration += Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed * Time.deltaTime;            
        }

        CheckAcceleration(ref acceleration, ref accelerationDecay);

        zoomAmount += acceleration;

        zoomAmount = Mathf.Clamp(zoomAmount,0, 1);

        cameraGO.transform.position = Vector3.Slerp(lowestZoom.transform.position, highestZoom.transform.position, zoomAmount);
        cameraGO.transform.rotation = Quaternion.Slerp(lowestZoom.transform.rotation, highestZoom.transform.rotation, zoomAmount);

        chooseDirection();

        CheckAcceleration(ref h, ref torqueDecay);

        rig.AddTorque(transform.up * h);
    }

    private void FixedUpdate()
    {
        rig.MovePosition(rig.position + transform.TransformDirection(direction) * speed * Time.deltaTime);
    }

    private void chooseDirection()
    {
        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    private void CheckAcceleration(ref float value, ref float decayValue)
    {
        if (value > 0)
        {
            value -= decayValue * Time.deltaTime;
            if (value <= 0)
            {
                value = 0;
            }
        }
        else if (value < 0)
        {
            value += decayValue * Time.deltaTime;
            if (value >= 0)
            {
                value = 0;
            }
        }
    }
}