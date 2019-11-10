using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public delegate void OnLevelAction();
    public static OnLevelAction OnLevelEndWave;
    public OnLevelAction OnLevelGameOver;
    public OnLevelAction OnGameGivePoints;
    public OnLevelAction OnLastFairyAlive;

    [Header("Cheat Settings")]
    public bool areCheatsOn;

    [Header("Level Settings")]
    public int maxEnemies;
    public int maxFairies;
    public int maxTurrets;
    public float towerFireRate;
    public float fairySpeed;

    [Header("Score Settings")]
    public int upgradePointsCurrentMatch;
    public int upgradePointsToGive;
    public int upgradePointsGiveMilestonesOriginal;
    public int initialMilestone;
    public int scoreAmount;
    public float givePointsTime;
    public int milestoneMultiplier;
    public float confettiTime;
    public bool isConfettiOn;

    [Header("Assign Components/GameObjects")]
    public ParticleSystem[] confetti;
    public GameObject GameOverPanel;
    public CameraMovement movement;
    public Shoot shoot;
    public TurretSpawner turretSpawner;
    public WaveSystem waves;

    [Header("Check Variables")]
    public int score;
    public int enemiesKilled;
    public int currentFairies;
    public List<GameObject> enemies;
    public List<UITowersState> towersUI;

    
    private GameObject[] enemiesToDelete;
    private bool gameOver;
    private bool canGivePoints;
    private float confettiTimer;
    private float pointsTimer;
    private int increaseScoreMilestone;
    private int givePointsMultiplier;
    private int upgradePointsGiveMilestone;
    private UpgradeSystem upgrades;

    private void Start()
    {
        Time.timeScale = 1;
        upgrades = UpgradeSystem.Get();
        increaseScoreMilestone = initialMilestone;
        WaveSystem.OnStartWaveFirstTime += StartGivingPoints;
        Fairy.OnFairyDeath += CheckFairiesCount;
        Enemy.OnDeath += AddPoints;

        maxFairies = (int)upgrades.GetUpgrade(0).GetCurrentAmount();
        shoot.rechargeTime = upgrades.GetUpgrade(1).GetCurrentAmount();
        towerFireRate = upgrades.GetUpgrade(2).GetCurrentAmount();
        fairySpeed = (int)upgrades.GetUpgrade(3).GetCurrentAmount();

        turretSpawner.fireRate = towerFireRate;
        upgradePointsGiveMilestone = upgradePointsGiveMilestonesOriginal;

        for (int i = 0; i < confetti.Length; i++)
        {
            confetti[i].Stop();
        }
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentFairies <= 0)
        {
            if (!gameOver)
            {
                GameOver();
                gameOver = true;
            }
        }
        else
        {
            pointsTimer += Time.deltaTime;
        }

        if (pointsTimer >= givePointsTime)
        {
            if (!gameOver && canGivePoints)
            {
                pointsTimer = 0;
                AddPoints(null, scoreAmount * givePointsMultiplier);

                if (OnGameGivePoints != null)
                {
                    OnGameGivePoints();
                }
            } 
        }

        if(score >= increaseScoreMilestone)
        {
            givePointsMultiplier++;
            increaseScoreMilestone = increaseScoreMilestone * milestoneMultiplier;
            StopWave();
        }

        if(isConfettiOn)
        {

            for (int i = 0; i < confetti.Length; i++)
            {
                if (!confetti[i].isPlaying)
                {
                    confetti[i].Play();
                }
            }
            
            confettiTimer += Time.deltaTime;

            if(confettiTimer >= confettiTime)
            {
                isConfettiOn = false;
                confettiTimer = 0;

                for (int i = 0; i < confetti.Length; i++)
                {
                    if (!confetti[i].isStopped)
                    {
                        confetti[i].Stop();
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < confetti.Length; i++)
            {
                if (!confetti[i].isStopped)
                {
                    confetti[i].Stop();
                }
            }
        }
    }

    public void SwitchMeteorActivation()
    {
        shoot.SwitchActivationForced();
    }

    public void SwitchTurretActivation()
    {
        turretSpawner.SwitchPreviewForced();
    }

    private void GameOver()
    {
        int originalPoints = PlayerPrefs.GetInt("UpgradePoints", 0);
        PlayerPrefs.SetInt("UpgradePoints", originalPoints + upgradePointsCurrentMatch);
        UpgradeSystem.Get().UpdatePoints();
        movement.enabled = false;
        shoot.enabled = false;
        GameOverPanel.SetActive(true);
        TurretSpawner.Get().preview = false;
        TurretSpawner.Get().StopAllOutlines();

        if(OnLevelGameOver != null)
        {
            OnLevelGameOver();
        }
    }

    private void CheckFairiesCount()
    {
        if(currentFairies == 1)
        {
            if(OnLastFairyAlive != null)
            {
                OnLastFairyAlive();
            }
            
        }
    }

    private void OnDestroy()
    {
        Fairy.OnFairyDeath -= CheckFairiesCount;
        Enemy.OnDeath -= AddPoints;
        WaveSystem.OnStartWaveFirstTime -= StartGivingPoints;
        upgradePointsCurrentMatch = 0;
    }

    public void StopWave()
    {

        waves.StopWave();
        waves.SetNextWave();
        maxEnemies = waves.maxEnemies[waves.maxEnemies.Count - 1];

        if(OnLevelEndWave != null)
        {
            OnLevelEndWave();
        }
    }

    private void AddPoints(GameObject enemy, int pointsToGive)
    {
        if(!gameOver)
        {
            score += pointsToGive;

            if (OnGameGivePoints != null)
            {
                OnGameGivePoints();
            }

            if(enemy)
            {
                enemiesKilled++;
            }

            if(score >= upgradePointsGiveMilestone)
            {
                upgradePointsCurrentMatch += upgradePointsToGive;
                upgradePointsGiveMilestone += upgradePointsGiveMilestonesOriginal;
            }
        }
    }

    public void KillAllEnemies()
    {
        enemiesToDelete = new GameObject[enemies.Count];

        for (int i = 0; i < enemiesToDelete.Length; i++)
        {
            enemiesToDelete[i] = enemies[i];
            Destroy(enemiesToDelete[i]);
        }

        enemies.Clear();
    }

    private void StartGivingPoints()
    {
        canGivePoints = true;
    }
}
