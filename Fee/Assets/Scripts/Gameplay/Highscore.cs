using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscore : MonoBehaviourSingleton<Highscore>
{
    public int highscore;
    public bool hasNewHighscore;

    private bool doOnce;

    // Start is called before the first frame update
    private void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
    }

    // Update is called once per frame
    private void Update()
    {
        if(GameManager.Get())
        {
            GetHighscore();

            if (GameManager.Get().score >= highscore)
            {
                if (!doOnce)
                {
                    if (highscore != 0)
                    {
                        GameManager.Get().isConfettiOn = true;
                    }

                    doOnce = true;
                }

                highscore = GameManager.Get().score;
                PlayerPrefs.SetInt("highscore", highscore);
                hasNewHighscore = true;
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
