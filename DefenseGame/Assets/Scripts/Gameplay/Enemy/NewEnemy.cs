using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy : MonoBehaviour
{
    public enum allyStates
    {
        idle,
        move,
        allStates
    }

    public delegate void OnAction(GameObject enemy);
    public static OnAction OnDeath;

    [Header("General Settings")]
    public GameObject initialWaypoint;
    public allyStates initialState;
    
    [Header("Speed Settings")]
    public float speed;
    public float runSpeedMultiplier;

    [Header("Distance Settings")]
    [Range(0, 3)]
    public float distanceToStop;
    [Range(1, 30)]
    public float npcLostDistance;

    [Header("Assign Variables/Components")]
    public AllyRadius radius; // CHANGE NAME TO RADIUS
    public NewAlly ally;

    [Header("Checking Private Variables")]
    public allyStates currentState;
    public GameObject currentTarget;
    public GameObject selectedWaypoint;
    public GameObject oldWaypoint;
    public GameObject nextWaypointTarget;
    public float finalSpeed;
    public float originalDistanceToStop;
    public float finalDistanceToStop;

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

        radius.OnRadiusFindAlly += StartPursuit;

        currentState = initialState;
        selectedWaypoint = initialWaypoint;

        SwitchRotationTarget();

        finalSpeed = speed;
        originalDistanceToStop = distanceToStop;
        finalDistanceToStop = originalDistanceToStop;
        GameManager.Get().enemies.Add(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        switch (currentState)
        {
            case allyStates.idle:
                break;
            case allyStates.move:
                Move();
                break;
            default:
                break;
        }
    }

    private void Move()
    {
        float distance = Vector3.Distance(selectedWaypoint.transform.position, transform.position);
        float npcDistance = 100.0f;

        if (distance <= finalDistanceToStop)
        {
            ChangeWaypoint();
        }

        if (currentTarget)
        {
            npcDistance = Vector3.Distance(currentTarget.transform.position, transform.position);

            if (npcDistance >= npcLostDistance)
            {
                EndPursuit();
            }
        }

        if(npcDistance <= 4.0f)
        {
            torque.target = currentTarget.transform;
            selectedWaypoint = NewAlly.selectedWaypoint;
            finalDistanceToStop = originalDistanceToStop * 8.0f;
            Vector3 direction = (ally.offset.transform.position - transform.position).normalized;
            rig.MovePosition(rig.position + direction * finalSpeed * Time.deltaTime);
        }
        else
        {
            finalDistanceToStop = originalDistanceToStop;
            Vector3 direction = (selectedWaypoint.transform.position - transform.position).normalized;
            rig.MovePosition(rig.position + direction * finalSpeed * Time.deltaTime);
        }
    }

    private void ChangeWaypoint()
    {
        if(!currentTarget)
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
        else
        {
            oldWaypoint = selectedWaypoint;
            selectedWaypoint = nextWaypointTarget;
            SwitchRotationTarget();
            nextWaypointTarget = NewAlly.selectedWaypoint;
        }
        
    }

    private void SwitchRotationTarget()
    {
        torque.target = selectedWaypoint.transform;
    }

    private void StartPursuit(GameObject npc)
    {
        Debug.Log("ENTERED FIND ALLY");

        if (!currentTarget)
        {
            Debug.Log("ASSIGNED ALLY");
            currentTarget = npc;
            ally = currentTarget.GetComponent<NewAlly>();
            nextWaypointTarget = NewAlly.selectedWaypoint;
            finalSpeed = speed * runSpeedMultiplier;
        }
    }

    private void EndPursuit()
    {
        Debug.Log("Deleting npc target");
        currentTarget = null;
        nextWaypointTarget = null;
        finalSpeed = speed;
        finalDistanceToStop = originalDistanceToStop;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "explosion":
                if(OnDeath != null)
                {
                    OnDeath(gameObject);
                }
                Destroy(gameObject);
                break;
            case "proyectile":
                if (OnDeath != null)
                {
                    OnDeath(gameObject);
                }
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        GameManager.Get().enemies.Remove(gameObject);
        radius.OnRadiusFindEnemy -= StartPursuit;
    }
}
