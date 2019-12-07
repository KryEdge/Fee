using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("General Settings")]
    public KeyCode teleportKey;
    public GameObject initialWaypoint;

    [Header("References")]
    public GameObject cameraGO;
    public Transform lowestZoom;
    public Transform highestZoom;

    [Header("Distance Settings")]
    public Vector3 lastPosition;
    public float distanceTravelled;

    [Header("Sound Settings")]
    public GameObject movingSound;
    public float soundValue;

    [Header("Fairy Teleport")]
    public float teleportSpeed;
    public float distanceToStop;
    public float distance;
    public bool canTeleport;
    private Vector3 teleportPosition;

    [Header("Fairy Follow")]
    public float followTime;
    private float followTimer;
    private bool canFollow;

    [Header("Speed")]
    public int speed;
    public int fasterSpeed;
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

    [Header("Checking Variables")]
    public int finalSpeed;

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
        finalSpeed = speed;
        transform.position = initialWaypoint.transform.position;
        transform.position += transform.forward;
        canFollow = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!canFollow)
        {
            if(!canTeleport)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    finalSpeed = fasterSpeed;
                }
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    finalSpeed = speed;
                }

                if (Input.GetKeyDown(teleportKey))
                {
                    GoToFairies();
                    canTeleport = true;
                }
            }
            else
            {
                
            }

            if (Input.GetMouseButton(1))
            {
                if (isInverted)
                {
                    h = Input.GetAxis("Mouse X") * -1 * torqueSpeed * Time.deltaTime;
                }
                else
                {
                    h = Input.GetAxis("Mouse X") * torqueSpeed * Time.deltaTime;
                }
            }

            accelerationInput = Input.GetAxis("Mouse ScrollWheel") * -1;

            if (Mathf.Abs(accelerationInput) > 0.01f)
            {
                acceleration += Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed * Time.deltaTime;
            }

        }
        else
        {
            followTimer += Time.deltaTime;

            if(FlockManager.fairies[0])
            {
                gameObject.transform.position = FlockManager.fairies[0].transform.position + FlockManager.fairies[0].transform.up * 2.0f;
            }
            

            if (followTimer >= followTime)
            {
                canFollow = false;
                followTimer = 0;
            }
        }

        CheckAcceleration(ref acceleration, ref accelerationDecay);

        zoomAmount += acceleration;

        zoomAmount = Mathf.Clamp(zoomAmount, 0, 1);

        cameraGO.transform.position = Vector3.Slerp(lowestZoom.transform.position, highestZoom.transform.position, zoomAmount);
        cameraGO.transform.rotation = Quaternion.Slerp(lowestZoom.transform.rotation, highestZoom.transform.rotation, zoomAmount);

        chooseDirection();

        CheckAcceleration(ref h, ref torqueDecay);

        /*if (finalSpeed >= 0)
        {
            soundValue += 0.001f;
            Debug.Log("soundValue ==== " + soundValue);
            
            
            
        }*/

        //AkSoundEngine.SetRTPCValue("env_movimiento", soundValue);
        //AkSoundEngine.PostEvent("movimiento_camara", movingSound);

        if (FlockManager.fairies[0])
        {
            float distance = Vector3.Distance(cameraGO.transform.position, FlockManager.fairies[0].transform.position);

            //Debug.Log("Fairy distance from camera: " + distance);
        }

        
    }

    private void FixedUpdate()
    {
        if(canTeleport)
        {
            Vector3 direction = (teleportPosition - transform.position).normalized;
            rig.MovePosition(rig.position + direction * teleportSpeed * Time.deltaTime);

            distance = Vector3.Distance(transform.position, teleportPosition);

            if (distance <= distanceToStop)
            {
                canTeleport = false;
            }
        }
        else
        {
            rig.MovePosition(rig.position + transform.TransformDirection(direction) * finalSpeed * Time.deltaTime);
        }
        

        rig.AddTorque(transform.up * h);

        distanceTravelled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
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

    public void GoToFairies()
    {
        if(CheatSystem.isTimeNormal && !UIPauseButton.isGamePaused)
        {
            if (FlockManager.fairies[0])
            {
                teleportPosition = FlockManager.fairies[0].transform.position + FlockManager.fairies[0].transform.up * 2.0f;
            }
        }
    }
}