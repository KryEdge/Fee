using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [Header("Level Settings")]
    public int maxEnemies;
    public int maxFairies;
    public int maxTurrets;

    [Header("Score Settings")]
    public int increaseMilestoneScoreAmount;
    public int scoreAmount;
    public float givePointsTime;

    [Header("Assign Components/GameObjects")]
    public UIFairies fairies;
    public UITowers towers;
    public UIScore scoreUI;
    public UIVignette vignette;
    public GameObject GameOverPanel;
    public CameraMovement movement;
    public Shoot shoot;

    [Header("Check Variables")]
    public int score;
    public int currentFairies;
    public int givePointsMultiplier;
    public List<GameObject> enemies;
    public bool gameOver;

    private int increaseScoreMilestone;
    private float pointsTimer;

    private void Start()
    {
        increaseScoreMilestone += increaseMilestoneScoreAmount;
        Fairy.OnFairyDeath += CheckFairiesCount;
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
            increaseScoreMilestone += increaseMilestoneScoreAmount;
        }
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
    }
}
