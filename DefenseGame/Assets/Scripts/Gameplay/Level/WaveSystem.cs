using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSystem : MonoBehaviourSingleton<WaveSystem>
{
    public delegate void OnWaveAction();
    public static OnWaveAction OnStartWave;

    [Header("Current Wave")]
    public int currentWave;

    [Header("Wave Settings")]
    public float maxWaitTime;
    public float initialWaitTime;
    public float textScreenTime;
    public int enemiesAdd;

    [Header("UI")]
    public Text waveText;

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
        waveText.text = "Game is about to Start!";
        timerHasStarted = true;
        originalWaitTime = maxWaitTime;
        maxWaitTime = initialWaitTime;
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
                waveText.enabled = false;
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
        waveText.text = "Wave Ended... Destroy the remaining enemies!";
        screenTimerStarted = true;
        textTimer = 0;
    }

    public void SetNextWave()
    {
        maxEnemies.Add(maxEnemies[maxEnemies.Count - 1] + enemiesAdd);
        currentWave = maxEnemies.Count;
    }
}
