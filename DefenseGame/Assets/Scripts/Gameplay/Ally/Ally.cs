using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{
    [Header("Ally Settings")]
    public float speed;
    public float originalSpeed;
    public Color dangerColor;

    [Header("Assign References")]
    //public FlockManager fm;
    
    public AllyRadius radius;
    public AllyRadius radius2;
    public AllyRadius findEnemyRadius;
    public GameObject offset;
    public GameObject foundEnemy;

    [Header("Checking Variables")]
    public Flock flock;
    public List<GameObject> waypointsFound;
    public GameObject selectedWaypoint;
    public bool hasReachedWaypoint;
    public bool hasSelectedWaypoint;
    public bool doOnce;
    public bool doOnce2;
    public int randomDirection;

    //Private
    private Rigidbody rig;
    //private ConstantForce cf;
    //private TorqueLookRotation //torque;
    private GameManager gm;
    private Vector3 destination;
    private float distanceToStop;
    private Outline outline;

    private void Start()
    {
        flock = GetComponent<Flock>();
        //cf = GetComponent<ConstantForce>();
        gm = GameManager.Get();
        rig = GetComponent<Rigidbody>();
        //torque = GetComponent<TorqueLookRotation>();
        outline = GetComponent<Outline>();

        radius.OnRadiusFindWaypoint += AddFoundWaypoint;
        radius2.OnRadiusFindWaypoint += AddFoundWaypoint;
        findEnemyRadius.OnRadiusFindEnemy += Escape;
        //radius2.OnRadiusFindEnemy += Escape;
        ////torque.target = gm.playerWaypoints[0].transform;
        selectedWaypoint = gm.playerWaypoints[0];
        hasSelectedWaypoint = true;
        //cf.enabled = false;
        originalSpeed = speed;

        FlockManager.goalPosition = gm.playerWaypoints[0].transform.position;

        waypointsFound.Add(gm.playerWaypoints[0]);
        SwitchRadiusOff(radius.gameObject);
        SwitchRadiusOff(radius2.gameObject);
    }


    // Update is called once per frame
    private void Update()
    {
        float distanceFromEnemy = 90.0f;
        float distance = Vector3.Distance(selectedWaypoint.transform.position, transform.position);

        if(foundEnemy)
        {
            distanceFromEnemy = Vector3.Distance(foundEnemy.transform.position, transform.position);

            /*if(distanceFromEnemy <= 2.0f)
            {
                //speed = 0;
            }*/

            if(distanceFromEnemy >= 15.0f)
            {
                flock.finalSpeed = flock.originalFinalSpeed;
                SuccessfullEscape();
            }
        }

        if (distance <= 2.0f)
        {
            if(!foundEnemy)
            {
                if (!doOnce)
                {
                    ////("parte 0");
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
                if (!doOnce2)
                {
                    if (CheckForRandomWaypoint())
                    {
                        ////("parte 0.5");
                        //cf.enabled = false;
                        //torque.enabled = true;
                        SwitchRadiusOff(radius.gameObject);
                        SwitchRadiusOff(radius2.gameObject);
                        //torque.target = selectedWaypoint.transform;
                        hasSelectedWaypoint = true;
                        doOnce = false;
                    }
                    else
                    {
                        ////("parte 1");
                        //torque.enabled = false;
                        //cf.enabled = true;
                        hasSelectedWaypoint = false;
                        SwitchRadiusOff(radius.gameObject);
                        SwitchRadiusOff(radius2.gameObject);
                        SwitchRadiusOn(radius.gameObject);
                        randomDirection = Random.Range(0, 2);
                    }

                    doOnce2 = true;
                }
        }

        if (hasSelectedWaypoint)
        {
            //Vector3 direction = (selectedWaypoint.transform.position - transform.position).normalized;
            //rig.MovePosition(rig.position + direction * speed * Time.deltaTime);
        }
        else
        {
            ////cf.//torque = new Vector3(0, 0.8f, 0);
            if(randomDirection == 0)
            {
                transform.Rotate(new Vector3(0, 0.8f, 0));
            }
            else if (randomDirection == 1)
            {
                transform.Rotate(new Vector3(0, -0.8f, 0));
            }
            
        }

    }

    private void AddFoundWaypoint(GameObject newWaypoint)
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
                    ////("messi");

                    if (CheckForRandomWaypoint())
                    {
                        //cf.enabled = false;
                        //torque.enabled = true;
                        SwitchRadiusOff(radius.gameObject);
                        SwitchRadiusOff(radius2.gameObject);
                        //torque.target = selectedWaypoint.transform;
                        hasSelectedWaypoint = true;
                        doOnce = false;
                        doOnce2 = false;
                        ////("parte 1 EX");
                    }
                    else
                    {
                        ////("parte 2");
                        //torque.enabled = false;
                        //cf.enabled = true;
                        hasSelectedWaypoint = false;
                        SwitchRadiusOff(radius.gameObject);
                        SwitchRadiusOff(radius2.gameObject);
                        SwitchRadiusOn(radius.gameObject);
                    }
                }
            }
        }


    }

    private bool CheckForRandomWaypoint()
    {
        if (waypointsFound.Count != 0)
        {
            ////("Found waypoint!!!");
            selectedWaypoint = waypointsFound[Random.Range(0, waypointsFound.Count)];
            FlockManager.goalPosition = selectedWaypoint.transform.position;
            flock.ApplyRules();
            waypointsFound.Clear();
            return true;
        }

        return false;
    }

    private void SwitchRadiusOn(GameObject target)
    {
        ////("Switching On");
        target.gameObject.SetActive(true);
    }

    private void SwitchRadiusOff(GameObject target)
    {
        ////("Switching Off");
        target.gameObject.SetActive(false);
    }

    private void Escape(GameObject enemy)
    {
        ////("Escaping from ENEMY");
        foundEnemy = enemy;
        //speed = originalSpeed * 2.0f;
        flock.finalSpeed = flock.originalFinalSpeed * 2.0f;
        hasSelectedWaypoint = false;
        hasReachedWaypoint = true;
        //torque.enabled = false;
        //cf.enabled = true;
        hasSelectedWaypoint = false;
        SwitchRadiusOff(radius.gameObject);
        SwitchRadiusOff(radius2.gameObject);
        SwitchRadiusOn(radius.gameObject);
        doOnce2 = false;
        outline.OutlineColor = dangerColor;
    }

    private void SuccessfullEscape()
    {
        //("SUCCESSFULL ESCAPE");
        //speed = originalSpeed;
        flock.finalSpeed = flock.originalFinalSpeed;
        foundEnemy = null;
        SwitchRadiusOff(findEnemyRadius.gameObject);
        SwitchRadiusOn(findEnemyRadius.gameObject);
        if(hasReachedWaypoint)
        {
            hasSelectedWaypoint = false;
            //torque.enabled = false;
            //cf.enabled = true;
            SwitchRadiusOff(radius.gameObject);
            SwitchRadiusOff(radius2.gameObject);
            SwitchRadiusOn(radius.gameObject);
        }
        doOnce2 = false;
        outline.OutlineColor = Color.white;
    }

    private void OnDestroy()
    {
        radius.OnRadiusFindWaypoint -= AddFoundWaypoint;
        radius2.OnRadiusFindWaypoint -= AddFoundWaypoint;
        findEnemyRadius.OnRadiusFindEnemy -= Escape;
    }
}
