using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAlly : MonoBehaviour
{
    public enum allyStates
    {
        idle,
        walk,
        run,
        allStates
    }

    [Header("Ally Settings")]
    public GameObject initialWaypoint;
    public allyStates initialState;
    public allyStates currentState;
    public float speed;
    public float runSpeed;
    public Color normalColor;
    public Color dangerColor;
    [Range(0, 3)]
    public float distanceToStop;

    [Header("Checking Variables // Private")]
    static public GameObject selectedWaypoint;
    static public GameObject oldWaypoint;

    //Private
    private Rigidbody rig;
    private TorqueLookRotation torque;
    private Outline outline;


    // Start is called before the first frame update
    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        torque = GetComponent<TorqueLookRotation>();
        outline = GetComponent<Outline>();

        currentState = initialState;
        selectedWaypoint = initialWaypoint;

        SwitchRotationTarget();
    }

    // Update is called once per frame
    private void Update()
    {
        switch (currentState)
        {
            case allyStates.idle:
                break;
            case allyStates.walk:
                Walk();
                break;
            case allyStates.run:
                break;
            default:
                break;
        }
    }

    private void Walk()
    {
        float distance = Vector3.Distance(selectedWaypoint.transform.position, transform.position);

        if(distance <= distanceToStop)
        {
            ChangeWaypoint();
        }

        Vector3 direction = (selectedWaypoint.transform.position - transform.position).normalized;
        rig.MovePosition(rig.position + direction * speed * Time.deltaTime);
    }

    private void ChangeWaypoint()
    {
        Waypoints waypointList = selectedWaypoint.GetComponent<Waypoints>();
        List<GameObject> newWaypoints = new List<GameObject>();
        GameObject newSelectedWaypoint;

        for (int i = 0; i < waypointList.nextWaypoints.Length; i++)
        {
            if (waypointList.nextWaypoints[i] != oldWaypoint)
            {
                newWaypoints.Add(waypointList.nextWaypoints[i]);
            }
        }

        newSelectedWaypoint = newWaypoints[Random.Range(0, newWaypoints.Count)];
        oldWaypoint = selectedWaypoint;
        selectedWaypoint = newSelectedWaypoint;
        SwitchRotationTarget();
    }

    private void SwitchRotationTarget()
    {
        Debug.Log("Applying new Target");
        torque.target = selectedWaypoint.transform;
        FlockManager.goalPosition = selectedWaypoint.transform.position;
    }
}
