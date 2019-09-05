using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bulletTemplate;
    public float fireRate;
    public LayerMask Mask;

    private Bullet bulletProperties;
    private bool shootOnce;
    private bool canShoot;
    private float fireRateTimer;
    // Start is called before the first frame update
    void Start()
    {
        bulletProperties = bulletTemplate.GetComponent<Bullet>();
    }

    private void Update()
    {
        fireRateTimer += Time.deltaTime;

        if (fireRateTimer >= fireRate)
        {
            canShoot = true;
            fireRateTimer = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (canShoot)
            {
                shootOnce = false;
                canShoot = false;
                ShootWeapon();
            }

        }
    }

    private void ShootWeapon()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin,ray.direction,out hit,50,Mask))
        {
            if(hit.transform.gameObject.tag != "explosion")
            {
                if (!shootOnce)
                {
                    bulletProperties.isFired = true;
                    bulletProperties.target = hit.point;
                    GameObject newBullet = Instantiate(bulletTemplate);
                    newBullet.SetActive(true);
                    shootOnce = true;
                }
            }
            
        }
    }
}
