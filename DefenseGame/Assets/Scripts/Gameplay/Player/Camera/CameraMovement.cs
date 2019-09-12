using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 direction;
    public Vector3 torque;
    public int speed;
    public int torqueSpeed;
    public float zoomSpeed = 10;
    public GameObject cameraGO;
    public Transform lowestZoom;
    public Transform highestZoom;

    private Rigidbody rig;
    [Range(0,1)]
    public float zoomAmount;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Rotation Horizontal") * torqueSpeed * Time.deltaTime;
        zoomAmount += Input.GetAxis("Mouse ScrollWheel") *-1 * zoomSpeed * Time.deltaTime;
        zoomAmount = Mathf.Clamp(zoomAmount,0, 1);

        cameraGO.transform.position = Vector3.Slerp(lowestZoom.transform.position, highestZoom.transform.position, zoomAmount);

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