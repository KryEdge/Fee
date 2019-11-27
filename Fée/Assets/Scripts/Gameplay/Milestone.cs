using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class Milestone : MonoBehaviour
{
    public delegate void OnMilestoneAction();
    public static OnMilestoneAction OnMilestoneDone;

    [Header("General Settings")]
    public MilestoneScriptableObject milestone;

    [Header("Assign Components")]
    public Text titleUI;
    public Text progressTextUI;
    public Slider progressBar;

    private bool doOnce;

    // Update is called once per frame
    private void Update()
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
                        CheckTowersPlacedType();
                        break;
                    case MilestoneScriptableObject.MilestoneType.TimesEscaped:
                        CheckTimesEscapedType();
                        break;
                    case MilestoneScriptableObject.MilestoneType.GemsCollected:
                        CheckGemsCollectedType();
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
                        if(OnMilestoneDone != null)
                        {
                            OnMilestoneDone();
                        }
                    }
                    
                    progressTextUI.text = "Done!";
                    progressBar.value = 1;
                    doOnce = true;
                }
                
            }
        }
    }

    private void CheckProgress(int type)
    {
        milestone.currentStep = type;
        progressTextUI.text = milestone.currentStep + "/" + milestone.amountOfSteps;

        float progressBarValue = ((milestone.currentStep * 100) / milestone.amountOfSteps) * 0.01f;
        progressBar.value = progressBarValue;

        if (progressBar.value >= 1)
        {
            milestone.isDone = true;
        }
    }

    private void CheckScoreType()
    {
        CheckProgress(GameManager.Get().score);
    }

    private void CheckEnemiesKilledType()
    {
        CheckProgress(GameManager.Get().enemiesKilled);
    }

    private void CheckTowersPlacedType()
    {
        CheckProgress(GameManager.Get().towersPlaced);
    }

    private void CheckTimesEscapedType()
    {
        CheckProgress(GameManager.Get().timesEscaped);
    }

    private void CheckGemsCollectedType()
    {
        CheckProgress(GameManager.Get().gemsCollected);
    }

    public void AssignData()
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
                CheckTowersPlacedType();
                break;
            case MilestoneScriptableObject.MilestoneType.TimesEscaped:
                CheckTimesEscapedType();
                break;
            case MilestoneScriptableObject.MilestoneType.GemsCollected:
                CheckGemsCollectedType();
                break;
            default:
                break;
        }

        MilestoneManager.Get().allMilestones.Add(this);
        MilestoneManager.Get().currentMilestonesTotal++;
    }

    private void OnDestroy()
    {
        milestone.hasGivenReward = false;
        milestone.isDone = false;
        milestone.isActive = false;
        milestone.currentStep = 0;
    }
}
