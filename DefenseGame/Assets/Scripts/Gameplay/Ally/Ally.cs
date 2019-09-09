using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{
    public enum rotations
    {
        left,
        right,
        front,
        back,
        allDirs
    }

    public enum states
    {
        idle,
        escape,
        allStates
    }

    [Header("Configurable Settings")]
    public states defaultState;
    public float speed;
    public float minTime;
    public float maxTime;
    public float waitingTime;

    [Header("State Settings")]
    public states currentState;
    public bool isBeingChased;

    private Rigidbody rig;
    private Transform target;
    private TorqueLookRotation lookRotation;
    private float randomTime;
    public float timer;
    public float waitingTimer;
    public float slerpTimer;
    private float slerpMaxTime;
    private Vector3 direction;
    private Quaternion rotation;
    private Quaternion oldRotation;
    private Quaternion newRotation;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        lookRotation = GetComponent<TorqueLookRotation>();
        randomRotation();
        newRandomTime();
        currentState = defaultState;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case states.idle:
                IdleState();
                break;
            case states.escape:
                EscapeState();
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        rig.MovePosition(rig.position + (direction) * Time.deltaTime);
    }

    private void switchState(states newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case states.idle:
                lookRotation.enabled = false;
                break;
            case states.escape:
                lookRotation.enabled = true;
                lookRotation.target = target;
                break;
            default:
                break;
        }
    }

    private void IdleState()
    {
        if (waitingTimer < waitingTime)
        {
            direction = transform.forward * speed * 3.0f;
            waitingTimer += Time.deltaTime;
        }
        else if (waitingTimer >= waitingTime)
        {
            direction = transform.forward * speed;
            timer += Time.deltaTime;
            slerpTimer += Time.deltaTime;

            if (timer >= randomTime)
            {
                randomRotation();
                newRandomTime();
                timer = 0;
            }

            if (slerpTimer <= 1)
            {
                newRotation = Quaternion.Slerp(transform.rotation, rotation, slerpTimer);
                transform.rotation = newRotation;
            }
        }
    }

    private void EscapeState()
    {
        direction = transform.forward * speed * 3.0f;

        if(!lookRotation.target)
        {
            waitingTimer = 0;
            timer = 0;
            slerpTimer = 0;
            randomRotation();
            switchState(states.idle);
        }
    }

    private void randomRotation()
    {
        oldRotation = rotation;
        rotations randomRotation = (rotations)Random.Range((int)rotations.left, (int)rotations.allDirs);       

        switch (randomRotation)
        {
            case rotations.left:
                rotation = transform.rotation * Quaternion.Euler(new Vector3(0, -90, 0));
                break;
            case rotations.right:
                rotation = transform.rotation * Quaternion.Euler(new Vector3(0, 90, 0));
                break;
            case rotations.front:
                //nothing, keeps going forward.
                break;
            case rotations.back:
                rotation = transform.rotation * Quaternion.Euler(new Vector3(0, 180, 0));
                break;
            default:
                break;
        }

        if(oldRotation != rotation)
        {
            slerpTimer = 0;
        }
    }

    private void newRandomTime()
    {
        randomTime = Random.Range(minTime, maxTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "enemyRadius")
        {
            target = other.gameObject.transform.parent.transform;
            switchState(states.escape);
            timer = 0;
            slerpTimer = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "enemyRadius")
        {
            waitingTimer = 0;
            timer = 0;
            slerpTimer = 0;
            randomRotation();
            switchState(states.idle);
        }
    }
}
