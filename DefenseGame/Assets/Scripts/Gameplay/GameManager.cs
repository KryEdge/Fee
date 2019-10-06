using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [Header("Level Settings")]
    public int maxEnemies;
    public int maxFairies;

    [Header("Score Settings")]
    public int increaseMilestoneScoreAmount;
    public int scoreAmount;
    public float givePointsTime;

    [Header("Assign Components/GameObjects")]
    public UIFairies fairies;
    public UITowers towers;
    public UIScore scoreUI;

    [Header("Check Variables")]
    public int score;
    public int currentFairies;
    public int givePointsMultiplier;
    public List<GameObject> enemies;

    private int increaseScoreMilestone;
    private float pointsTimer;

    private void Start()
    {
        increaseScoreMilestone += increaseMilestoneScoreAmount;
        UpdateUI();
    }

    // Update is called once per frame
    private void Update()
    {
        pointsTimer += Time.deltaTime;

        if(pointsTimer >= givePointsTime)
        {
            pointsTimer = 0;
            score += scoreAmount * givePointsMultiplier;
            scoreUI.UpdateText();
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
}
