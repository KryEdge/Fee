using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPages : MonoBehaviour
{
    public delegate void OnTutorialAction();
    public OnTutorialAction OnTutorialFinished;

    public GameObject panel;
    public GameObject[] pages;
    public int currentPage;
    public bool automaticClose;
    public bool setPlayerPref;
    public bool isCurrentlyOpen;
    public bool needsToPauseGame;

    // Start is called before the first frame update
    void Start()
    {
        if(!needsToPauseGame)
        {
            Debug.Log("Pausing from tutorialPages");
            CheckFirstTimePlaying();
        }
        
    }

    public void OpenTutorial()
    {
        currentPage = 0;

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }

        pages[currentPage].SetActive(true);

        if (panel)
        {
            panel.SetActive(true);
        }

        isCurrentlyOpen = true;
    }

    public void CloseTutorial()
    {
        if(panel)
        {
            panel.SetActive(false);
        }

        isCurrentlyOpen = false;

        if(needsToPauseGame)
        {
            if(OnTutorialFinished != null)
            {
                Debug.Log("Oh damn CLOSING IN");
                OnTutorialFinished();
            }
        }
    }

    public void NextPage()
    {
        pages[currentPage].SetActive(false);

        currentPage++;

        if(currentPage >= pages.Length)
        {
            if(automaticClose)
            {
                CloseTutorial();
            }

            currentPage = 0;
        }

        pages[currentPage].SetActive(true);
    }

    public void PreviousPage()
    {
        pages[currentPage].SetActive(false);

        currentPage--;

        if (currentPage < 0)
        {
            currentPage++;
        }

        pages[currentPage].SetActive(true);
    }

    public bool CheckFirstTimePlaying()
    {
        Debug.Log("ENTERING CHECKFIRST TIME PLAYING");
        int goToTutorial = PlayerPrefs.GetInt("isFirstTimePlaying", 1);

        if (goToTutorial == 1)
        {
            Debug.Log("opening");
            OpenTutorial();

            if (setPlayerPref)
            {
                PlayerPrefs.SetInt("isFirstTimePlaying", 0);
            }

            return true;
        }
        else if (goToTutorial == 0)
        {
            Debug.Log("closing");
            CloseTutorial();
        }

        return false;
    }
}
