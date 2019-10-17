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

    [Header("Score Settings")]
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
    public int currentFairies;
    public int givePointsMultiplier;
    public List<GameObject> enemies;
    public GameObject[] enemiesToDelete;
    public bool gameOver;

    public int increaseScoreMilestone;
    private float pointsTimer;

    private void Start()
    {
        Time.timeScale = 1;
        increaseScoreMilestone = initialMilestone;
        Fairy.OnFairyDeath += CheckFairiesCount;
        Enemy.OnDeath += AddPoints;
        UpdateUI();
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

    private void StopWave()
    {
        enemiesToDelete = new GameObject[enemies.Count];

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
    }
}
