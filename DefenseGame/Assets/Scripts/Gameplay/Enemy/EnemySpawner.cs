using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyTemplate;
    public KeyCode keyNumber;
    public float minSpeed;
    public float maxSpeed;

    private Enemy enemyProperties;
    private void Start()
    {
        enemyProperties = enemyTemplate.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyNumber))
        {
            enemyProperties.speed = Random.Range(minSpeed, maxSpeed);
            GameObject newEnemy = Instantiate(enemyTemplate);
            newEnemy.SetActive(true);
        }
    }
}