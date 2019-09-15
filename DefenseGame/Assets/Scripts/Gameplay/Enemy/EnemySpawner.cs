using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyTemplate;
    public KeyCode keyNumber;
    public float minSpeed;
    public float maxSpeed;
    public float minTime;
    public float maxTime;

    private float spawnTimer;
    private float finalSpawnTime;
    public int firstSpawnWaypoint;
    public int finalSpawnWaypoint;
    public int entryWaypoint;

    private Enemy enemyProperties;
    private void Start()
    {
        enemyProperties = enemyTemplate.GetComponent<Enemy>();
        SetRandomSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if(spawnTimer >= finalSpawnTime)
        {
            SpawnEnemy();
            SetRandomSpawnTime();
            spawnTimer = 0;
        }

        if(Input.GetKeyDown(keyNumber))
        {
            SpawnEnemy();
        }
    }

    private void SetRandomSpawnTime()
    {
        finalSpawnTime = Random.Range(minTime, maxTime);
    }

    private void SpawnEnemy()
    {
        enemyProperties.speed = Random.Range(minSpeed, maxSpeed);
        enemyProperties.firstSpawnWaypoint = firstSpawnWaypoint;
        enemyProperties.finalSpawnWaypoint = finalSpawnWaypoint;
        enemyProperties.entryWaypoint = entryWaypoint;
        GameObject newEnemy = Instantiate(enemyTemplate);
        newEnemy.SetActive(true);
    }
}