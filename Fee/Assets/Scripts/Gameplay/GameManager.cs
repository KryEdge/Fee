using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public delegate void OnLevelAction();
    public static OnLevelAction OnLevelEndWave;
    public OnLevelAction OnLevelGameOver;

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
    public UIPauseButton pause;
    public UIFairies fairies;
    public UITowers towers;
    public UIScore scoreUI;
    public UIVignette vignette;
    public GameObject GameOverPanel;
    public CameraMovement movement;
    public Shoot shoot;
    public TurretSpawner turretSpawner;
    public WaveSystem waves;

    [Header("Check Variables")]
    public int score;
    public int enemiesKilled;
    public int currentFairies;
    public int givePointsMultiplier;
    public List<GameObject> enemies;
    public List<UITowersState> towersUI;
    public GameObject[] enemiesToDelete;
    public bool gameOver;
    public bool canGivePoints;
    public float confettiTimer;

    public int increaseScoreMilestone;
    private float pointsTimer;
    private int upgradePointsGiveMilestone;
    private UpgradeSystem upgrades;

    private void Start()
    {
        Time.timeScale = 1;
        upgrades = UpgradeSystem.Get();
        increaseScoreMilestone = initialMilestone;
        WaveSystem.OnStartWaveFirstTime = StartGivingPoints;
        Fairy.OnFairyDeath += CheckFairiesCount;
        Enemy.OnDeath += AddPoints;
        UpdateUI();

        maxFairies = (int)upgrades.GetUpgradeAmount(upgrades.fairiesUpgrade);
        shoot.rechargeTime = upgrades.GetUpgradeAmount(upgrades.meteorCooldownUpgrade);
        towerFireRate = upgrades.GetUpgradeAmount(upgrades.towersFireRateUpgrade);
        fairySpeed = (int)upgrades.GetUpgradeAmount(upgrades.fairySpeedUpgrade);

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
                scoreUI.UpdateText();
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
            Debug.Log("Confetti is on");

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

    public void UpdateUI()
    {
        if(fairies)
        {
            fairies.UpdateText();
        }

        if(towers)
        {
            towers.UpdateText();
        }
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
        pause.pauseMenu.SetActive(false);
        pause.enabled = false;

        if(OnLevelGameOver != null)
        {
            OnLevelGameOver();
        }
    }

    private void CheckFairiesCount()
    {
        if(currentFairies == 1)
        {
            vignette.SetLowHealthColor();
            vignette.SwitchMask();
        }
        else if(currentFairies == 0)
        {
            vignette.SwitchMask();
        }
    }

    private void OnDestroy()
    {
        Fairy.OnFairyDeath -= CheckFairiesCount;
        Enemy.OnDeath -= AddPoints;
        upgradePointsCurrentMatch = 0;
    }

    public void StopWave()
    {
        //enemiesToDelete = new GameObject[enemies.Count];

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
            Debug.Log("Gave " + pointsToGive + "Points to the player.");
            score += pointsToGive;
            scoreUI.UpdateText();

            if(enemy)
            {
                enemiesKilled++;
            }

            if(score >= upgradePointsGiveMilestone)
            {
                Debug.Log("Added upgrade points!");
                upgradePointsCurrentMatch += upgradePointsToGive;
                upgradePointsGiveMilestone += upgradePointsGiveMilestonesOriginal;
            }
        }
    }

    public void KillAllEnemies()
    {
        //Debug.Log("Killed all enemies");
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
