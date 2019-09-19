using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public int bulletSpeed;
    public GameObject proyectileTemplate;
    public float fireRate;
    public float lifespan;
    public bool canShoot;
    public bool isPreview;

    private GameObject currentTarget;
    private float lifespanTimer;
    private float fireRateTimer;
    private Proyectile proyectile;
    private TurretRadius turretRadius;
    private Rigidbody rig;
    // Start is called before the first frame update
    void Start()
    {
        turretRadius = transform.GetChild(0).gameObject.GetComponent<TurretRadius>();
        proyectile = proyectileTemplate.GetComponent<Proyectile>();
        proyectile.speed = bulletSpeed;
        turretRadius.onTurretDetectEnemy = SetTarget;
        turretRadius.onTurretLostEnemy = ChangeTarget;
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        lifespanTimer += Time.deltaTime;
        
        if(lifespanTimer >= lifespan)
        {
            Destroy(gameObject);
        }

        if(!canShoot)
        {
            fireRateTimer += Time.deltaTime;

            if(fireRateTimer >= fireRate)
            {
                fireRateTimer = 0;
                canShoot = true;
            }
        }
        else
        {
            ShootTarget();
        }
    }

    private void SetTarget(GameObject newTarget)
    {
        Debug.Log("Seteando Target");
        currentTarget = newTarget;
    }

    private void ShootTarget()
    {
        if(currentTarget)
        {
            GameObject newProyectile = Instantiate(proyectileTemplate);
            Proyectile proy = newProyectile.GetComponent<Proyectile>();
            proy.startPosition = transform.position + transform.up * 7;
            proy.target = currentTarget;
            newProyectile.SetActive(true);
            canShoot = false;
        }
    }

    private void ChangeTarget(GameObject newTarget)
    {
        if(newTarget == currentTarget)
        {
            turretRadius.gameObject.SetActive(false);
            proyectile.target = null;
            turretRadius.gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "planet")
        {
            rig.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
