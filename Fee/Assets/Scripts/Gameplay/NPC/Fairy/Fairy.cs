using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy : MonoBehaviour
{
    public enum allyStates
    {
        idle,
        move,
        allStates
    }

    public delegate void OnFairyAction();
    public static OnFairyAction OnFairySpawn;
    public static OnFairyAction OnFairyDeath;
    public OnFairyAction OnFairyEaten;

    [Header("General Settings")]
    public static bool isInmunityOn;
    public GameObject initialWaypoint;
    public allyStates initialState;

    [Header("Damage Settings")]
    public float invincibilityMaxTime;
    static public bool canBeDamaged = true;

    [Header("Speed Settings")]
    public float speed;
    //public float speedMultiplier;
    public float runSpeedMultiplier;

    [Header("Outline Color Settings")]
    public Color normalColor;
    public Color dangerColor;
    static public Color currentColor;

    [Header("Distance Settings")]
    [Range(0, 3)]
    public float distanceToStop;
    [Range(1, 30)]
    public float escapeDistance;

    [Header("Assign GameObjects/Components")]
    public NPCRadius radius;
    public GameObject offset;
    public Animator animator;

    [Header("Checking Private Variables")]
    public allyStates currentState;
    static public GameObject selectedWaypoint;
    static public GameObject oldWaypoint;
    static public GameObject currentEnemySpotted;
    static public float invincibilityTimer;

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
        animator = GetComponentInChildren<Animator>();

        radius.OnRadiusFindEnemy += StartEscape;
        Enemy.OnDeath += CheckEnemySpotted;
        OnFairyDeath += ForceStartEscape;
        GameManager.OnLevelEndWave += EndEscape;

        currentState = initialState;
        selectedWaypoint = initialWaypoint;

        SwitchRotationTarget();
        currentColor = normalColor;
        outline.OutlineColor = currentColor;
        GameManager.Get().currentFairies++;
        //GameManager.Get().UpdateUI();
        animator.speed = Random.Range(0.8f, 1.4f);


        if(OnFairySpawn != null)
        {
            OnFairySpawn();
        }
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

        outline.OutlineColor = currentColor;

        if (!canBeDamaged)
        {
            invincibilityTimer += Time.deltaTime;

            if (invincibilityTimer >= invincibilityMaxTime)
            {
                canBeDamaged = true;
                invincibilityTimer = 0;
            }
        }
    }

    private void Move()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isIdle", false);

        float distance = Vector3.Distance(selectedWaypoint.transform.position, transform.position);
        float enemyDistance = 100.0f;

        if (distance <= distanceToStop)
        {
            ChangeWaypoint();
        }

        if (currentEnemySpotted)
        {
            enemyDistance = Vector3.Distance(currentEnemySpotted.transform.position, transform.position);

            if (enemyDistance >= escapeDistance)
            {
                EndEscape();
            }
        }
    }

    private void ChangeWaypoint()
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

    private void SwitchRotationTarget()
    {
        //Debug.Log("This waypoint: " + selectedWaypoint.name);
        FlockManager.goalPosition = selectedWaypoint.transform.position;
        //torque.target = selectedWaypoint.transform;
    }

    private void StartEscape(GameObject enemy)
    {
        if (currentEnemySpotted != enemy)
        {
            float enemyCurrentDistance = 0;

            if (enemy)
            {
                enemyCurrentDistance = Vector3.Distance(enemy.transform.position, transform.position);
            }
            
            //Debug.Log("enemy spotted!");

            if (currentEnemySpotted == null)
            {
                if (enemyCurrentDistance >= 4.0f)
                {
                    GameObject aux = selectedWaypoint;
                    selectedWaypoint = oldWaypoint;
                    oldWaypoint = aux;

                    FlockManager.goalPosition = selectedWaypoint.transform.position;
                }
            }

            currentEnemySpotted = enemy;

            Flock.finalSpeed = Flock.originalFinalSpeed * runSpeedMultiplier;
            currentColor = dangerColor;
            Debug.Log("Final Speed :  " + Flock.finalSpeed);
        }
    }

    private void ForceStartEscape()
    {
        StartEscape(oldWaypoint);
    }

    private void EndEscape()
    {
        //Debug.Log("ESCAPED!");
        currentEnemySpotted = null;
        Flock.finalSpeed = Flock.originalFinalSpeed;
        currentColor = normalColor;
    }

    private void CheckEnemySpotted(GameObject enemy, int pointsToGive)
    {
        if (enemy == currentEnemySpotted)
        {
            EndEscape();
        }
    }

    //Fairy Death

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "enemy":
                if (canBeDamaged)
                {
                    if (!isInmunityOn)
                    {
                        canBeDamaged = false;
                        FlockManager.fairies.Remove(gameObject);
                        Destroy(gameObject);
                    }
                }
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        radius.OnRadiusFindEnemy -= StartEscape;
        Enemy.OnDeath -= CheckEnemySpotted;
        GameManager.OnLevelEndWave -= EndEscape;

        GameManager.Get().currentFairies--;
        //GameManager.Get().UpdateUI();

        if (OnFairyDeath != null)
        {
            OnFairyDeath();
        }

        OnFairyDeath -= ForceStartEscape;
    }

    private void ForceEndEscape()
    {
        EndEscape();
    }

    public static void SwitchInvincibility()
    {
        isInmunityOn = !isInmunityOn;
        Debug.Log(isInmunityOn);
    }

    public void ChangeSpeed(float newSpeed)
    {
        Flock.finalSpeed = newSpeed;
        Flock.originalFinalSpeed = newSpeed;
    }
}
