using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviourSingleton<UpgradeSystem>
{
    [System.Serializable]
    public class Upgrade
    {
        public string upgradeName;
        public int currentLevel;
        public float[] amountPerLevel;
        public int[] costPerLevel;
    }

    [Header("General Settings")]
    public int upgradePoints;
    public int initialPoints;
    public bool resetToInitialPoints;

    [Header("Upgrades")]
    public Upgrade fairiesUpgrade;
    public Upgrade towersFireRateUpgrade;
    public Upgrade meteorCooldownUpgrade;

    // Start is called before the first frame update
    void Start()
    {
        if(resetToInitialPoints)
        {
            PlayerPrefs.SetInt("UpgradePoints", initialPoints);
        }

        upgradePoints = PlayerPrefs.GetInt("UpgradePoints", 0);
    }

    public void UpdatePoints()
    {
        upgradePoints = PlayerPrefs.GetInt("UpgradePoints", 0);
    }

    public void DiscountPoints(int amount)
    {
        upgradePoints = PlayerPrefs.GetInt("UpgradePoints", 0);
        upgradePoints = upgradePoints - amount;

        if(upgradePoints < 0)
        {
            upgradePoints = 0;
        }

        PlayerPrefs.SetInt("UpgradePoints", upgradePoints);
    }

    public bool BuyUpgrade(Upgrade upgrade)
    {
        upgradePoints = PlayerPrefs.GetInt("UpgradePoints", 0);

        if(upgrade.amountPerLevel.Length > upgrade.currentLevel + 1)
        {
            if (upgradePoints >= upgrade.costPerLevel[upgrade.currentLevel + 1])
            {
                DiscountPoints(upgrade.costPerLevel[upgrade.currentLevel + 1]);
                upgrade.currentLevel++;

                //PlayerPrefs.SetInt(upgrade.upgradeName, upgrade.currentLevel);
                return true;
            }
        }

        

        return false;
    }

    public float GetNextLevelUpgradeAmount(Upgrade upgrade)
    {
        if (upgrade.amountPerLevel.Length > upgrade.currentLevel + 1)
        {
            return upgrade.amountPerLevel[upgrade.currentLevel + 1];
        }
            
        return 0;
    }

    public int GetNextLevelUpgradeCost(Upgrade upgrade)
    {
        if(upgrade.costPerLevel.Length > upgrade.currentLevel + 1)
        {
            return upgrade.costPerLevel[upgrade.currentLevel + 1];
        }

        return 0;
    }

    public float GetUpgradeAmount(Upgrade upgrade)
    {
        return upgrade.amountPerLevel[upgrade.currentLevel];
    }

    public int GetUpgradeCost(Upgrade upgrade)
    {
        return upgrade.costPerLevel[upgrade.currentLevel];
    }

    public string GetUpgradeName(Upgrade upgrade)
    {
        return upgrade.upgradeName;
    }

    public int GetNextUpgradeLevel(Upgrade upgrade)
    {
        return upgrade.currentLevel + 1;
    }
}
