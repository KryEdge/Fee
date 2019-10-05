using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public float maxSpeed;
    public float minSpeed;
    public float rotationSpeed = 5f;
    Vector3 averageHeading;
    Vector3 averageAvoidance;
    float neighbourDistance = 30f;

    static public float originalFinalSpeed;
    static public float finalSpeed;
    public bool isAlone;
    public Vector3 goalPos;
    private Rigidbody rig;
    private TorqueLookRotation torque;
    //private GameObject goalPosTransform;

    // Start is called before the first frame update
    void Start()
    {
        isAlone = false;
        finalSpeed = Random.Range(minSpeed, maxSpeed);
        originalFinalSpeed = finalSpeed;
        goalPos = FlockManager.goalPosition;
        rig = GetComponent<Rigidbody>();
        torque = GetComponent<TorqueLookRotation>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(finalSpeed);

        if(isAlone)
        {
            Vector3 direction = (goalPos - transform.position).normalized;
            rig.MovePosition(rig.position + direction * finalSpeed * Time.deltaTime);
        }
        else
        {
            rig.MovePosition(rig.position + transform.forward * finalSpeed * Time.deltaTime);
        }
        

        ApplyRules();
    }

    public void ApplyRules()
    {
        List<GameObject> gos;
        gos = FlockManager.fairies;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;

        goalPos = FlockManager.goalPosition;

        float distance;

        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            if(go != gameObject && go != null)
            {
                distance = Vector3.Distance(go.transform.position, transform.position);

                if(distance <= neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if(distance < 1.0f)
                    {
                        vavoid = vavoid + (transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();

                }
            }
        }

        if(groupSize > 0)
        {
            torque.enabled = false;
            vcentre = vcentre / groupSize + (goalPos - transform.position);

            Vector3 direction = (vcentre + vavoid) - transform.position;
            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }

            isAlone = false;
        }
        else
        {
            torque.enabled = true;
            torque.target = NewAlly.selectedWaypoint.transform;
            isAlone = true;
        }
    }
}
