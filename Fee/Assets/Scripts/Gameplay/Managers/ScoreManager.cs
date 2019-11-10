using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
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

    [Header("Check Variables")]
    public int score;
    public float confettiTimer;
    public int givePointsMultiplier;
    public int increaseScoreMilestone;
    private int upgradePointsGiveMilestone;
    private float pointsTimer;


    // Start is called before the first frame update
    void Start()
    {
        increaseScoreMilestone = initialMilestone;
        upgradePointsGiveMilestone = upgradePointsGiveMilestonesOriginal;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PointsUpdate()
    {
        pointsTimer += Time.deltaTime;


    }
}
