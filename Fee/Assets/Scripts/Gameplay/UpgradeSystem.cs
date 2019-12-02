using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviourSingleton<UpgradeSystem>
{
    [Header("General Settings")]
    public UpgradeSO[] upgradesTemplates;
    public GameObject template;
    public GameObject parent;
    public int upgradePoints;
    public int initialPoints;
    public bool resetToInitialPoints;

    [Header("Sound Settings")]
    public GameObject fairyMaxSound;
    public GameObject fairySpeedSound;
    public GameObject meteorCooldownSound;
    public GameObject towerSound;
    public GameObject errorSound;

    [Header("Upgrades")]
    public List<Upgrade> allUpgrades;
    public int[] allUpgradesCurrentLevel;

    // Start is called before the first frame update
    void Start()
    {
        allUpgradesCurrentLevel = new int[upgradesTemplates.Length];

#if UNITY_EDITOR
        if (resetToInitialPoints)
        {
            PlayerPrefs.SetInt("UpgradePoints", initialPoints);
        }
#endif

        upgradePoints = PlayerPrefs.GetInt("UpgradePoints", 0);

        for (int i = 0; i < upgradesTemplates.Length; i++)
        {
            allUpgradesCurrentLevel[upgradesTemplates[i].id] = upgradesTemplates[i].currentLevel;
        }
    }

    public void AssignUpgrades()
    {
        for (int i = 0; i < upgradesTemplates.Length; i++)
        {
            GameObject newUpgrade = Instantiate(template);
            Upgrade currentUpgrade = newUpgrade.GetComponent<Upgrade>();

            currentUpgrade.data = upgradesTemplates[i];
            currentUpgrade.AssignData();
            currentUpgrade.currentLevel = allUpgradesCurrentLevel[currentUpgrade.id];

            if (parent)
            {
                newUpgrade.transform.SetParent(parent.transform, false);
                CheckButtonActivation(currentUpgrade);
            }
        }

        /*AkSoundEngine.PostEvent("upgrade_max", fairyMaxSound);
        AkSoundEngine.PostEvent("upgrade_speed", fairySpeedSound);
        AkSoundEngine.PostEvent("upgrade_cooldown", meteorCooldownSound);
        AkSoundEngine.PostEvent("upgrade_tower", towerSound);
        AkSoundEngine.PostEvent("upgrade_error", errorSound);*/
        CheckAllUpgrades();
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

                allUpgradesCurrentLevel[id] = upgrade.currentLevel;

                CheckButtonActivation(upgrade);

                CheckAllUpgrades();

                return true;
            }
            else
            {
                upgrade.currentColor = upgrade.unavailableColor;
                upgrade.button.image.sprite = upgrade.data.buttonDisabled;

                SpriteState state;
                state.pressedSprite = upgrade.data.buttonDisabled;
                upgrade.button.spriteState = state;
                Debug.Log("Cant Buy!");
            }
        }
        else
        {
            upgrade.currentColor = upgrade.unavailableColor;
            upgrade.button.image.sprite = upgrade.data.buttonDisabled;

            SpriteState state;
            state.pressedSprite = upgrade.data.buttonDisabled;
            upgrade.button.spriteState = state;
            Debug.Log("Cant Buy!");
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

    public void CheckAllUpgrades()
    {
        foreach (Upgrade selectedUpgrade in allUpgrades)
        {
            if (selectedUpgrade.data.amountPerLevel.Length > selectedUpgrade.currentLevel + 1)
            {
                if (!(upgradePoints >= selectedUpgrade.data.costPerLevel[selectedUpgrade.currentLevel + 1]))
                {
                    selectedUpgrade.currentColor = selectedUpgrade.unavailableColor;
                    selectedUpgrade.button.image.sprite = selectedUpgrade.data.buttonDisabled;

                    SpriteState state;
                    state.pressedSprite = selectedUpgrade.data.buttonDisabled;
                    selectedUpgrade.button.spriteState = state;
                }
            }
        }
    }

    public void CheckButtonActivation(Upgrade upgrade)
    {
        if (upgrade.currentLevel + 1 >= upgrade.data.amountPerLevel.Length)
        {
            upgrade.currentColor = upgrade.unavailableColor;
            upgrade.button.image.sprite = upgrade.data.buttonDisabled;

            SpriteState state;
            state.pressedSprite = upgrade.data.buttonDisabled;
            upgrade.button.spriteState = state;
        }
        else
        {
            upgrade.currentColor = Color.white;
            upgrade.button.image.sprite = upgrade.data.buttonIdle;
            SpriteState state;
            state.pressedSprite = upgrade.data.buttonPressed;
            upgrade.button.spriteState = state;
        }
    }

    public void CleanList()
    {
        allUpgrades.Clear();
    }
}
