using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerUpgrades : MonoBehaviour
{
    public Text gemsText;
    private UpgradeSystem upgradeSystem;

    // Start is called before the first frame update
    void Start()
    {
        upgradeSystem = UpgradeSystem.Get();
        Upgrade.OnUpgradePurcharsed = UpdateText;
        upgradeSystem.AssignUpgrades();
    }

    private void UpdateText(int id)
    {
        Debug.Log("id == " + id);
        gemsText.text = "Gems: " + upgradeSystem.upgradePoints;

        foreach (Upgrade upgrade in upgradeSystem.allUpgrades)
        {
            if(upgrade.id == id)
            {
                upgrade.nameText.text = upgrade.data.upgradeName;
                upgrade.levelText.text = "Current Level: " + (upgrade.currentLevel + 1) + " / " + upgrade.maxLevel;

                if (upgrade.CheckNextLevel())
                {
                    upgrade.costText.text = "Cost: " + upgrade.data.costPerLevel[upgrade.currentLevel + 1];
                    upgrade.differenceText.text = "Next Level: " + upgrade.data.amountPerLevel[upgrade.currentLevel] + " -> " + upgrade.data.amountPerLevel[upgrade.currentLevel + 1];
                }
                else
                {
                    upgrade.costText.text = "REACHED MAX LEVEL !";
                    upgrade.differenceText.text = "Current: " + upgrade.data.amountPerLevel[upgrade.currentLevel];
                }
            }
        }
    }
}
