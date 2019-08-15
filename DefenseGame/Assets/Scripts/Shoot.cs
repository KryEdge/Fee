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

    void ShootEnemy(GameObject enemy)
    {
        bulletProperties.target = enemy;
        GameObject newBullet = Instantiate(bulletTemplate);
        newBullet.SetActive(true);
    }
}
