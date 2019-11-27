using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerMenu : MonoBehaviour
{
    [Header("General Settings")]
    public Button upgradesButton;

    // Start is called before the first frame update
    private void Start()
    {
        string goToTutorial = PlayerPrefs.GetString("isFirstTimePlaying", "yes"); // pasarlo a int

        if (goToTutorial == "yes")
        {
            upgradesButton.interactable = false;
            upgradesButton.image.color = Color.gray;
        }
        else if (goToTutorial == "no")
        {
            upgradesButton.interactable = true;
            upgradesButton.image.color = Color.white;
        }
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/
}
