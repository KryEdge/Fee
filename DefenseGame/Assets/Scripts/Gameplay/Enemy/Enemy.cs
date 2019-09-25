using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
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
        cf = GetComponent<ConstantForce>();
        gm = GameManager.Get();
        rig = GetComponent<Rigidbody>();
        torque = GetComponent<TorqueLookRotation>();

        radius.OnRadiusFindWaypoint += AddFoundWaypoint;
        radius2.OnRadiusFindWaypoint += AddFoundWaypoint;
        radius.OnRadiusFindAlly += AddFoundAlly;
        radius2.OnRadiusFindAlly += AddFoundAlly;
        exitRadius.OnRadiusLostAlly += LostAlly;
        //radius2.OnRadiusLostAlly += LostAlly;
        torque.target = gm.playerWaypoints[initialTarget].transform;
        selectedWaypoint = gm.playerWaypoints[initialTarget];
        hasSelectedWaypoint = true;
        cf.enabled = false;
        originalSpeed = speed;

        waypointsFound.Add(gm.playerWaypoints[initialTarget]);
        SwitchRadiusOff(radius.gameObject);
        SwitchRadiusOff(radius2.gameObject);
    }


    // Update is called once per frame
    private void Update()
    {
        float distance = 90.0f;
        if (selectedWaypoint)
        {
            distance = Vector3.Distance(selectedWaypoint.transform.position, transform.position);

        }
        //float distance = Vector3.Distance(selectedWaypoint.transform.position, transform.position);
        //float distanceAlly = 90.0f;
        if (target)
        {
            distanceAlly = Vector3.Distance(target.transform.position, transform.position);

            if (distanceAlly >= 15.0f)
            {
                LostAlly(target);
            }
        }

        
        

        if (distance <= 2.0f)
        {
            if (!allyOnSight)
            {
                if (!doOnce)
                {
                    //("parte 0");
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
            if(!allyOnSight)
            {
                if (!doOnce2)
                {
                    if (CheckForRandomWaypoint())
                    {
                        //("parte 0.5");
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
                        //("parte 1");
                        torque.enabled = false;
                        cf.enabled = true;
                        hasSelectedWaypoint = false;
                        SwitchRadiusOff(radius.gameObject);
                        SwitchRadiusOff(radius2.gameObject);
                        SwitchRadiusOn(radius.gameObject);
                    }

                    doOnce2 = true;
                }
            }
        }

        if (hasSelectedWaypoint)
        {
            if (allyOnSight)
            {
                if(distanceAlly <= 3.0f)
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
            else
            {
                Vector3 direction = (selectedWaypoint.transform.position - transform.position).normalized;
                rig.MovePosition(rig.position + direction * speed * Time.deltaTime);
            }
            
        }
        else
        {
            if(!allyOnSight)
            {
                //("Enemy is rotating!!");
                cf.torque = new Vector3(0, 0.8f, 0);
            }
        }

    }

    private void AddFoundWaypoint(GameObject newWaypoint)
    {
        if(!allyOnSight)
        {
            //("Initiate Finding waypoints");
            //("Waypoint name : " + newWaypoint.name);
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
                        //("messi");

                        if (CheckForRandomWaypoint())
                        {
                            cf.enabled = false;
                            torque.enabled = true;
                            SwitchRadiusOff(radius.gameObject);
                            SwitchRadiusOff(radius2.gameObject);
                            torque.target = selectedWaypoint.transform;
                            hasSelectedWaypoint = true;
                            doOnce = false;
                            //("parte 1 EX");
                        }
                        else
                        {
                            //("parte 2");
                            torque.enabled = false;
                            cf.enabled = true;
                            hasSelectedWaypoint = false;
                            SwitchRadiusOff(radius.gameObject);
                            SwitchRadiusOff(radius2.gameObject);
                            SwitchRadiusOn(radius.gameObject);
                        }
                    }
                }
            }
        }
    }

    private void AddFoundAlly(GameObject ally)
    {
        //("Found ALLY !!!");
        allyOnSight = true;
        target = ally;
        Ally foundAlly = ally.GetComponent<Ally>();
        selectedWaypoint = foundAlly.selectedWaypoint;
        targetOffset = foundAlly.offset;
        //torque.target = selectedWaypoint.transform;
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
    }

    private void LostAlly(GameObject ally)
    {
        //("Enemy Escaped");
        allyOnSight = false;
        //Ally foundAlly = ally.GetComponent<Ally>();
        waypointsFound.Clear();
        hasSelectedWaypoint = false;
        hasReachedWaypoint = true;
        selectedWaypoint = null;
        //("parte V2");
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
    }

    private bool CheckForRandomWaypoint()
    {
        if(!allyOnSight)
        {
            if (waypointsFound.Count != 0)
            {
                //("Found waypoint!!!");
                selectedWaypoint = waypointsFound[Random.Range(0, waypointsFound.Count)];
                waypointsFound.Clear();
                return true;
            }
        }

        return false;
    }

    private void SwitchRadiusOn(GameObject target)
    {
        //("Switching On");
        target.gameObject.SetActive(true);
    }

    private void SwitchRadiusOff(GameObject target)
    {
        //("Switching Off");
        target.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        radius.OnRadiusFindWaypoint -= AddFoundWaypoint;
        radius2.OnRadiusFindWaypoint -= AddFoundWaypoint;
        radius.OnRadiusFindAlly -= AddFoundAlly;
        radius2.OnRadiusFindAlly -= AddFoundAlly;
        exitRadius.OnRadiusLostAlly -= LostAlly;
        //radius2.OnRadiusLostAlly -= LostAlly;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "proyectile")
        {
            Destroy(gameObject);
        }
    }
}
