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

    /*// Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        torque.target = waypoints[currentTarget].transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(waypoints[currentTarget].transform.position, transform.position);

        if(distance <= 0.5f)
        {
            currentTarget++;
            if(currentTarget >= waypoints.Length)
            {
                currentTarget = 0;
            }
            torque.target = waypoints[currentTarget].transform;
        }

        Vector3 direction = (waypoints[currentTarget].transform.position - transform.position).normalized;
        rig.MovePosition(rig.position + direction * speed * Time.deltaTime);
    }*/
}
