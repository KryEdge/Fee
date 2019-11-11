using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilestoneManager : MonoBehaviourSingleton<MilestoneManager>
{
    [Header("General Settings")]
    public MilestoneScriptableObject[] milestonesSO;
    public int maxMilestones; 
    public float maxSwitchMilestoneTime;

    [Header("Assign Components")]
    public GameObject milestonePrefab;
    public GameObject milestonePanel;

    [Header("Checking Variables")]
    public int currentMilestones;
    public int currentMilestonesTotal;
    public List<Milestone> allMilestones;
    public List<Milestone> milestonesToMove;
    public bool canSwitch;

    private float switchMilestoneTimer;

    private void Start()
    {
        for (int i = 0; i < milestonesSO.Length; i++)
        {
            GameObject newMilestoneObject = Instantiate(milestonePrefab);
            Milestone newMilestone = newMilestoneObject.GetComponent<Milestone>();

            newMilestone.milestone = milestonesSO[i];
            newMilestone.AssignData();
            newMilestone.transform.SetParent(transform, false);
        }

        SetNewMilestone();
    }


    // Update is called once per frame
    void Update()
    {
        if(canSwitch)
        {
            switchMilestoneTimer += Time.deltaTime;

            if(switchMilestoneTimer >= maxSwitchMilestoneTime)
            {
                RemoveOldMilestone();
                SetNewMilestone();
                switchMilestoneTimer = 0;
                canSwitch = false;
            }
        }
    }

    public void SetNewMilestone()
    {

        for (int i = 0; i < allMilestones.Count; i++)
        {
            if(currentMilestones < maxMilestones)
            {
                if (!allMilestones[i].milestone.isDone && !allMilestones[i].milestone.isActive && currentMilestones < maxMilestones)
                {
                    allMilestones[i].milestone.isActive = true;
                    allMilestones[i].transform.SetParent(milestonePanel.transform, false);
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
                    currentMilestones--;
                    item.milestone.isActive = false;
                    item.transform.SetParent(transform, false);
                }
            }

            milestonesToMove.Clear();
        }
    }
}
