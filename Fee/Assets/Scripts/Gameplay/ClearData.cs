using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearData : MonoBehaviour
{
    public GameObject button;
    public bool firstTimePlaying;

    private UpgradeSystem upgrades;
    private int buttonAppearCounter;

    // Start is called before the first frame update
    private void Start()
    {
        int goToTutorial = PlayerPrefs.GetInt("isFirstTimePlaying", 1);

        if (goToTutorial == 1)
        {
            PlayerPrefs.SetInt("isFirstTimePlaying", 1);
        }
        else if (goToTutorial == 0)
        {
            PlayerPrefs.SetInt("isFirstTimePlaying", 0);
        }

        Debug.Log(goToTutorial);
        upgrades = UpgradeSystem.Get();
        button.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            buttonAppearCounter++;

            if(buttonAppearCounter >= 3)
            {
                button.SetActive(true);
            }
        }
    }

    public void ResetEverything()
    {
        upgrades = UpgradeSystem.Get();

        if(upgrades)
        {
            upgrades.ResetUpgrades();
        }

        PlayerPrefs.SetInt("isFirstTimePlaying", 1);

        buttonAppearCounter = 0;
        button.SetActive(false);

        SceneManager.LoadScene("Home Screen");
    }
}
