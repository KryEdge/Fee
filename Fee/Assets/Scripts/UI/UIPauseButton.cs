using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseButton : MonoBehaviour
{
    [Header("General Settings")]
    public TutorialPages tutorial;
    public GameObject pauseMenu;
    public GameObject warningMenu;
    public GameObject warningMenu2;
    public bool isSecondaryPauseButton;

    [Header("Sound Settings")]
    public GameObject pauseON;
    public GameObject pauseOFF;

    [Header("Checking Variables")]
    public static bool isGamePaused;
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

        if (warningMenu2)
        {
            warningMenu2.SetActive(false);
        }

        //TutorialButton();

        if(!isSecondaryPauseButton)
        {
            if (tutorial)
            {
                tutorial.OnTutorialFinished += ForcedUnpause;

                if (tutorial.CheckFirstTimePlaying())
                {
                    Debug.Log("CHECKING IN PAUSE BUTTON");
                    PauseGame(true);
                }
            }
        }

        //AkSoundEngine.PostEvent("pausa_off", pauseOFF);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!warningMenu.activeSelf && !warningMenu2.activeSelf)
        {
            if(tutorial)
            {
                if(!tutorial.isCurrentlyOpen)
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
        }       
    }

    public void PauseGame()
    {
        if (!warningMenu.activeSelf && !warningMenu2.activeSelf)
        {
            if (!isSecondaryPauseButton)
            {
                isGamePaused = !isGamePaused;

                if (isGamePaused)
                {
                    AkSoundEngine.PostEvent("pausa_on", pauseON);

                    if (CheatSystem.isTimeNormal)
                    {
                        Debug.Log("SETTING TIME SCALE TO 0 (REAL ONE)");
                        Time.timeScale = 0;
                    }

                    pauseMenu.SetActive(true);
                    myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

                }
                else
                {
                    AkSoundEngine.PostEvent("pausa_off", pauseOFF);
                    if (CheatSystem.isTimeNormal)
                    {
                        Debug.Log("SETTING TIME SCALE TO 1 (REAL ONE)");
                        Time.timeScale = 1;
                    }

                    pauseMenu.SetActive(false);
                    myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
                }
            }
        }
    }

    public void PauseGame(bool state)
    {
        if (!warningMenu.activeSelf && !warningMenu2.activeSelf)
        {
            if (!isSecondaryPauseButton)
            {
                Debug.Log("PAUSING");
                isGamePaused = state;

                if (isGamePaused)
                {
                    AkSoundEngine.PostEvent("pausa_on", pauseON);
                    Debug.Log("REALLY PAUSING");

                    if (CheatSystem.isTimeNormal)
                    {
                        Debug.Log("SETTING TIME SCALE TO 0");
                        Time.timeScale = 0;
                    }

                    //pauseMenu.SetActive(true);
                    myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

                }
                else
                {
                    AkSoundEngine.PostEvent("pausa_off", pauseOFF);
                    if (CheatSystem.isTimeNormal)
                    {
                        Debug.Log("SETTING TIME SCALE TO 1");
                        Time.timeScale = 1;
                    }

                    //pauseMenu.SetActive(false);
                    myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
                }
            }
        }
    }

    public void ContinueGame()
    {
        if (!warningMenu.activeSelf && !warningMenu2.activeSelf)
        {
            if (isSecondaryPauseButton)
            {
                Debug.Log("COntinuining");
                AkSoundEngine.PostEvent("pausa_off", pauseOFF);
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

    public void OpenWarningMenu2()
    {
        if (warningMenu2)
        {
            warningMenu2.SetActive(true);
        }
    }

    public void CloseWarningMenu2()
    {
        if (warningMenu2)
        {
            warningMenu2.SetActive(false);
        }
    }

    private void ForcedUnpause()
    {
        Debug.Log("FORCE UNPAUSE");
        PauseGame(false);
    }

    private void OnDestroy()
    {
        tutorial.OnTutorialFinished -= ForcedUnpause;
    }

    public void ShowTutorialOnPause()
    {
        Debug.Log("TUTORIAL BUTTON");
        tutorial.OpenTutorial();
        PauseGame(true);
    }
}