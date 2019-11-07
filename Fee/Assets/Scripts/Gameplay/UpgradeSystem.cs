using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviourSingleton<UpgradeSystem>
{
    [Header("General Settings")]
    public UpgradeSO[] upgradesTemplates;
    public GameObject template;
    public GameObject parent;
    public int upgradePoints;
    public int initialPoints;
    public bool resetToInitialPoints;

    [Header("Upgrades")]
    public List<Upgrade> allUpgrades;

    // Start is called before the first frame update
    void Start()
    {
        if (resetToInitialPoints)
        {
            PlayerPrefs.SetInt("UpgradePoints", initialPoints);
        }

        upgradePoints = PlayerPrefs.GetInt("UpgradePoints", 0);

        for (int i = 0; i < upgradesTemplates.Length; i++)
        {
            GameObject newUpgrade = Instantiate(template);

            newUpgrade.GetComponent<Upgrade>().data = upgradesTemplates[i];
            newUpgrade.GetComponent<Upgrade>().AssignData();
            newUpgrade.transform.SetParent(parent.transform, false);
        }
    }

    public void UpdatePoints()
    {
        upgradePoints = PlayerPrefs.GetInt("UpgradePoints", 0);
    }

    public void DiscountPoints(int amount)
    {
        upgradePoints = PlayerPrefs.GetInt("UpgradePoints", 0);
        upgradePoints = upgradePoints - amount;

        if (upgradePoints < 0)
        {
            upgradePoints = 0;
        }

        PlayerPrefs.SetInt("UpgradePoints", upgradePoints);
    }

    public bool BuyUpgrade(int id)
    {
        Upgrade upgrade = null;

        foreach (Upgrade selectedUpgrade in allUpgrades)
        {
            if (id == selectedUpgrade.id)
            {
                upgrade = selectedUpgrade;
            }
        }

        upgradePoints = PlayerPrefs.GetInt("UpgradePoints", 0);

        if (upgrade.data.amountPerLevel.Length > upgrade.currentLevel + 1)
        {
            if (upgradePoints >= upgrade.data.costPerLevel[upgrade.currentLevel + 1])
            {
                DiscountPoints(upgrade.data.costPerLevel[upgrade.currentLevel + 1]);
                upgrade.currentLevel++;

                //PlayerPrefs.SetInt(upgrade.upgradeName, upgrade.currentLevel);
                return true;
            }
        }

        return false;
    }

    public void ResetUpgrades()
    {
        foreach (Upgrade upgrade in allUpgrades)
        {
            upgrade.ResetData();
        }

        upgradePoints = 0;
        PlayerPrefs.SetInt("UpgradePoints", upgradePoints);
    }

    public void AddUpgrade(Upgrade upgrade)
    {
        //upgrade.id = allUpgrades.Count;
        allUpgrades.Add(upgrade);
    }

    public Upgrade GetUpgrade(int id)
    {
        foreach (Upgrade selectedUpgrade in allUpgrades)
        {
            if (id == selectedUpgrade.id)
            {
                return selectedUpgrade;
            }
        }

        return null;
    }
}
