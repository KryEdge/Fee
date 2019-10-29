using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseButton : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject warningMenu;
    public bool isSecondaryPauseButton;

    private bool isGamePaused;
    private GameObject myEventSystem;
    private GameObject primaryPauseButton;
    private ColorBlock originalColors;
    private void Start()
    {
        if (isSecondaryPauseButton)
        {
            primaryPauseButton = GameObject.Find("Pause Button");
        }
        else
        {
            myEventSystem = GameObject.Find("EventSystem");
            isGamePaused = false;
        }

        if(warningMenu)
        {
            warningMenu.SetActive(false);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!warningMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isSecondaryPauseButton)
                {
                    PauseGame();
                }
            }
        }
    }

    public void PauseGame()
    {
        if (!warningMenu.activeSelf)
        {
            if (!isSecondaryPauseButton)
            {
                isGamePaused = !isGamePaused;

                if (isGamePaused)
                {
                    Time.timeScale = 0;
                    pauseMenu.SetActive(true);
                    //ColorBlock colors = GetComponent<Button>().colors;
                    //originalColors = colors;
                    //colors.normalColor = Color.red;
                    //GetComponent<Button>().colors = colors;
                    myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
                }
                else
                {
                    Time.timeScale = 1;
                    pauseMenu.SetActive(false);
                    //GetComponent<Button>().colors = originalColors;
                    myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
                }
            }
        }
    }

    public void ContinueGame()
    {
        if (!warningMenu.activeSelf)
        {
            if (isSecondaryPauseButton)
            {
                primaryPauseButton.GetComponent<UIPauseButton>().PauseGame();
            }
        }
        
    }

    public void OpenWarningMenu()
    {
        if (warningMenu)
        {
            warningMenu.SetActive(true);
        }
    }

    public void CloseWarningMenu()
    {
        if (warningMenu)
        {
            warningMenu.SetActive(false);
        }
    }
}