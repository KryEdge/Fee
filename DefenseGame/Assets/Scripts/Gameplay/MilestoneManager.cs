using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilestoneManager : MonoBehaviourSingleton<MilestoneManager>
{
    public List<Milestone> allMilestones;
    public GameObject milestonePanel;
    public int maxMilestones;
    public int currentMilestones;

    public int maxMilestonesTotal;
    public int currentMilestonesTotal;

    private bool doOnce;
    // Update is called once per frame
    void Update()
    {
        if(!doOnce)
        {
            if(currentMilestonesTotal >= maxMilestonesTotal)
            {
                SetNewMilestone();
                doOnce = true;
            }
        }

        /*
        if(currentMilestones < maxMilestones)
        {
            SetNewMilestone();
        }*/
    }

    public void SetNewMilestone()
    {
        Debug.Log("Setting new Milestones");

        int random = Random.Range(0, allMilestones.Count);

        while(currentMilestones < maxMilestones)
        {
            if (!allMilestones[random].milestone.isDone && !allMilestones[random].milestone.isActive && currentMilestones < maxMilestones)
            {
                Debug.Log("Choosen " + allMilestones[random].gameObject.name);
                allMilestones[random].milestone.isActive = true;
                allMilestones[random].transform.SetParent(milestonePanel.transform);
                currentMilestones++;
            }
            random = Random.Range(0, allMilestones.Count);
        }

        /*foreach (Milestone item in allMilestones)
        {
            if(!item.milestone.isDone && !item.milestone.isActive && currentMilestones < maxMilestones)
            {
                Debug.Log("Choosen " + item.gameObject.name);
                item.milestone.isActive = true;
                item.transform.SetParent(milestonePanel.transform);
                currentMilestones++;
            }
        }*/
        
    }

    public void RemoveOldMilestone(Milestone oldMilestone)
    {
        if(allMilestones.Count > maxMilestones)
        {
            if (oldMilestone.milestone.isDone)
            {
                Debug.Log("rip");
                currentMilestones--;
                Debug.Log("rip " + currentMilestones);
                oldMilestone.milestone.isActive = false;
                oldMilestone.transform.SetParent(transform);
            }
        }
    }
}
