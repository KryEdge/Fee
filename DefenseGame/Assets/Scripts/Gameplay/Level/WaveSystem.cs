using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviourSingleton<WaveSystem>
{
    /*[System.Serializable]
    public class Wave
    {
        int waveNumber;
        List<EnemySpawner> typeOfEnemies;
        List<GameObject> spawnPoints;
        int maxEnemies;
    }*/

    public List<EnemySpawner> spawners;
    public List<int> maxEnemies;
    public float maxWaitTime;
    public int enemiesAdd;
    public int currentWave;
    public bool timerHasStarted;

    public float waitingTimer;

    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Get().waves = this;
        maxEnemies.Add(enemiesAdd);
        GameManager.Get().maxEnemies = enemiesAdd;
        currentWave = maxEnemies.Count;
    }

    private void Update()
    {
        if(timerHasStarted)
        {
            waitingTimer += Time.deltaTime;

            if(waitingTimer >= maxWaitTime)
            {
                waitingTimer = 0;
                timerHasStarted = false;
                StartWave();
            }
        }
    }

    public void StartWave()
    {
        foreach (EnemySpawner spawner in spawners)
        {
            spawner.automaticSpawn = true;
        }
    }

    public void StopWave()
    {
        foreach (EnemySpawner spawner in spawners)
        {
            spawner.automaticSpawn = false;
        }

        timerHasStarted = true;
    }

    public void SetNextWave()
    {
        maxEnemies.Add(maxEnemies[maxEnemies.Count - 1] + enemiesAdd);
        currentWave = maxEnemies.Count;
    }
}
