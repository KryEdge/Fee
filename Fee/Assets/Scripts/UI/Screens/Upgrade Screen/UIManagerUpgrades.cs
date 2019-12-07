using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerUpgrades : MonoBehaviour
{
    [Header("General Settings")]
    public GameObject mainCanvas;
    public GameObject backgroundCanvas;
    public GameObject parentForUpgrades;
    public Text gemsText;
    public UpgradeSystem upgradeSystem;
    //

    // Start is called before the first frame update
    void Start()
    {
        upgradeSystem = UpgradeSystem.Get();
        Upgrade.OnUpgradePurcharsed = UpdateText;
        upgradeSystem.parent = parentForUpgrades;
        upgradeSystem.AssignUpgrades();

        for (int i = 0; i < upgradeSystem.allUpgrades.Count; i++)
        {
            UpdateText(upgradeSystem.allUpgrades[i].id);
        }

        mainCanvas.SetActive(true);
        backgroundCanvas.SetActive(false);
    }

    private void UpdateText(int id)
    {
        if(gemsText)
        {
            gemsText.text = "Gems: " + upgradeSystem.upgradePoints;
        }

        foreach (Upgrade upgrade in upgradeSystem.allUpgrades)
        {
            upgrade.nameText.color = upgrade.currentColor;
            upgrade.costText.color = upgrade.currentColor;
            upgrade.levelText.color = upgrade.currentColor;
            upgrade.differenceText.color = upgrade.currentColor;

            if (upgrade.id == id)
            {
                if(upgrade)
                {
                    upgrade.nameText.text = upgrade.data.upgradeName;
                    upgrade.levelText.text = "Current Level: " + (upgrade.currentLevel + 1) + " / " + upgrade.maxLevel;

                    if (upgrade.CheckNextLevel())
                    {
                        upgrade.costText.text = "Cost: " + upgrade.data.costPerLevel[upgrade.currentLevel + 1];
                        upgrade.differenceText.text = "Next Level: " + upgrade.data.amountPerLevel[upgrade.currentLevel] + " -> " + upgrade.data.amountPerLevel[upgrade.currentLevel + 1] + " " + upgrade.data.unitType; ;
                    }
                    else
                    {
                        upgrade.costText.text = "REACHED MAX LEVEL !";
                        upgrade.differenceText.text = "Current: " + upgrade.data.amountPerLevel[upgrade.currentLevel] + " " + upgrade.data.unitType;
                    }
                }
            }
        }
    }

    public void SwitchCanvas()
    {
        mainCanvas.SetActive(false);
        backgroundCanvas.SetActive(true);
    }
}
