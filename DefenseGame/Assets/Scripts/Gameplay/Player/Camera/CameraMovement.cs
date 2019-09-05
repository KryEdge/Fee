using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Rigidbody rig;
    public Vector3 direction;
    public Vector3 torque;
    public int speed;
    public int torqueSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Rotation Horizontal") * torqueSpeed * Time.deltaTime;
        
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