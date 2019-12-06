using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public delegate void OnTurretAction(GameObject turret);
    public OnTurretAction OnTurretDead;

    [Header("General Settings")]
    public GameObject attachedVisionRadius;
    public GameObject attachedParticles;
    public SkinnedMeshRenderer attachedModel;
    public Animator attachedAnimator;
    public GameObject proyectileTemplate;
    public int bulletSpeed;
    public float spawnSpeed;
    public float fireRate;
    public float lifespan;
    public float exitDistance;
    public float distance = 0;

    [Header("Sound Settings")]
    public GameObject shootSound;

    [Header("Checking Variables")]
    public TurretZone zone;
    public List<GameObject> enteredZones;
    public List<GameObject> enteredTurrets;
    public TurretRadius turretRadius;
    public UITowersState stateUI;
    public float lifespanTimer;
    public bool canShoot;
    public bool isPreview;
    public bool canBePlaced;
    public bool isInTurretZone;
    public bool isSpawned;
    public bool isDying;

    private Outline outline;
    private GameObject currentTarget;
    private Proyectile proyectile;
    private Rigidbody rig;
    private float fireRateTimer;
    private float generateTimer;

    // Start is called before the first frame update
    private void Start()
    {
        outline = GetComponent<Outline>();
        proyectile = proyectileTemplate.GetComponent<Proyectile>();
        proyectile.speed = bulletSpeed;
        turretRadius.onTurretDetectEnemy = SetTarget;
        turretRadius.onTurretLostEnemy = ChangeTarget;
        rig = GetComponent<Rigidbody>();

        generateTimer = 1;
    }

    // Update is called once per frame
    private void Update()
    {
        if(!isPreview)
        {
            if(currentTarget)
            {
                distance = Vector3.Distance(transform.position, currentTarget.transform.position);
            }
            else
            {
                distance = exitDistance;
            }

            if(distance >= exitDistance)
            {
                outline.OutlineColor = Color.white;
            }

            lifespanTimer += Time.deltaTime;

            if (lifespanTimer >= lifespan)
            {
                if(OnTurretDead != null)
                {
                    OnTurretDead(gameObject);
                    isDying = true;
                    attachedAnimator.SetTrigger("die");
                    attachedParticles.SetActive(false);
                }
            }

            if (!canShoot)
            {
                fireRateTimer += Time.deltaTime;

                if (fireRateTimer >= fireRate)
                {
                    fireRateTimer = 0;
                    canShoot = true;
                }
            }
            else
            {
                ShootTarget();
            }

            if(isSpawned)
            {
                generateTimer -= Time.deltaTime * spawnSpeed;

                attachedModel.material.SetFloat("_dissolve", generateTimer);

                if (generateTimer <= -1)
                {
                    generateTimer = -1;
                    isSpawned = false;
                }
            }

            if(isDying)
            {
                generateTimer += Time.deltaTime * spawnSpeed;

                attachedModel.material.SetFloat("_dissolve", generateTimer);

                if (generateTimer >= 1)
                {
                    generateTimer = 1;
                    Destroy(gameObject);
                }
            }
        }
    }

    private void SetTarget(GameObject newTarget)
    {
        if (!isPreview)
        {
            outline.OutlineColor = Color.red;
            currentTarget = newTarget;
        }           
    }

    private void ShootTarget()
    {
        if (!isPreview)
        {
            if (currentTarget)
            {
                if(currentTarget.gameObject.tag != "dead")
                {
                    GameObject newProyectile = Instantiate(proyectileTemplate);
                    Proyectile proy = newProyectile.GetComponent<Proyectile>();
                    proy.startPosition = transform.position + transform.up * 15;
                    proy.target = currentTarget;
                    newProyectile.SetActive(true);
                    canShoot = false;
                    AkSoundEngine.PostEvent("torre_lanza", shootSound);
                }
            }
        }
        
    }

    private void ChangeTarget(GameObject newTarget)
    {
        if (!isPreview)
        {
            if (newTarget == currentTarget)
            {
                turretRadius.gameObject.SetActive(false);
                proyectile.target = null;
                turretRadius.gameObject.SetActive(true);
            }
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "planet")
        {
            rig.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "turretZone")
        {
            if(!other.GetComponent<TurretZone>().isUsed)
            {
                enteredZones.Add(other.gameObject);

                if (enteredZones.Count == 1)
                {
                    isInTurretZone = true;

                    if (!other.GetComponent<TurretZone>().isUsed)
                    {
                        Debug.Log(other.gameObject.name + " isUsed: " + other.GetComponent<TurretZone>().isUsed);
                        canBePlaced = true;
                        zone = other.GetComponent<TurretZone>();
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "turretZone")
        {
            enteredZones.Remove(other.gameObject);
            if (enteredZones.Count <= 0)
            {
                isInTurretZone = false;
                canBePlaced = false;
            }
        }
    }

    public void TurnOffOutline()
    {
        outline.enabled = false;
    }

    public void CheckIfCanBePlaced()
    {
        if (enteredTurrets.Count <= 0)
        {
            canBePlaced = true;
        }
    }

    private void OnDestroy()
    {
        if(zone)
        {
            zone.isUsed = false;
        }

        if(stateUI)
        {
            stateUI.isBeingUsed = false;
        }
    }
}
