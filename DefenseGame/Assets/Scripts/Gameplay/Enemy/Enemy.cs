using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum enemyStates
    {
        idle,
        search,
        walk,
        follow,
        allStates
    }

    [Header("Enemy Settings")]
    public enemyStates initialState;
    public enemyStates currentState;
    public float speed;
    public float originalSpeed;
    public int initialTarget;

    [Header("Assign References")]
    public EnemyRadius radius;
    public EnemyRadius radius2;
    public EnemyRadius exitRadius;

    [Header("Checking Variables")]
    public GameObject target;
    public GameObject targetOffset;
    public List<GameObject> waypointsFound;
    public GameObject selectedWaypoint;
    public bool hasReachedWaypoint;
    public bool hasSelectedWaypoint;
    public bool doOnce;
    public bool doOnce2;
    public bool allyOnSight;
    public float distanceAlly;

    //Private
    private Rigidbody rig;
    private ConstantForce cf;
    private TorqueLookRotation torque;
    private GameManager gm;
    private Vector3 destination;
    private float distanceToStop;

    private void Start()
    {
        // DELETE EXIT RADIUS AND REPLACE IT TO VECTOR3 DISTANCE.
        cf = GetComponent<ConstantForce>();
        gm = GameManager.Get();
        rig = GetComponent<Rigidbody>();
        torque = GetComponent<TorqueLookRotation>();

        radius.OnRadiusFindWaypoint += AddFoundWaypoint;
        radius2.OnRadiusFindWaypoint += AddFoundWaypoint;
        radius.OnRadiusFindAlly += AddFoundAlly;
        radius2.OnRadiusFindAlly += AddFoundAlly;
        exitRadius.OnRadiusLostAlly += LostAlly;
        torque.target = gm.playerWaypoints[initialTarget].transform;
        selectedWaypoint = gm.playerWaypoints[initialTarget];
        hasSelectedWaypoint = true;
        cf.enabled = false;
        originalSpeed = speed;
        currentState = initialState;

        waypointsFound.Add(gm.playerWaypoints[initialTarget]);
        SwitchRadiusOff(radius.gameObject);
        SwitchRadiusOff(radius2.gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        switch (currentState)
        {
            case enemyStates.idle:
                break;
            case enemyStates.search:
                Search();
                break;
            case enemyStates.walk:
                Walk();
                break;
            case enemyStates.follow:
                Follow();
                break;
            default:
                break;
        }
    }

    private void Search()
    {
        if (!allyOnSight)
        {
            cf.torque = new Vector3(0, 0.8f, 0);
        }
    }

    private void Walk()
    {
        float distance = 90.0f;
        if (selectedWaypoint)
        {
            distance = Vector3.Distance(selectedWaypoint.transform.position, transform.position);
        }

        if (distance <= 2.0f)
        {
            if (!allyOnSight)
            {
                if (!doOnce)
                {
                    hasReachedWaypoint = true;
                    SwitchRadiusOn(radius.gameObject);
                    SwitchRadiusOn(radius2.gameObject);
                    doOnce2 = false;
                    doOnce = true;
                }
            }

        }

        if (distance <= 1.0f)
        {
            if (!allyOnSight)
            {
                if (!doOnce2)
                {
                    if (CheckForRandomWaypoint())
                    {
                        cf.enabled = false;
                        torque.enabled = true;
                        SwitchRadiusOff(radius.gameObject);
                        SwitchRadiusOff(radius2.gameObject);
                        torque.target = selectedWaypoint.transform;
                        hasSelectedWaypoint = true;
                        doOnce = false;
                    }
                    else
                    {
                        torque.enabled = false;
                        cf.enabled = true;
                        hasSelectedWaypoint = false;
                        SwitchRadiusOff(radius.gameObject);
                        SwitchRadiusOff(radius2.gameObject);
                        SwitchRadiusOn(radius.gameObject);
                        currentState = enemyStates.search;
                    }

                    doOnce2 = true;
                }
            }
        }

        if (hasSelectedWaypoint)
        {
            if(!allyOnSight)
            {
                Vector3 direction = (selectedWaypoint.transform.position - transform.position).normalized;
                rig.MovePosition(rig.position + direction * speed * Time.deltaTime);
            }
        }
    }

    private void Follow()
    {
        if (target)
        {
            distanceAlly = Vector3.Distance(target.transform.position, transform.position);

            if (distanceAlly >= 15.0f)
            {
                LostAlly(target);
            }
        }

        if (allyOnSight)
        {
            if (distanceAlly <= 3.0f)
            {
                Vector3 direction = (targetOffset.transform.position - transform.position).normalized;
                rig.MovePosition(rig.position + direction * speed * Time.deltaTime);
            }
            else
            {
                Vector3 direction = (selectedWaypoint.transform.position - transform.position).normalized;
                rig.MovePosition(rig.position + direction * speed * Time.deltaTime);
            }
        }
    }

    private void AddFoundWaypoint(GameObject newWaypoint)
    {
        if(!allyOnSight)
        {
            bool isTheSameWaypoint = false;

            if (hasReachedWaypoint)
            {
                if (newWaypoint != selectedWaypoint)
                {
                    foreach (GameObject waypoint in waypointsFound)
                    {
                        if (waypoint == newWaypoint)
                        {
                            isTheSameWaypoint = true;
                        }
                    }

                    if (!isTheSameWaypoint)
                    {
                        waypointsFound.Add(newWaypoint);
                    }

                    if (!hasSelectedWaypoint)
                    {
                        if (CheckForRandomWaypoint())
                        {
                            cf.enabled = false;
                            torque.enabled = true;
                            SwitchRadiusOff(radius.gameObject);
                            SwitchRadiusOff(radius2.gameObject);
                            torque.target = selectedWaypoint.transform;
                            hasSelectedWaypoint = true;
                            doOnce = false;
                        }
                        else
                        {
                            torque.enabled = false;
                            cf.enabled = true;
                            hasSelectedWaypoint = false;
                            SwitchRadiusOff(radius.gameObject);
                            SwitchRadiusOff(radius2.gameObject);
                            SwitchRadiusOn(radius.gameObject);
                            currentState = enemyStates.search;
                        }
                    }
                }
            }
        }
    }

    private void AddFoundAlly(GameObject ally)
    {
        allyOnSight = true;
        target = ally;
        Ally foundAlly = ally.GetComponent<Ally>();
        selectedWaypoint = foundAlly.selectedWaypoint;
        targetOffset = foundAlly.offset;
        cf.enabled = false;
        torque.enabled = true;
        SwitchRadiusOff(radius.gameObject);
        SwitchRadiusOff(radius2.gameObject);
        torque.target = target.transform;
        hasSelectedWaypoint = true;
        doOnce = false;
        waypointsFound.Clear();
        SwitchRadiusOff(exitRadius.gameObject);
        SwitchRadiusOn(exitRadius.gameObject);
        speed = speed * 1.5f;
        currentState = enemyStates.follow;
    }

    private void LostAlly(GameObject ally)
    {
        allyOnSight = false;
        waypointsFound.Clear();
        hasSelectedWaypoint = false;
        hasReachedWaypoint = true;
        selectedWaypoint = null;
        torque.enabled = false;
        cf.enabled = true;
        hasSelectedWaypoint = false;
        target = null;
        SwitchRadiusOff(radius.gameObject);
        SwitchRadiusOff(radius2.gameObject);
        SwitchRadiusOn(radius.gameObject);
        SwitchRadiusOff(exitRadius.gameObject);
        SwitchRadiusOn(exitRadius.gameObject);
        speed = originalSpeed;
        currentState = enemyStates.walk;
    }

    private bool CheckForRandomWaypoint()
    {
        if(!allyOnSight)
        {
            if (waypointsFound.Count != 0)
            {
                selectedWaypoint = waypointsFound[Random.Range(0, waypointsFound.Count)];
                waypointsFound.Clear();
                currentState = enemyStates.walk;
                return true;
            }
        }

        return false;
    }

    private void SwitchRadiusOn(GameObject target)
    {
        target.gameObject.SetActive(true);
    }

    private void SwitchRadiusOff(GameObject target)
    {
        target.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        radius.OnRadiusFindWaypoint -= AddFoundWaypoint;
        radius2.OnRadiusFindWaypoint -= AddFoundWaypoint;
        radius.OnRadiusFindAlly -= AddFoundAlly;
        radius2.OnRadiusFindAlly -= AddFoundAlly;
        exitRadius.OnRadiusLostAlly -= LostAlly;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "proyectile")
        {
            Destroy(gameObject);
        }
    }
}
