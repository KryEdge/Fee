using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{
    public enum allyStates
    {
        idle,
        search,
        walk,
        escape,
        allStates
    }


    [Header("Ally Settings")]
    public allyStates initialState;
    public allyStates currentState;
    public float speed;
    public float originalSpeed;
    public Color dangerColor;

    [Header("Assign References")]
    
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
    private GameManager gm;
    private Vector3 destination;
    private float distanceToStop;
    private Outline outline;

    private void Start()
    {
        flock = GetComponent<Flock>();
        gm = GameManager.Get();
        rig = GetComponent<Rigidbody>();
        outline = GetComponent<Outline>();

        radius.OnRadiusFindWaypoint += AddFoundWaypoint;
        radius2.OnRadiusFindWaypoint += AddFoundWaypoint;
        findEnemyRadius.OnRadiusFindEnemy += Escape;
        selectedWaypoint = gm.playerWaypoints[0];
        hasSelectedWaypoint = true;
        originalSpeed = speed;
        currentState = initialState;

        FlockManager.goalPosition = gm.playerWaypoints[0].transform.position;

        waypointsFound.Add(gm.playerWaypoints[0]);
        SwitchRadiusOff(radius.gameObject);
        SwitchRadiusOff(radius2.gameObject);
    }


    // Update is called once per frame
    private void Update()
    {
        switch (currentState)
        {
            case allyStates.idle:
                break;
            case allyStates.search:
                Search();
                break;
            case allyStates.walk:
                Walk();
                break;
            case allyStates.escape:
                Escape();
                break;
            case allyStates.allStates:
                break;
            default:
                break;
        }
    }

    private void Walk()
    {
        float distance = Vector3.Distance(selectedWaypoint.transform.position, transform.position);

        if (foundEnemy)
        {
            currentState = allyStates.search;
        }

        if (distance <= 2.0f)
        {
            if (!foundEnemy)
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
            if (!doOnce2)
            {
                if (CheckForRandomWaypoint())
                {
                    SwitchRadiusOff(radius.gameObject);
                    SwitchRadiusOff(radius2.gameObject);
                    hasSelectedWaypoint = true;
                    doOnce = false;
                }
                else
                {
                    hasSelectedWaypoint = false;
                    SwitchRadiusOff(radius.gameObject);
                    SwitchRadiusOff(radius2.gameObject);
                    SwitchRadiusOn(radius.gameObject);
                    randomDirection = Random.Range(0, 2);
                    currentState = allyStates.search;
                }

                doOnce2 = true;
            }
        }
    }

    private void Search()
    {
        if (randomDirection == 0)
        {
            transform.Rotate(new Vector3(0, 0.8f, 0));
        }
        else if (randomDirection == 1)
        {
            transform.Rotate(new Vector3(0, -0.8f, 0));
        }
    }

    private void Escape()
    {
        float distanceFromEnemy = 90.0f;

        distanceFromEnemy = Vector3.Distance(foundEnemy.transform.position, transform.position);

        if (distanceFromEnemy >= 15.0f)
        {
            flock.finalSpeed = flock.originalFinalSpeed;
            SuccessfullEscape();
        }
    }

    private void AddFoundWaypoint(GameObject newWaypoint)
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

                if (currentState == allyStates.search)
                {
                    if (CheckForRandomWaypoint())
                    {
                        SwitchRadiusOff(radius.gameObject);
                        SwitchRadiusOff(radius2.gameObject);
                        hasSelectedWaypoint = true;
                        doOnce = false;
                        doOnce2 = false;
                    }
                    else
                    {
                        hasSelectedWaypoint = false;
                        SwitchRadiusOff(radius.gameObject);
                        SwitchRadiusOff(radius2.gameObject);
                        SwitchRadiusOn(radius.gameObject);
                    }

                    if(foundEnemy)
                    {
                        currentState = allyStates.escape;
                    }
                    else
                    {
                        currentState = allyStates.walk;
                    }
                }
            }
        }


    }

    private bool CheckForRandomWaypoint()
    {
        if (waypointsFound.Count != 0)
        {
            selectedWaypoint = waypointsFound[Random.Range(0, waypointsFound.Count)];
            FlockManager.goalPosition = selectedWaypoint.transform.position;
            flock.ApplyRules();
            waypointsFound.Clear();
            currentState = allyStates.walk;
            return true;
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

    private void Escape(GameObject enemy)
    {
        foundEnemy = enemy;
        flock.finalSpeed = flock.originalFinalSpeed * 2.0f;
        hasSelectedWaypoint = false;
        hasReachedWaypoint = true;
        hasSelectedWaypoint = false;
        SwitchRadiusOff(radius.gameObject);
        SwitchRadiusOff(radius2.gameObject);
        SwitchRadiusOn(radius.gameObject);
        doOnce2 = false;
        outline.OutlineColor = dangerColor;
    }

    private void SuccessfullEscape()
    {
        flock.finalSpeed = flock.originalFinalSpeed;
        foundEnemy = null;
        SwitchRadiusOff(findEnemyRadius.gameObject);
        SwitchRadiusOn(findEnemyRadius.gameObject);
        if(hasReachedWaypoint)
        {
            hasSelectedWaypoint = false;
            SwitchRadiusOff(radius.gameObject);
            SwitchRadiusOff(radius2.gameObject);
            SwitchRadiusOn(radius.gameObject);
            currentState = allyStates.walk;
        }
        else
        {
            currentState = allyStates.search;
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
