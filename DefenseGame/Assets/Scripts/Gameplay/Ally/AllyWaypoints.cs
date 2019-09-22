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
    private ConstantForce constantForce;
    private GameManager gm;
    public AllyRadius radius;
    public AllyRadius radius2;
    public List<GameObject> waypointsFound;
    public bool hasReachedWaypoint;
    public bool hasSelectedWaypoint;
    //public bool switchOnce = false;
    //public bool switchOnce2 = false;
    //public bool switchOnce3 = false;
    public bool doOnce;
    public bool doOnce2;
    public GameObject selectedWaypoint;

    private void Start()
    {
        constantForce = GetComponent<ConstantForce>();
        gm = GameManager.Get();
        rig = GetComponent<Rigidbody>();
        radius.OnRadiusFindWaypoint += AddFoundWaypoint;
        radius2.OnRadiusFindWaypoint += AddFoundWaypoint;
        currentTarget = 0;
        torque = GetComponent<TorqueLookRotation>();
        torque.target = gm.playerWaypoints[currentTarget].transform;

        selectedWaypoint = gm.playerWaypoints[0];
        waypointsFound.Add(gm.playerWaypoints[0]);
        SwitchRadiusOff(radius.gameObject);
        SwitchRadiusOff(radius2.gameObject);
        hasSelectedWaypoint = true;
        constantForce.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(selectedWaypoint.transform.position, transform.position);

        if (distance <= 2.0f)
        {
            if(!doOnce)
            {
                Debug.Log("parte 0");
                hasReachedWaypoint = true;
                SwitchRadiusOn(radius.gameObject);
                SwitchRadiusOn(radius2.gameObject);
                doOnce2 = false;
                doOnce = true;
            }
        }
        //
        if (distance <= 1.0f)
        {
            if(!doOnce2)
            {
                if (CheckForRandomWaypoint())
                {
                    Debug.Log("parte 0.5");
                    constantForce.enabled = false;
                    torque.enabled = true;
                    SwitchRadiusOff(radius.gameObject);
                    SwitchRadiusOff(radius2.gameObject);
                    torque.target = selectedWaypoint.transform;
                    hasSelectedWaypoint = true;
                    doOnce = false;
                    //distance = 20;
                }
                else
                {
                    Debug.Log("parte 1");
                    torque.enabled = false;
                    constantForce.enabled = true;
                    hasSelectedWaypoint = false;
                    SwitchRadiusOff(radius.gameObject);
                    SwitchRadiusOff(radius2.gameObject);
                    SwitchRadiusOn(radius.gameObject);
                }

                doOnce2 = true;
            }
            
        }

        if(hasSelectedWaypoint)
        {
            Vector3 direction = (selectedWaypoint.transform.position - transform.position).normalized;
            rig.MovePosition(rig.position + direction * speed * Time.deltaTime);
        }
        else
        {
            //SwitchRadiusOn();
            //rig.AddTorque(new Vector3(0,0.5f,0),ForceMode.VelocityChange);
            constantForce.torque = new Vector3(0, 0.8f, 0);
        }
        
    }

    private void AddFoundWaypoint(GameObject newWaypoint)
    {
        Debug.Log("Initiate Finding waypoints");
        Debug.Log("Waypoint name : " + newWaypoint.name);
        bool isTheSameWaypoint = false;

        if(hasReachedWaypoint)
        {
            if(newWaypoint != selectedWaypoint)
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
                    Debug.Log("messi");

                    if (CheckForRandomWaypoint())
                    {
                        constantForce.enabled = false;
                        torque.enabled = true;
                        SwitchRadiusOff(radius.gameObject);
                        SwitchRadiusOff(radius2.gameObject);
                        torque.target = selectedWaypoint.transform;
                        hasSelectedWaypoint = true;
                        doOnce = false;
                        Debug.Log("parte 1 EX");
                    }
                    else
                    {
                        Debug.Log("parte 2");
                        torque.enabled = false;
                        constantForce.enabled = true;
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
        if(waypointsFound.Count != 0)
        {
            Debug.Log("Found waypoint!!!");
            selectedWaypoint = waypointsFound[Random.Range(0, waypointsFound.Count)];
            waypointsFound.Clear();
            return true;
        }
        
        return false;
    }

    private void SwitchRadiusOn(GameObject target)
    {
        Debug.Log("Switching On");
        target.gameObject.SetActive(true);
        //radius2.gameObject.SetActive(true);
    }

    private void SwitchRadiusOff(GameObject target)
    {
        Debug.Log("Switching Off");
        target.gameObject.SetActive(false);
        //radius2.gameObject.SetActive(false);
    }

    private void SearchNearestWaypoint()
    {

    }

    private void OnDestroy()
    {
        radius.OnRadiusFindWaypoint -= AddFoundWaypoint;
        radius2.OnRadiusFindWaypoint -= AddFoundWaypoint;
    }
}
