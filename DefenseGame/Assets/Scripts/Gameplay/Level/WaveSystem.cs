using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Text waveText;
    public List<EnemySpawner> spawners;
    public List<int> maxEnemies;
    public float maxWaitTime;
    public float textScreenTime;
    public int enemiesAdd;
    public int currentWave;
    public bool timerHasStarted;
    public bool screenTimerStarted;
    //public bool waveIsOff;
    

    public float waitingTimer;
    public float textTimer;

    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Get().waves = this;
        maxEnemies.Add(enemiesAdd);
        GameManager.Get().maxEnemies = enemiesAdd;
        currentWave = maxEnemies.Count;
        StopWave();
        waveText.text = "Game is about to Start!";
        timerHasStarted = true;
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

        if(screenTimerStarted)
        {
            textTimer += Time.deltaTime;

            if(textTimer >= textScreenTime)
            {
                waveText.enabled = false;
                screenTimerStarted = false;
                textTimer = 0;
            }
        }
    }

    public void StartWave()
    {
        foreach (EnemySpawner spawner in spawners)
        {
            spawner.automaticSpawn = true;
        }

        waveText.enabled = true;
        waveText.text = "Wave Start!";
        screenTimerStarted = true;
        textTimer = 0;
    }

    public void StopWave()
    {
        foreach (EnemySpawner spawner in spawners)
        {
            spawner.automaticSpawn = false;
        }

        waveText.enabled = true;
        timerHasStarted = true;
        waveText.text = "Wave Ended...";
        screenTimerStarted = true;
        textTimer = 0;
    }

    public void SetNextWave()
    {
        maxEnemies.Add(maxEnemies[maxEnemies.Count - 1] + enemiesAdd);
        currentWave = maxEnemies.Count;
    }
}
