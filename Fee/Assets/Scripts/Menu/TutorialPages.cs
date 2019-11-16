using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPages : MonoBehaviour
{
    public GameObject panel;
    public GameObject[] pages;
    public int currentPage;
    public bool automaticClose;
    public bool setPlayerPref;

    // Start is called before the first frame update
    void Start()
    {
        string goToTutorial = PlayerPrefs.GetString("isFirstTimePlaying", "yes");

        if (goToTutorial == "yes")
        {
            Debug.Log("opening");
            OpenTutorial();

            if(setPlayerPref)
            {
                PlayerPrefs.SetString("isFirstTimePlaying", "no");
            }
        }
        else if (goToTutorial == "no")
        {
            Debug.Log("closing");
            CloseTutorial();
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
    }

    public void CloseTutorial()
    {
        if(panel)
        {
            panel.SetActive(false);
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
}
