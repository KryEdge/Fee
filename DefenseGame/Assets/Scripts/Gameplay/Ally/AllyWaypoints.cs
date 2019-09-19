using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyWaypoints : MonoBehaviour
{
    public float speed;
    public GameObject target;

    public TorqueLookRotation torque;
    public int currentTarget;


    private Vector3 destination;
    private float distanceToStop;
    private Rigidbody rig;
    private GameManager gm;
    public AllyRadius radius;
    public List<GameObject> waypointsFound;
    public bool hasReachedWaypoint;
    public GameObject selectedWaypoint;

    private void Start()
    {
        radius = GetComponentInChildren<AllyRadius>();
        gm = GameManager.Get();
        rig = GetComponent<Rigidbody>();
        radius.OnRadiusFindWaypoint = AddFoundWaypoint;
        currentTarget = 0;
        torque = GetComponent<TorqueLookRotation>();
        torque.target = gm.playerWaypoints[currentTarget].transform;

        selectedWaypoint = gm.playerWaypoints[0];

        //torque.target = waypoints[currentTarget].transform;
    }


    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(selectedWaypoint.transform.position, transform.position);

        if (distance <= 1.5f)
        {
            hasReachedWaypoint = true;
            SwitchRadius();
        }

        if (distance <= 1.0f)
        {
            CheckForRandomWaypoint();
            torque.target = selectedWaypoint.transform;
        }

        Vector3 direction = (selectedWaypoint.transform.position - transform.position).normalized;
        rig.MovePosition(rig.position + direction * speed * Time.deltaTime);
    }

    private void AddFoundWaypoint(GameObject newWaypoint)
    {
        Debug.Log("Initiate Finding waypoints");

        bool isTheSameWaypoint = false;

        if(hasReachedWaypoint)
        {
            foreach (GameObject waypoint in waypointsFound)
            {
                if(waypoint == newWaypoint)
                {
                    isTheSameWaypoint = true;
                }
            }

            if(!isTheSameWaypoint)
            {
                waypointsFound.Add(newWaypoint);
            }
            
        }
    }

    private void CheckForRandomWaypoint()
    {
        selectedWaypoint = waypointsFound[Random.Range(0, waypointsFound.Count)];
        waypointsFound.Clear();
    }

    private void SwitchRadius()
    {
        Debug.Log("Switching");
        radius.gameObject.SetActive(false);
        radius.gameObject.SetActive(true);
    }
}
