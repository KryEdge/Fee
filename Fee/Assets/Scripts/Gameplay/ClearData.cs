using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearData : MonoBehaviour
{
    public GameObject button;
    public bool firstTimePlaying;

    private UpgradeSystem upgrades;
    private int buttonAppearCounter;

    // Start is called before the first frame update
    private void Start()
    {
        string goToTutorial = PlayerPrefs.GetString("isFirstTimePlaying", "yes");

        if (goToTutorial == "yes")
        {
            PlayerPrefs.SetString("isFirstTimePlaying", "yes");
        }
        else if (goToTutorial == "no")
        {
            PlayerPrefs.SetString("isFirstTimePlaying", "no");
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

        PlayerPrefs.SetString("isFirstTimePlaying", "yes");

        buttonAppearCounter = 0;
        button.SetActive(false);
    }
}
