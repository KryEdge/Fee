using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilestoneManager : MonoBehaviourSingleton<MilestoneManager>
{
    public List<Milestone> allMilestones;
    public List<Milestone> milestonesToMove;
    public GameObject milestonePanel;
    public int maxMilestones;
    public int currentMilestones;

    public int maxMilestonesTotal;
    public int currentMilestonesTotal;
    public float maxSwitchMilestoneTime;

    public float switchMilestoneTimer;

    private bool doOnce;
    public bool canSwitch;
    // Update is called once per frame
    void Update()
    {
        if(!doOnce)
        {
            if(currentMilestonesTotal >= maxMilestonesTotal)
            {
                SetNewMilestoneUpdate();
                doOnce = true;
            }
        }

        if(canSwitch)
        {
            switchMilestoneTimer += Time.deltaTime;

            if(switchMilestoneTimer >= maxSwitchMilestoneTime)
            {
                RemoveOldMilestone();
                SetNewMilestoneUpdate();
                switchMilestoneTimer = 0;
                canSwitch = false;
            }
        }
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
    }

    public void SetNewMilestoneUpdate()
    {
        Debug.Log("Setting new Milestones 2");

        for (int i = 0; i < allMilestones.Count; i++)
        {
            if(currentMilestones < maxMilestones)
            {
                if (!allMilestones[i].milestone.isDone && !allMilestones[i].milestone.isActive && currentMilestones < maxMilestones)
                {
                    //Debug.Log("Choosen " + allMilestones[i].gameObject.name);
                    allMilestones[i].milestone.isActive = true;
                    allMilestones[i].transform.SetParent(milestonePanel.transform);
                    currentMilestones++;
                }
            }
            else
            {
                i = allMilestones.Count;
            }
        }
    }

    public void RemoveOldMilestone()
    {
        if(allMilestones.Count > maxMilestones)
        {
            foreach (Milestone item in milestonesToMove)
            {
                if (item.milestone.isDone)
                {
                   // Debug.Log("rip");
                    currentMilestones--;
                    //Debug.Log("rip " + currentMilestones);
                    item.milestone.isActive = false;
                    item.transform.SetParent(transform);
                }
            }

            milestonesToMove.Clear();
        }
    }
}
