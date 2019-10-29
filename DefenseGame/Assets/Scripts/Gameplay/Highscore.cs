using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscore : MonoBehaviourSingleton<Highscore>
{
    public int highscore;
    public bool hasNewHighscore;
    private bool doOnce;

    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        //gameManager = GameManager.Get();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Get())
        {
            GetHighscore();

            if (GameManager.Get().score >= highscore)
            {
                highscore = GameManager.Get().score;
                PlayerPrefs.SetInt("highscore", highscore);
                hasNewHighscore = true;
                if(!doOnce)
                {
                    GameManager.Get().isConfettiOn = true;
                    doOnce = true;
                }
            }
        }
    }

    public int GetHighscore()
    {
        highscore = PlayerPrefs.GetInt("highscore");

        return highscore;
    }

    public void ResetHighscoreBool()
    {
        hasNewHighscore = false;
    }
}
