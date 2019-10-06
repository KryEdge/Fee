using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAlly : MonoBehaviour
{
    public enum allyStates
    {
        idle,
        move,
        allStates
    }

    [Header("General Settings")]
    public GameObject initialWaypoint;
    public allyStates initialState;

    [Header("Damage Settings")]
    public float invincibilityMaxTime;
    static public bool canBeDamaged = true;

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
    public AllyRadius radius;

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

        radius.OnRadiusFindEnemy += StartEscape;
        NewEnemy.OnDeath += CheckEnemySpotted;

        currentState = initialState;
        selectedWaypoint = initialWaypoint;

        SwitchRotationTarget();
        currentColor = normalColor;
        outline.OutlineColor = currentColor;
        GameManager.Get().currentFairies++;
        GameManager.Get().UpdateUI();
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

        if(!canBeDamaged)
        {
            invincibilityTimer += Time.deltaTime;

            if(invincibilityTimer >= invincibilityMaxTime)
            {
                canBeDamaged = true;
                invincibilityTimer = 0;
            }
        }
    }

    private void Move()
    {
        float distance = Vector3.Distance(selectedWaypoint.transform.position, transform.position);
        float enemyDistance = 100.0f;

        if(distance <= distanceToStop)
        {
            ChangeWaypoint();
        }

        if(currentEnemySpotted)
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
        Debug.Log("Applying new Target");
        FlockManager.goalPosition = selectedWaypoint.transform.position;
    }

    private void StartEscape(GameObject enemy)
    {
        if(currentEnemySpotted != enemy)
        {
            float enemyCurrentDistance = Vector3.Distance(enemy.transform.position, transform.position);
            Debug.Log("enemy spotted!");

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
        }
    }

    private void EndEscape()
    {
        Debug.Log("ESCAPED!");
        currentEnemySpotted = null;
        Flock.finalSpeed = Flock.originalFinalSpeed;
        currentColor = normalColor;
    }

    private void CheckEnemySpotted(GameObject enemy)
    {
        if(enemy == currentEnemySpotted)
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
                    canBeDamaged = false;
                    Destroy(gameObject);
                }
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        radius.OnRadiusFindEnemy -= StartEscape;
        NewEnemy.OnDeath -= CheckEnemySpotted;
        GameManager.Get().currentFairies--;
        GameManager.Get().UpdateUI();
    }
}
