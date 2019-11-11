using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviourSingleton<WaveSystem>
{
    public delegate void OnWaveAction();
    public static OnWaveAction OnStartWave;
    public static OnWaveAction OnStartWaveFirstTime;
    public static OnWaveAction OnEndUI;

    public delegate void OnUIAction(int id);
    public static OnUIAction OnStartUI;
    public static OnUIAction OnEndWave;

    [Header("Current Wave")]
    public int currentWave;

    [Header("Wave Settings")]
    public float maxWaitTime;
    public float initialWaitTime;
    public float textScreenTime;
    public int enemiesAdd;

    [Header("Checking Variables")]
    public List<EnemySpawner> spawners;
    public List<int> maxEnemies;
    public bool timerHasStarted;
    public bool screenTimerStarted;
    public float waitingTimer;
    public float textTimer;
    public float originalWaitTime;

    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Get().waves = this;
        maxEnemies.Add(enemiesAdd);
        GameManager.Get().maxEnemies = enemiesAdd;
        currentWave = maxEnemies.Count;
        StopWave();
        timerHasStarted = true;
        originalWaitTime = maxWaitTime;
        maxWaitTime = initialWaitTime;

        foreach (EnemySpawner spawner in spawners)
        {
            spawner.automaticSpawn = false;
        }
    }

    private void Update()
    {
        if(timerHasStarted)
        {
            waitingTimer += Time.deltaTime;

            if(waitingTimer >= maxWaitTime)
            {
                if(currentWave == 1)
                {
                    maxWaitTime = originalWaitTime;
                }
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
                if(OnEndUI != null)
                {
                    OnEndUI();
                }

                screenTimerStarted = false;
                textTimer = 0;
            }
        }
    }

    public void StartWave()
    {

        if(OnStartWave != null)
        {
            OnStartWave();
        }

        if(currentWave == 1)
        {

            if (OnStartWaveFirstTime != null)
            {
                OnStartWaveFirstTime();
            }
        }
        

        foreach (EnemySpawner spawner in spawners)
        {
            spawner.automaticSpawn = true;
        }

        if(OnStartUI != null)
        {
            OnStartUI(0);
        }


        screenTimerStarted = true;
        textTimer = 0;
    }

    public void StopWave()
    {
        foreach (EnemySpawner spawner in spawners)
        {
            spawner.automaticSpawn = false;
        }

        timerHasStarted = true;
        screenTimerStarted = true;
        textTimer = 0;

        if(OnEndWave != null)
        {
            OnEndWave(1);
        }
    }

    public void SetNextWave()
    {
        maxEnemies.Add(maxEnemies[maxEnemies.Count - 1] + enemiesAdd);
        currentWave = maxEnemies.Count;
    }
}
