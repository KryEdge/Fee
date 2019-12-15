using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvents : MonoBehaviour
{
    public delegate void OnEventAction();
    public static OnEventAction OnEventMeteorClose;
    public static OnEventAction OnEventTowerClose;
    public static OnEventAction OnEventSwitchToolClose;

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
        Shoot.OnShootMeteorSecond += CloseMeteorTutorial;
        TurretSpawner.OnSpawnerSwitchToolSecond += CloseSwitchToolTutorial;
        GameManager.Get().turretSpawner.OnSpawnerSpawnTurretSecond += CloseTowersTutorial;

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
        ChangeAllFirstTimeTutorialsState(true);

        switch (currentPage)
        {
            case (int)pages.challenges:
                break;
            case (int)pages.meteor:
                if (OnEventMeteorClose != null)
                {
                    OnEventMeteorClose();
                }
                break;
            case (int)pages.towers:
                if (OnEventTowerClose != null)
                {
                    OnEventTowerClose();
                }
                break;
            case (int)pages.switchTool:
                if (OnEventSwitchToolClose != null)
                {
                    OnEventSwitchToolClose();
                }
                break;
            default:
                break;
        }

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

    public void CloseSwitchToolTutorial()
    {
        tutorialText[(int)pages.switchTool].SetActive(false);
    }

    public void CloseMeteorTutorial()
    {
        tutorialText[(int)pages.meteor].SetActive(false);
    }

    public void CloseTowersTutorial()
    {
        tutorialText[(int)pages.towers].SetActive(false);
    }

    public void ChangeAllFirstTimeTutorialsState(bool state)
    {
        for (int i = 0; i < isAlreadyOpened.Length; i++)
        {
            isAlreadyOpened[i] = state;
        }
    }

    private void OnDestroy()
    {
        Shoot.OnShootMeteor -= GoToMeteor;
        GameManager.Get().turretSpawner.OnSpawnerSpawnTurret -= GoToTowers;
        TurretSpawner.OnSpawnerSwitchTool -= GoToSwitchTool;
        UIManagerGameplay.OnUICloseChallenges -= CloseChallengeTutorial;
        Shoot.OnShootMeteorSecond -= CloseMeteorTutorial;
        TurretSpawner.OnSpawnerSwitchToolSecond -= CloseSwitchToolTutorial;
        GameManager.Get().turretSpawner.OnSpawnerSpawnTurretSecond -= CloseTowersTutorial;
    }
}
