﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class Milestone : MonoBehaviour
{
    public MilestoneScriptableObject milestone;
    public Text titleUI;
    public Text progressTextUI;
    public Slider progressBar;

    private bool doOnce;
    // Start is called before the first frame update
    void Start()
    {
        titleUI.text = milestone.objectiveText;

        switch (milestone.type)
        {
            case MilestoneScriptableObject.MilestoneType.Score:
                CheckScoreType();
                break;
            case MilestoneScriptableObject.MilestoneType.EnemiesKilled:
                CheckEnemiesKilledType();
                break;
            case MilestoneScriptableObject.MilestoneType.TowersPlaced:
                break;
            default:
                break;
        }

        MilestoneManager.Get().allMilestones.Add(this);
        MilestoneManager.Get().currentMilestonesTotal++;
    }

    // Update is called once per frame
    void Update()
    {
        if(milestone.isActive)
        {
            if (!milestone.isDone)
            {
                switch (milestone.type)
                {
                    case MilestoneScriptableObject.MilestoneType.Score:
                        CheckScoreType();
                        break;
                    case MilestoneScriptableObject.MilestoneType.EnemiesKilled:
                        CheckEnemiesKilledType();
                        break;
                    case MilestoneScriptableObject.MilestoneType.TowersPlaced:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if(!doOnce)
                {
                    MilestoneManager.Get().milestonesToMove.Add(this);
                    MilestoneManager.Get().canSwitch = true;

                    if(!milestone.hasGivenReward)
                    {
                        GameManager.Get().upgradePointsCurrentMatch += milestone.rewardPoints;
                        milestone.hasGivenReward = true;
                    }
                    
                    progressTextUI.text = "Done!";
                    progressBar.value = 1;
                    doOnce = true;
                }
                
            }
        }
    }

    private void CheckScoreType()
    {
        milestone.currentStep = GameManager.Get().score;
        progressTextUI.text = milestone.currentStep + "/" + milestone.amountOfSteps;

        float progressBarValue = ((milestone.currentStep * 100) / milestone.amountOfSteps) * 0.01f;
        progressBar.value = progressBarValue;

        if (progressBar.value >= 1)
        {
            milestone.isDone = true;
        }
    }

    private void CheckEnemiesKilledType()
    {
        milestone.currentStep = GameManager.Get().enemiesKilled;
        progressTextUI.text = milestone.currentStep + "/" + milestone.amountOfSteps;

        float progressBarValue = ((milestone.currentStep * 100) / milestone.amountOfSteps) * 0.01f;
        progressBar.value = progressBarValue;

        if (progressBar.value >= 1)
        {
            milestone.isDone = true;
        }
    }

    private void OnDestroy()
    {
        milestone.hasGivenReward = false;
        milestone.isDone = false;
        milestone.isActive = false;
        milestone.currentStep = 0;
    }
}