using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("HighScore")]
    public Highscore highscore;
    public Text highscoreText;
    public Text newHighscoreText;

    [Header("Cheat System")]
    public CheatSystem cheatsComponent;
    public Image fairyButton;
    public Image towerButton;
    public Image timeButton;

    // Start is called before the first frame update
    void Start()
    {
        highscore = Highscore.Get();
        if(newHighscoreText)
        {
            newHighscoreText.enabled = false;
        }
        
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

        highscoreText.text = "" + highscore.highscore;
        if(highscore.hasNewHighscore)
        {
            newHighscoreText.enabled = true;
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
        highscore.ResetHighscoreBool();
        //
    }
}
