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

    [Header("Zoom Acceleration")]   
    public float accelerationInput = 0;
    public float acceleration = 0;
    public float accelerationDecay = 0.0001f;
    [Range(0, 1)]
    public float zoomAmount = 1;


    private Rigidbody rig;
    private Vector3 direction;
    private Vector3 torque;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Rotation Horizontal") * torqueSpeed * Time.deltaTime;

        accelerationInput = Input.GetAxis("Mouse ScrollWheel") *-1;

        if(Mathf.Abs(accelerationInput) > 0.01f)
        {
            acceleration += Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed * Time.deltaTime;            
        }

        if(acceleration > 0)
        {
            acceleration -= accelerationDecay * Time.deltaTime;
            if (acceleration <= 0)
            {
                acceleration = 0;
            }
        }
        else if (acceleration < 0)
        {
            acceleration += accelerationDecay * Time.deltaTime;
            if (acceleration >= 0)
            {
                acceleration = 0;
            }
        }

        zoomAmount += acceleration;

        zoomAmount = Mathf.Clamp(zoomAmount,0, 1);

        cameraGO.transform.position = Vector3.Slerp(lowestZoom.transform.position, highestZoom.transform.position, zoomAmount);
        cameraGO.transform.rotation = Quaternion.Slerp(lowestZoom.transform.rotation, highestZoom.transform.rotation, zoomAmount);

        chooseDirection();

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
}