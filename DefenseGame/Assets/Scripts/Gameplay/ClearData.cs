using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearData : MonoBehaviour
{
    public GameObject button;
    private UpgradeSystem upgrades;
    private FirstTimePlayingCheck firstTime;
    private int buttonAppearCounter;
    public bool firstTimePlaying;
    

    // Start is called before the first frame update
    void Start()
    {
        firstTime = FirstTimePlayingCheck.Get();
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
        firstTime = FirstTimePlayingCheck.Get();

        if(upgrades)
        {
            upgrades.ResetUpgrades();
        }
        firstTime.isFirstTimePlaying = true;

        buttonAppearCounter = 0;
        button.SetActive(false);
    }
}
