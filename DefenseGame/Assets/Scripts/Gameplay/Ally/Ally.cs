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
    public AllyRadius enemyHurt;
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
    private TorqueLookRotation torque;

    private void Start()
    {
        flock = GetComponent<Flock>();
        gm = GameManager.Get();
        rig = GetComponent<Rigidbody>();
        outline = GetComponent<Outline>();
        torque = GetComponent<TorqueLookRotation>();

        radius.OnRadiusFindWaypoint += AddFoundWaypoint;
        radius2.OnRadiusFindWaypoint += AddFoundWaypoint;
        findEnemyRadius.OnRadiusFindEnemy += SeeEnemy;
        enemyHurt.OnRadiusTouchEnemy += KillFairy;
        selectedWaypoint = gm.playerWaypoints[0];
        hasSelectedWaypoint = true;
        originalSpeed = speed;
        currentState = initialState;
        torque.target = gm.playerWaypoints[0].transform;

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
            default:
                break;
        }
    }

    private void Walk()
    {
        float distance = Vector3.Distance(selectedWaypoint.transform.position, transform.position);

        if (foundEnemy)
        {
            currentState = allyStates.escape;
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

        if (distance <= 1.3f)
        {
            if (!doOnce2)
            {
                if (CheckForRandomWaypoint())
                {
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

        Vector3 direction = (selectedWaypoint.transform.position - transform.position).normalized;
        rig.MovePosition(rig.position + direction * speed * Time.deltaTime);
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
        float distance = Vector3.Distance(selectedWaypoint.transform.position, transform.position);
        float distanceFromEnemy = 90.0f;

        if(foundEnemy)
        {
            distanceFromEnemy = Vector3.Distance(foundEnemy.transform.position, transform.position);
        }
        
        //
        if (distanceFromEnemy >= 15.0f)
        {
            flock.finalSpeed = flock.originalFinalSpeed;
            speed = originalSpeed;
            SuccessfullEscape();
        }

        if (distance <= 2.0f)
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

        if (distance <= 1.3f)
        {
            if (!doOnce2)
            {
                if (CheckForRandomWaypoint())
                {
                    torque.enabled = true;
                    SwitchRadiusOff(radius.gameObject);
                    SwitchRadiusOff(radius2.gameObject);
                    hasSelectedWaypoint = true;
                    torque.target = selectedWaypoint.transform;
                    doOnce = false;
                }
                else
                {
                    torque.enabled = false;
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

        Vector3 direction = (selectedWaypoint.transform.position - transform.position).normalized;
        rig.MovePosition(rig.position + direction * speed * Time.deltaTime);
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
                        torque.enabled = true;
                        SwitchRadiusOff(radius.gameObject);
                        SwitchRadiusOff(radius2.gameObject);
                        torque.target = selectedWaypoint.transform;
                        hasSelectedWaypoint = true;
                        doOnce = false;
                        doOnce2 = false;
                    }
                    else
                    {
                        torque.enabled = false;
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
           // flock.ApplyRules();
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

    private void SeeEnemy(GameObject enemy)
    {
        foundEnemy = enemy;
        flock.finalSpeed = flock.originalFinalSpeed * 2.0f;
        speed = originalSpeed * 2;
        hasSelectedWaypoint = false;
        hasReachedWaypoint = true;
        hasSelectedWaypoint = false;
        SwitchRadiusOff(radius.gameObject);
        SwitchRadiusOff(radius2.gameObject);
        SwitchRadiusOn(radius.gameObject);
        doOnce2 = false;
        outline.OutlineColor = dangerColor;
    }

    private void KillFairy(GameObject enemy)
    {
        /*if(GameManager.Get().fairies.Count > 0)
        {
            Destroy(GameManager.Get().fairies[0]);
            GameManager.Get().fairies.RemoveAt(0);
        }*/
    }

    private void SuccessfullEscape()
    {
        flock.finalSpeed = flock.originalFinalSpeed;
        speed = originalSpeed;
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
            torque.enabled = false;
            currentState = allyStates.search;
        }
        doOnce2 = false;
        outline.OutlineColor = Color.white;
    }

    private void OnDestroy()
    {
        radius.OnRadiusFindWaypoint -= AddFoundWaypoint;
        radius2.OnRadiusFindWaypoint -= AddFoundWaypoint;
        findEnemyRadius.OnRadiusFindEnemy -= SeeEnemy;
        enemyHurt.OnRadiusTouchEnemy -= KillFairy;
    }
}
