using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bulletTemplate;

    private Bullet bulletProperties;
    // Start is called before the first frame update
    void Start()
    {
        Enemy.OnEnemyClicked = ShootEnemy;
        bulletProperties = bulletTemplate.GetComponent<Bullet>();
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            ShootWeapon();
        }
    }

    void ShootEnemy(GameObject enemy)
    {
        
    }

    private void ShootWeapon()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            bulletProperties.target = hit.point;
            GameObject newBullet = Instantiate(bulletTemplate);
            newBullet.SetActive(true);

            Debug.Log(hit.point);
        }
    }
}
