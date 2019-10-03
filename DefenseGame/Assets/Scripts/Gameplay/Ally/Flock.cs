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

    public float originalFinalSpeed;
    public float finalSpeed;
    private Rigidbody rig;

    // Start is called before the first frame update
    void Start()
    {
        finalSpeed = Random.Range(minSpeed, maxSpeed);
        originalFinalSpeed = finalSpeed;
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rig.MovePosition(rig.position + transform.forward * finalSpeed * Time.deltaTime);

        ApplyRules();
        /*if(Random.Range(0,5) < 1)
        {
            
        }*/
    }

    public void ApplyRules()
    {
        GameObject[] gos;
        gos = FlockManager.fairies;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 goalPos = FlockManager.goalPosition;

        float distance;

        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            if(go != gameObject)
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
                    //gSpeed = gSpeed + anotherFlock.finalSpeed;
                }
            }
        }

        if(groupSize > 0)
        {
            vcentre = vcentre / groupSize + (goalPos - transform.position);
            //finalSpeed = gSpeed / groupSize;

            Vector3 direction = (vcentre + vavoid) - transform.position;
            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }
    }
}
