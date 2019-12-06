using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum enemyStates
    {
        idle,
        move,
        eating,
        dead,
        allStates
    }

    public delegate void OnAction(GameObject enemy, int pointsToGive);
    public static OnAction OnDeath;

    [Header("General Settings")]
    public GameObject initialWaypoint;
    public Shader deathShader;
    public enemyStates initialState;
    public int pointsToGive;

    [Header("Death Settings")]
    public float deathSpeed;
    private float deathTime;
    private float deathTimer;
    private int deathLayer;

    [Header("Speed Settings")]
    public float speed;
    public float runSpeedMultiplier;

    [Header("Goo Settings")]
    public GameObject gooParent;
    public float splatTime;
    public float splatTimer;

    [Header("Eat Settings")]
    public float eatingTime;
    public float eatingTimer;

    [Header("Distance Settings")]
    [Range(0, 3)]
    public float distanceToStop;
    [Range(1, 30)]
    public float npcLostDistance;

    [Header("Sound Settings")]
    public bool isSmallEnemy;
    public GameObject deathTowerSound;
    public GameObject deathMeteorSound;
    public GameObject smallDeathTowerSound;
    public GameObject smallDeathMeteorSound;

    [Header("Assign Variables/Components")]
    public GameObject splatTemplate;
    public Animator animator;
    public SkinnedMeshRenderer attachedModel;
    public NPCRadius radius;
    public Fairy ally;

    [Header("Checking Private Variables")]
    public enemyStates currentState;
    public GameObject currentTarget;
    public GameObject selectedWaypoint;
    public GameObject oldWaypoint;
    public GameObject nextWaypointTarget;
    public float finalSpeed;
    public float originalDistanceToStop;
    public float finalDistanceToStop;
    private float npcDistance;

    //Private
    private Collider killer;
    private Collider enemyCollider;
    private Rigidbody rig;
    private TorqueLookRotation torque;
    private Outline outline;
    private bool hasAlreadyDied;
    private bool doOnce;

    // Start is called before the first frame update
    private void Start()
    {
        npcDistance = 99;
        deathLayer = 24;
        rig = GetComponent<Rigidbody>();
        torque = GetComponent<TorqueLookRotation>();
        outline = GetComponent<Outline>();
        enemyCollider = GetComponent<Collider>();

        radius.OnRadiusFindAlly += StartPursuit;

        currentState = initialState;
        selectedWaypoint = initialWaypoint;

        SwitchRotationTarget();

        finalSpeed = speed;
        originalDistanceToStop = distanceToStop;
        finalDistanceToStop = originalDistanceToStop;
        GameManager.Get().enemies.Add(gameObject);
        deathTimer = -1;
        deathTime = 1;

        animator.SetBool("IsWalking", true);
        animator.SetBool("IsEating", false);
    }

    // Update is called once per frame
    private void Update()
    {
        switch (currentState)
        {
            case enemyStates.idle:
                break;
            case enemyStates.move:
                Move();
                break;
            case enemyStates.eating:
                Eat();
                break;
            case enemyStates.dead:
                Die();
                break;
            default:
                break;
        }


        if(killer)
        {
            if (killer.bounds.Intersects(enemyCollider.bounds))
            {
                if(!doOnce)
                {
                    if (!hasAlreadyDied)
                    {
                        if (OnDeath != null)
                        {
                            OnDeath(gameObject, pointsToGive);
                        }
                        attachedModel.material.shader = deathShader;
                        gameObject.layer = deathLayer;
                        gameObject.tag = "dead";
                        currentState = enemyStates.dead;

                        hasAlreadyDied = true;
                        animator.SetTrigger("Die");

                        if(isSmallEnemy)
                        {
                            AkSoundEngine.PostEvent("monstruo_2_muere_meteoro", smallDeathMeteorSound);
                        }
                        else
                        {
                            AkSoundEngine.PostEvent("monstruo_1_muere_meteoro", deathMeteorSound);
                        }
                        
                    }

                    doOnce = true;
                }
                Debug.Log("Bounds intersecting");
            }
        }

        if(splatTemplate)
        {
            splatTimer += Time.deltaTime;

            if(splatTimer >= splatTime)
            {
                splatTimer = 0;
                //Instantiate(splatTemplate);
                //Instantiate(splatTemplate, gooParent.transform, false);
                GameObject splat = Instantiate(splatTemplate, gooParent.transform.position, gooParent.transform.rotation);
            }
        }
        
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case enemyStates.idle:
                break;
            case enemyStates.move:
                MoveFixedUpdate();
                break;
            case enemyStates.eating:
                ///Eat();
                break;
            case enemyStates.dead:
                //Die();
                break;
            default:
                break;
        }
    }

    public void StartEating()
    {
        currentState = enemyStates.eating;
        finalSpeed = 0;

        animator.SetBool("IsWalking", false);
        animator.SetBool("IsEating", true);
    }

    private void Eat()
    {
        eatingTimer += Time.deltaTime;

        if(eatingTimer >= eatingTime)
        {
            eatingTimer = 0;
            finalSpeed = speed;
            currentState = enemyStates.move;

            animator.SetBool("IsWalking", true);
            animator.SetBool("IsEating", false);

            SwitchRotationTarget();
        }
    }

    private void Die()
    {
        deathTimer += Time.deltaTime * deathSpeed;

        attachedModel.material.SetFloat("_dissolve", deathTimer);

        if (deathTimer >= deathTime)
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        float distance = Vector3.Distance(selectedWaypoint.transform.position, transform.position);
        npcDistance = 100.0f;

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
            nextWaypointTarget = Fairy.selectedWaypoint;
        }
        
    }

    private void SwitchRotationTarget()
    {
        torque.target = selectedWaypoint.transform;
    }

    private void StartPursuit(GameObject npc)
    {
        if (!currentTarget)
        {
            currentTarget = npc;
            ally = currentTarget.GetComponent<Fairy>();
            nextWaypointTarget = Fairy.selectedWaypoint;
            finalSpeed = speed * runSpeedMultiplier;
        }
    }

    private void EndPursuit()
    {
        currentTarget = null;
        nextWaypointTarget = null;
        finalSpeed = speed;
        finalDistanceToStop = originalDistanceToStop;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "npc")
        {
            if(GameManager.Get().canBeDamaged)
            {
                StartEating();
            }
        }
        
        if (collision.gameObject.tag == "proyectile")
        {
            if (!hasAlreadyDied)
            {
                if (OnDeath != null)
                {
                    OnDeath(gameObject, pointsToGive);
                }
                gameObject.layer = deathLayer;
                gameObject.tag = "dead";
                currentState = enemyStates.dead;
                attachedModel.material.shader = deathShader;
                Destroy(collision.gameObject);
                hasAlreadyDied = true;

                animator.SetTrigger("Die");
                if(isSmallEnemy)
                {
                    AkSoundEngine.PostEvent("monstruo_2_muere_torre", smallDeathTowerSound);
                }
                else
                {
                    AkSoundEngine.PostEvent("monstruo_1_muere_torre", deathTowerSound);
                }
                
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "explosion":
                killer = other.GetComponent<Collider>();
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

    private void MoveFixedUpdate()
    {
        if (npcDistance <= 4.0f)
        {
            if(currentTarget)
            {
                torque.target = currentTarget.transform;
                selectedWaypoint = Fairy.selectedWaypoint;
                finalDistanceToStop = originalDistanceToStop * 8.0f;
                Vector3 direction = (ally.offset.transform.position - transform.position).normalized;
                rig.MovePosition(rig.position + direction * finalSpeed * Time.deltaTime);
            }
        }
        else
        {
            finalDistanceToStop = originalDistanceToStop;
            Vector3 direction = (selectedWaypoint.transform.position - transform.position).normalized;
            rig.MovePosition(rig.position + direction * finalSpeed * Time.deltaTime);
        }
    }
}
