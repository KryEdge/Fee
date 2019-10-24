using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("HighScore")]
    public Highscore highscore;
    public Text[] highscoreTexts;
    //public Text highscoreText;
    public Text newHighscoreText;

    [Header("Cheat System")]
    public CheatSystem cheatsComponent;
    public Image fairyButton;
    public Image towerButton;
    public Image timeButton;

    [Header("Upgrade System")]
    public Text upgradePointsText;

    [Header("Max Fairies Upgrade")]
    public Text fairiesUpgradeText;
    public Text meteorCooldownText;
    public Text fireRateUpgradeText;
    UpgradeSystem upgrades;

    // Start is called before the first frame update
    void Start()
    {
        upgrades = UpgradeSystem.Get();
        highscore = Highscore.Get();
        if(newHighscoreText)
        {
            newHighscoreText.enabled = false;
        }

        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        if(cheatsComponent)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                cheatsComponent.SwitchScreen();
            }
        }

        if (newHighscoreText)
        {
            if (highscoreTexts.Length > 0)
            {
                for (int i = 0; i < highscoreTexts.Length; i++)
                {
                    highscoreTexts[i].text = "" + highscore.highscore;

                }

                if (highscore.hasNewHighscore)
                {
                    newHighscoreText.enabled = true;
                }
            }
        }

            

    }

    public void CheatsReloadMeteor()
    {
        cheatsComponent.ReloadMeteor();
    }

    public void CheatsSwitchFairySInvincibility()
    {
        cheatsComponent.SwitchFairiesInmunity();

        if(Fairy.isInmunityOn)
        {
            fairyButton.color = Color.green;
        }
        else
        {
            fairyButton.color = Color.white;
        }
    }

    public void CheatsStopWave()
    {
        cheatsComponent.StopWave();
    }

    public void CheatsGivePoints()
    {
        cheatsComponent.GivePoints();
    }

    public void CheatsSwitchTowerDeletion()
    {
        cheatsComponent.SwitchTowerDeletion();

        if(TurretSpawner.Get().canDelete)
        {
            towerButton.color = Color.green;
        }
        else
        {
            towerButton.color = Color.white;
        }
    }

    public void CheatsKillAllEnemies()
    {
        cheatsComponent.KillAllEnemies();
    }

    public void CheatsKillAllFairies()
    {
        cheatsComponent.KillAllFairies();
    }

    public void CheatsStopTime()
    {
        cheatsComponent.StopTime();

        if(cheatsComponent.isTimeNormal)
        {
            timeButton.color = Color.white;
        }
        else
        {
            timeButton.color = Color.green;
        }
    }

    private void OnDestroy()
    {
        //newHighscoreText.enabled = false;
        if(highscore)
        {
            highscore.ResetHighscoreBool();
        }
        
        //
    }

    public void UpdateText()
    {
        if(upgradePointsText)
        {
            upgradePointsText.text = "Upgrade Points: " + upgrades.upgradePoints;

            fairiesUpgradeText.text = upgrades.GetUpgradeName(upgrades.fairiesUpgrade) + "                          "
                + "Next Level: " + upgrades.GetNextUpgradeLevel(upgrades.fairiesUpgrade)
                + " Cost: " + ItExists(upgrades.fairiesUpgrade);

            meteorCooldownText.text = upgrades.GetUpgradeName(upgrades.meteorCooldownUpgrade) + "                          "
                + "Next Level: " + upgrades.GetNextUpgradeLevel(upgrades.meteorCooldownUpgrade)
                + " Cost: " + ItExists(upgrades.meteorCooldownUpgrade);


            fireRateUpgradeText.text = upgrades.GetUpgradeName(upgrades.towersFireRateUpgrade) + "                          "
                + "Next Level: " + upgrades.GetNextUpgradeLevel(upgrades.towersFireRateUpgrade)
                + " Cost: " + ItExists(upgrades.towersFireRateUpgrade);
        }
    }

    public void UpgradeBuyMaxFaires()
    {
        upgrades.BuyUpgrade(upgrades.fairiesUpgrade);
        UpdateText();
    }

    public void UpgradeBuyMeteorCooldown()
    {
        upgrades.BuyUpgrade(upgrades.meteorCooldownUpgrade);
        UpdateText();
    }

    public void UpgradeBuyFireRate()
    {
        upgrades.BuyUpgrade(upgrades.towersFireRateUpgrade);
        UpdateText();
    }

    private string ItExists(UpgradeSystem.Upgrade upgrade)
    {
        if(upgrades.GetNextLevelUpgradeCost(upgrade) == 0)
        {
            return "???";
        }
        else
        {
            return "" + upgrades.GetNextLevelUpgradeCost(upgrade);
        }
    }
}
