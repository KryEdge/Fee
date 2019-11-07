using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Gems System")]
    public Text gemsText;

    /*[Header("Milestones Panel")]
    public GameObject milestonePanel;
    private Animator milestoneAnimator;*/

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

    [Header("Upgrades")]
    public List<Text> upgradesText;
    public Text fairiesUpgradeText;
    public Text meteorCooldownText;
    public Text fireRateUpgradeText;
    public Text fairySpeedUpgradeText;
    UpgradeSystem upgrades;

    [Header("Settings")]
    public SettingsScreen settings;
    public Toggle fullscreenButton;

    // Start is called before the first frame update
    void Start()
    {
        upgrades = UpgradeSystem.Get();
        highscore = Highscore.Get();
       // milestoneAnimator = milestonePanel.GetComponent<Animator>();
        if(GameManager.Get())
        {
            GameManager.Get().OnLevelGameOver = UpdateText;
        }
        

        if(newHighscoreText)
        {
            newHighscoreText.enabled = false;
        }

        UpdateText();

        if(fullscreenButton)
        {
            if (Screen.fullScreen)
            {
                fullscreenButton.image.color = Color.green;
            }
            else
            {
                fullscreenButton.image.color = Color.white;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cheatsComponent)
        {
            if (Input.GetKeyDown(KeyCode.F12))
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

    public void UpdateText()
    {
        if(upgradePointsText)
        {
            upgradePointsText.text = "Upgrade Points: " + upgrades.upgradePoints;
        }

        if(gemsText)
        {
            gemsText.text = "Gems Collected: " + GameManager.Get().upgradePointsCurrentMatch;
        }
    }

    public void SettingsSwitchFullscreen()
    {
        settings.SwitchFullscreen();

        if(Screen.fullScreen)
        {
            fullscreenButton.image.color = Color.green;
        }
        else
        {
            fullscreenButton.image.color = Color.white;
        }
    }

    private void OnDestroy()
    {
        if (highscore)
        {
            highscore.ResetHighscoreBool();
        }
    }
}
