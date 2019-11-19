using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "MilestoneScriptableObject", menuName = "Frank/Utils/MilestoneScriptableObject" + "", order = 1)]
public class MilestoneScriptableObject : ScriptableObject
{
    //https://docs.unity3d.com/Manual/class-ScriptableObject.html

    public enum MilestoneType
    {
        Score,
        EnemiesKilled,
        TowersPlaced,
        TimesEscaped,
        GemsCollected,
        allTypes
    }

    [Header("General Settings")]
    public MilestoneType type;
    public string objectiveText;
    public int rewardPoints;
    public bool isMultipleSteps;

    [Header("Mutliple Steps Objective")]
    public int amountOfSteps;

    [Header("Checking Variables")]
    public bool isActive;
    public bool isDone;
    public bool hasGivenReward;
    public int currentStep;
}
