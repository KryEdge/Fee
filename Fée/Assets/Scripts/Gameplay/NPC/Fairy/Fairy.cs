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
    public delegate void OnFairyState(Color uiColor);
    public static OnFairyAction OnFairySpawn;
    public static OnFairyAction OnFairyDeath;
    public OnFairyAction OnFairyEaten;
    public static OnFairyState OnFairyEscaped;
    public static OnFairyState OnFairyDanger;

    [Header("General Settings")]
    public static bool isInmunityOn;
    public GameObject initialWaypoint;
    public allyStates initialState;
    public static LayerMask normalMask;
    public static LayerMask invulnerableMask;

    [Header("Speed Settings")]
    public float speed;
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
    public float distanceFromPlanet;
    public allyStates currentState;
    static public GameObject selectedWaypoint;
    static public GameObject oldWaypoint;
    static public GameObject currentEnemySpotted;

    //Private
    private Rigidbody rig;
    private TorqueLookRotation torque;
    private Outline outline;
    private GameManager gm;

    // Start is called before the first frame update
    private void Start()
    {
        normalMask = 17;
        invulnerableMask = 24;

        gm = GameManager.Get();
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
        gm.currentFairies++;
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
        CheckDistanceFromPlanet();
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
        FlockManager.goalPosition = selectedWaypoint.transform.position;
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
            if (OnFairyDanger != null)
            {
                OnFairyDanger(dangerColor);
            }
        }
    }

    private void ForceStartEscape()
    {
        StartEscape(oldWaypoint);
    }

    private void EndEscape()
    {
        currentEnemySpotted = null;
        Flock.finalSpeed = Flock.originalFinalSpeed;
        currentColor = normalColor;
        if (OnFairyEscaped != null)
        {
            OnFairyEscaped(Color.white);
        }
    }

    private void CheckEnemySpotted(GameObject enemy, int pointsToGive)
    {
        if (enemy == currentEnemySpotted)
        {
            EndEscape();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "enemy":
                if (gm.canBeDamaged)
                {
                    if (!isInmunityOn)
                    {
                        gm.canBeDamaged = false;
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

        gm.currentFairies--;

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
    }

    public void ChangeSpeed(float newSpeed)
    {
        Flock.finalSpeed = newSpeed;
        Flock.originalFinalSpeed = newSpeed;
    }

    public void TeleportFairy()
    {
        List<Fairy> fairies = FlockManager.fairiesProperties;

        foreach (Fairy item in fairies)
        {
            if(item)
            {
                if (item.distanceFromPlanet < 100)
                {
                    transform.position = item.transform.position;
                    Debug.Log("TELEPORTED");
                }
            }
        }

    }

    public void CheckDistanceFromPlanet()
    {
        distanceFromPlanet = Vector3.Distance(transform.position,gm.planet.transform.position);

        if(distanceFromPlanet >= 100)
        {
           TeleportFairy();
        }
    }
}
