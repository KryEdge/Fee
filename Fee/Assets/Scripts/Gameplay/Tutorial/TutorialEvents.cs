using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvents : MonoBehaviour
{
    public enum pages
    {
        challenges,
        meteor,
        towers,
        switchTool,
        allPages
    }

    public GameObject[] tutorialText;
    public bool[] isAlreadyOpened;
    public bool automaticClose;
    public bool isClosed;
    public bool isFirstTimeEnabled;
    public int currentPage;

    // Start is called before the first frame update
    void Start()
    {
        Shoot.OnShootMeteor += GoToMeteor;
        GameManager.Get().turretSpawner.OnSpawnerSpawnTurret += GoToTowers;
        TurretSpawner.OnSpawnerSwitchTool += GoToSwitchTool;
        UIManagerGameplay.OnUICloseChallenges += CloseChallengeTutorial;

        CloseAllPages();
        CheckFirstTimePlaying();
    }

    private void CloseAllPages()
    {
        for (int i = 0; i < tutorialText.Length; i++)
        {
            tutorialText[i].SetActive(false);
        }
    }

    private void GoToPage(int page)
    {
        if(page >= 0 && page < tutorialText.Length)
        {
            CloseAllPages();
            tutorialText[page].SetActive(true);
        }
        else
        {
            Debug.Log("Tutorial Page Doesn't Exist.");
        }
    }

    public void NextPage()
    {
        CloseAllPages();

        if (isClosed)
        {
            tutorialText[currentPage].SetActive(true);
            isClosed = false;
        }
        else
        {
            tutorialText[currentPage].SetActive(false);

            currentPage++;

            if (currentPage >= tutorialText.Length)
            {
                if (automaticClose)
                {
                    CloseAllPages();
                    isClosed = true;
                }

                currentPage = 0;
            }

            if (!isClosed)
            {
                tutorialText[currentPage].SetActive(true);
            }
        }
    }

    public void GoToChallenges()
    {
        if (isFirstTimeEnabled)
        {
            if (!isAlreadyOpened[(int)pages.challenges])
            {
                GoToPage((int)pages.challenges);
                isAlreadyOpened[(int)pages.challenges] = true;
            }
        }
    }

    public void GoToMeteor()
    {
        if (isFirstTimeEnabled)
        {
            if (!isAlreadyOpened[(int)pages.meteor])
            {
                GoToPage((int)pages.meteor);
                isAlreadyOpened[(int)pages.meteor] = true;
            }
        }
    }

    public void GoToTowers()
    {
        if(isFirstTimeEnabled)
        {
            if (!isAlreadyOpened[(int)pages.towers])
            {
                GoToPage((int)pages.towers);
                isAlreadyOpened[(int)pages.towers] = true;
            }
        }
    }

    public void GoToSwitchTool()
    {
        if (isFirstTimeEnabled)
        {
            if (!isAlreadyOpened[(int)pages.switchTool])
            {
                GoToPage((int)pages.switchTool);
                isAlreadyOpened[(int)pages.switchTool] = true;
            }
        }
    }

    private bool CheckFirstTimePlaying()
    {
        Debug.Log("ENTERING CHECKFIRST TIME PLAYING");
        int goToTutorial = PlayerPrefs.GetInt("isFirstTimePlaying", 1);

        if (goToTutorial == 1)
        {
            Debug.Log("opening");
            GoToPage((int)pages.challenges);
            isFirstTimeEnabled = true;

            return true;
        }
        else if (goToTutorial == 0)
        {
            Debug.Log("closing");
            isFirstTimeEnabled = false;
        }

        return false;
    }

    public void CloseChallengeTutorial()
    {
        tutorialText[(int)pages.challenges].SetActive(false);
    }

    private void OnDestroy()
    {
        Shoot.OnShootMeteor -= GoToMeteor;
        GameManager.Get().turretSpawner.OnSpawnerSpawnTurret -= GoToTowers;
        TurretSpawner.OnSpawnerSwitchTool -= GoToSwitchTool;
        UIManagerGameplay.OnUICloseChallenges -= CloseChallengeTutorial;
    }
}
