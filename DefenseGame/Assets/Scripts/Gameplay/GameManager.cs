using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public delegate void OnLevelAction();
    public static OnLevelAction OnLevelEndWave;

    [Header("Cheat Settings")]
    public bool areCheatsOn;

    [Header("Level Settings")]
    public int maxEnemies;
    public int maxFairies;
    public int maxTurrets;
    public float towerFireRate;

    [Header("Score Settings")]
    public int upgradePointsCurrentMatch;
    public int initialMilestone;
    public int scoreAmount;
    public float givePointsTime;
    public int milestoneMultiplier;

    [Header("Assign Components/GameObjects")]
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

    public int increaseScoreMilestone;
    private float pointsTimer;
    private UpgradeSystem upgrades;

    private void Start()
    {
        Time.timeScale = 1;
        upgrades = UpgradeSystem.Get();
        increaseScoreMilestone = initialMilestone;
        Fairy.OnFairyDeath += CheckFairiesCount;
        Enemy.OnDeath += AddPoints;
        UpdateUI();

        maxFairies = (int)upgrades.GetUpgradeAmount(upgrades.fairiesUpgrade);
        maxTurrets = (int)upgrades.GetUpgradeAmount(upgrades.towersUpgrade);
        towerFireRate = upgrades.GetUpgradeAmount(upgrades.towersFireRateUpgrade);
        turretSpawner.fireRate = towerFireRate;
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
            if (!gameOver)
            {
                pointsTimer = 0;
                score += scoreAmount * givePointsMultiplier;
                scoreUI.UpdateText();
            } 
        }

        if(score >= increaseScoreMilestone)
        {
            givePointsMultiplier++;
            increaseScoreMilestone = increaseScoreMilestone * milestoneMultiplier;
            StopWave();
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
        upgradePointsCurrentMatch = 0;
        movement.enabled = false;
        shoot.enabled = false;
        GameOverPanel.SetActive(true);
        TurretSpawner.Get().preview = false;
        TurretSpawner.Get().StopAllOutlines();
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
        Debug.Log("Gave " + pointsToGive + "Points to the player.");
        score += pointsToGive;
        scoreUI.UpdateText();
        enemiesKilled++;
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
}
