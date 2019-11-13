﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerGameplay : MonoBehaviour
{
    [Header("Gems")]
    public Text gemsText;

    [Header("Score")]
    public Text scoreText;

    [Header("Fairies")]
    public Text fairiesText;

    [Header("Towers")]
    public Text towersText;

    [Header("Tower Spawner")]
    private TurretSpawner spawner;

    [Header("Milestones Panel")]
    public KeyCode switchKey;
    public GameObject panel;
    private Animator animator;
    private bool animationSwitch;

    [Header("HighScore")]
    public Highscore highscore;
    public Text[] highscoreTexts;
    public Text newHighscoreText;

    [Header("Wave System")]
    public string initialText;
    public string startText;
    public string endText;
    public Text waveText;

    [Header("Cheat System")]
    public CheatSystem cheatsComponent;
    public Image fairyButton;
    public Image towerButton;
    public Image timeButton;

    [Header("Pause")]
    public UIPauseButton pause;

    [Header("Vignette")]
    public UIVignette vignette;

    [Header("Assign Components")]
    public CheatSystem cheats;
    private GameManager gm;
    private TurretSpawner turretSpawner;

    // Start is called before the first frame update
    void Start()
    {
        turretSpawner = TurretSpawner.Get();
        gm = GameManager.Get();
        spawner = TurretSpawner.Get();
        highscore = Highscore.Get();

        cheats.OnCheatGivePoints += UpdateText;
        gm.OnGameGivePoints += UpdateText;
        Fairy.OnFairyDeath += UpdateText;
        Fairy.OnFairySpawn += UpdateText;
        turretSpawner.OnSpawnerSpawnTurret = UpdateText;
        turretSpawner.OnSpawnerDeleteTurret = UpdateText;
        GameManager.Get().OnLevelGameOver += UpdateText;
        GameManager.Get().OnLevelGameOver += GameOverConfiguration;
        GameManager.Get().OnLastFairyAlive = ActivateVignette;
        WaveSystem.OnStartUI = ChangeWaveText;
        WaveSystem.OnEndUI = TurnOffUI;
        WaveSystem.OnEndWave = ChangeWaveText;

        animator = panel.GetComponent<Animator>();
        WaveSystem.OnStartWaveFirstTime += Hide;

        if (newHighscoreText)
        {
            newHighscoreText.enabled = false;
        }

        waveText.text = initialText;

        UpdateText();
    }

    private void Update()
    {
        if (cheatsComponent)
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

        if(Input.GetKeyDown(switchKey))
        {
            SwitchAnimation();
        }
    }

    public void UpdateText()
    {
        if(fairiesText)
        {
            fairiesText.text = GameManager.Get().currentFairies + "/" + GameManager.Get().maxFairies;
        }
        
        if(towersText)
        {
            towersText.text = TurretSpawner.Get().spawnedTurrets.Count + "/" + GameManager.Get().maxTurrets;
        }
        
        if(scoreText)
        {
            scoreText.text = "" + GameManager.Get().score;
        }

        if (gemsText)
        {
            gemsText.text = "Gems Collected: " + GameManager.Get().upgradePointsCurrentMatch;
        }
    }

    public void ChangeWaveText(int id)
    {
        switch (id)
        {
            case 0:
                if(waveText)
                {
                    waveText.enabled = true;
                    waveText.text = startText;
                }
                
                break;
            case 1:
                if (waveText)
                {
                    waveText.enabled = true;
                    waveText.text = endText;
                }
                    
                break;
            default:
                break;
        }
    }

    public void TurnOffUI()
    {
        waveText.enabled = false;
    }

    public void SwitchAnimation()
    {
        animationSwitch = !animationSwitch;

        if (animationSwitch)
        {
            animator.SetBool("isHiding", true);
            animator.SetBool("isShowing", false);
        }
        else
        {
            animator.SetBool("isHiding", false);
            animator.SetBool("isShowing", true);
        }
    }

    private void Hide()
    {
        if (!animationSwitch)
        {
            animationSwitch = true;

            animator.SetBool("isHiding", true);
            animator.SetBool("isShowing", false);
        }
    }

    public void SwitchSpawner()
    {
        spawner.SwitchPreview();
    }

    public void CheatsReloadMeteor()
    {
        cheatsComponent.ReloadMeteor();
    }

    public void CheatsSwitchFairySInvincibility()
    {
        cheatsComponent.SwitchFairiesInmunity();

        if (Fairy.isInmunityOn)
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

        if (TurretSpawner.Get().canDelete)
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

        if (cheatsComponent.isTimeNormal)
        {
            timeButton.color = Color.white;
        }
        else
        {
            timeButton.color = Color.green;
        }
    }

    public void GameOverConfiguration()
    {
        pause.pauseMenu.SetActive(false);
        pause.enabled = false;
        vignette.SwitchMask();
    }

    public void ActivateVignette()
    {
        vignette.SetLowHealthColor();
        vignette.SwitchMask();
    }

    public void OnDestroy()
    {
        Fairy.OnFairyDeath -= UpdateText;
        Fairy.OnFairySpawn -= UpdateText;
        cheats.OnCheatGivePoints -= UpdateText;
        gm.OnGameGivePoints -= UpdateText;
        WaveSystem.OnStartWaveFirstTime -= Hide;
        GameManager.Get().OnLevelGameOver -= UpdateText;
        GameManager.Get().OnLevelGameOver -= GameOverConfiguration;

        if (highscore)
        {
            highscore.ResetHighscoreBool();
        }
    }
}